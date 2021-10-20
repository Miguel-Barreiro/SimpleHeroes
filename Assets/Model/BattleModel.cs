using System;
using System.Collections.Generic;
using Gram.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gram.Model
{
    
    public class BattleModel : IBattleModel
    {

        public event GameBasics.SingleParameterDelegate<BattleTurn> OnNewTurnExecuted;
        
        [Serializable]
        private class State
        {
            [SerializeField]
            public Enemy Enemy;
            [SerializeField]
            public List<string> ParticipatingHeroNameIds = new List<string>();

            [SerializeField]
            public BattleTurn LastTurn;
        }

        public void GenerateInitialState() {
            _state = new State();
        }

        public BattleModel(HeroCollectionModel heroCollectionModel, GameDefinitions gameDefinitions) {
            _heroCollectionModel = heroCollectionModel;
            _gameDefinitions = gameDefinitions;
        }

        public void GenerateBattle(List<string> participatingHeroNameIds, Enemy enemy) {
            enemy.Heal();
            _state = new State() {
                Enemy = enemy,
                ParticipatingHeroNameIds = participatingHeroNameIds
            };

            FillParticipatingHeroesCache();

            foreach (Hero hero in _participatingHeroesCache) {
                hero.Heal();
            }
        }


        public Enemy GetEnemy() {
            return _state.Enemy;
        }

        public BattleTurn GetLastTurn() {
            return _state.LastTurn;
        }
        

        //to reduce garbage/ improve perfomance
        private readonly List<Hero> _participatingHeroesCache = new List<Hero>();

        private void FillParticipatingHeroesCache() {
            _participatingHeroesCache.Clear();
            Hero[] participatingHeroes = _heroCollectionModel.GetHeroesByNameId(_state.ParticipatingHeroNameIds);
            _participatingHeroesCache.AddRange(participatingHeroes);
        }

        public Hero[] GetParticipatingHeroes() {
            return _participatingHeroesCache.ToArray();
        }


        //-------------------------------------------------------------------------------

        #region Execute Turn

        public void ExecuteTurn(string chosenHeroNameId) {
            Hero chosenHero = GetAvailableAttackingHeroByNameId(chosenHeroNameId);
            if (chosenHero == null) {
                return;
            }
            Enemy enemy = _state.Enemy;
            
            int damageToEnemy = chosenHero.AttackPower;
            enemy.Damage(damageToEnemy);
            
            BattleTurn battleTurn = new BattleTurn() {
                HeroNameIdAttack = chosenHeroNameId,
                DamageToEnemy = damageToEnemy,
                NewEnemyHealth = enemy.GetCurrentHealth(),
                NewEnemyHealthPercentage = enemy.GetCurrentHealthPercentage()
            };
            
            if (enemy.IsAlive()) {
                HandleEnemyAttack(battleTurn);
            } else {
                HandleEnemyDead(battleTurn);
            }
        }

        private void HandleEnemyAttack(BattleTurn battleTurn) {
            Hero attackedHero = GetRandomAliveHero();

            if (attackedHero != null) {
                int damage = _state.Enemy.AttackPower;
                HandleHeroDamage(battleTurn, attackedHero, damage);
            } else {
                Debug.LogError("Battle is in an invalid state where no player is alive before any enemy attacks");
            }
        }

        private void HandleHeroDamage(BattleTurn battleTurn, Hero attackedHero, int damage) {
            attackedHero.Damage(damage);

            BattleTurn.EnemyAttack enemyAttack = new BattleTurn.EnemyAttack() {
                HeroNameId = attackedHero.CharacterNameId,
                Damage = damage, 
                NewHeroHealth = attackedHero.CurrentHealth,
                NewHeroHealthPercentage = attackedHero.GetCurrentHealthPercentage()
            };
            
            battleTurn.EnemyAttacks.Add(enemyAttack);
            
            if (IsAnyHeroLeft()) {
                battleTurn.BattleEnd = false;
            } else {
                battleTurn.BattleEnd = true;
                battleTurn.BattleEndResult = new BattleTurn.BattleResult() {
                    PlayerWon = false,
                };
                FillHeroResults(battleTurn);
            }

            _state.LastTurn = battleTurn;
            OnNewTurnExecuted?.Invoke(battleTurn);
        }

        private void HandleEnemyDead(BattleTurn battleTurn) {
            battleTurn.BattleEnd = true;
            battleTurn.BattleEndResult = new BattleTurn.BattleResult() {
                PlayerWon = true
            };

            FillHeroResults(battleTurn);

            _state.LastTurn = battleTurn;
            OnNewTurnExecuted?.Invoke(battleTurn);
        }

        private void FillHeroResults(BattleTurn battleTurn) {
            
            Hero[] participatingHeroes = GetParticipatingHeroes();
            
            foreach (Hero hero in participatingHeroes) {

                Debug.Log("battle end for " + hero.CharacterNameId);
                
                BattleTurn.HeroResult heroResult = new BattleTurn.HeroResult() {
                    Hero = hero
                };
                battleTurn.BattleEndResult.HeroResults.Add(heroResult);

                if (hero.IsAlive()) {
                    AwardExperience(hero, _gameDefinitions.ExperienceGainedPerBattle, 
                                    out heroResult.LevelsGained, 
                                    out heroResult.AttackPowerGained, 
                                    out heroResult.HealthGained );
                    heroResult.ExperienceGained = _gameDefinitions.ExperienceGainedPerBattle;
                }
            }
        }

        private void AwardExperience(Hero hero, int experienceReward, out int levelsGained, out int attackPowerGained, out int healthGained) {
            hero.Experience += experienceReward;
            healthGained = 0;
            attackPowerGained = 0;
            if (hero.Experience >= _gameDefinitions.ExperienceLevelUp) {
                levelsGained = Math.DivRem(hero.Experience, _gameDefinitions.ExperienceLevelUp, out int experienceLeft);
                hero.Experience =  experienceLeft;
                for (int i = 0; i < levelsGained; i++) {
                    int heroAttackPowerDelta = Mathf.CeilToInt(hero.AttackPower * _gameDefinitions.AttackPowerPercentageGainedPerLevel);
                    int healthGainedDelta= Mathf.CeilToInt(hero.Health *  _gameDefinitions.HealthPercentageGainedPerLevel);
                    attackPowerGained += heroAttackPowerDelta;
                    healthGained += healthGainedDelta;
                    hero.AttackPower += heroAttackPowerDelta;
                    hero.Health+=healthGainedDelta;
                }
                hero.Level+= levelsGained;
            } else {
                levelsGained = 0;
            }
        }

        #endregion

        //-------------------------------------------------------------------------------

        #region Private Utils


        private bool IsAnyHeroLeft() {
            Hero[] heroes = GetParticipatingHeroes();
            int aliveHeroes = 0;
            foreach (Hero hero in heroes) {
                if (hero.IsAlive()) {
                    aliveHeroes++;
                }
            }
            return aliveHeroes> 0;
        }

        private Hero GetRandomAliveHero() {
            Hero[] possibleHeroes = GetParticipatingHeroes();
            List<Hero> heroList = new List<Hero>(possibleHeroes);
            heroList.RemoveAll(possibleHero => !possibleHero.IsAlive());
            if (heroList.Count > 0) {
                int randomIndex = Random.Range(0, heroList.Count);
                return heroList[randomIndex];
            } else {
                return null;
            }
        }

        private Hero GetAvailableAttackingHeroByNameId(string chosenHeroNameId) {
            Hero hero = _heroCollectionModel.GetHeroByNameId(chosenHeroNameId);
            if (hero != null && hero.IsAlive()) {
                return hero;
            } else {
                return null;
            }
        }
        

        #endregion

        public string GetSerializedGameState() {
            return JsonUtility.ToJson(_state);
        }
        public void RestoreGameState(string newGameState) {
            State state = JsonUtility.FromJson<State>(newGameState);
            _state = state;
            FillParticipatingHeroesCache();
        }

        private GameDefinitions _gameDefinitions;
        private IHeroCollectionModel _heroCollectionModel;
        private State _state;

    }
}