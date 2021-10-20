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


        //to reduce garbage/ improve perfomance
        private readonly List<Hero> _participatingHeroesCache = new List<Hero>();

        private void FillParticipatingHeroesCache() {
            _participatingHeroesCache.Clear();
            List<Hero> participatingHeroes = _heroCollectionModel.GetHeroesByNameId(_state.ParticipatingHeroNameIds);
            _participatingHeroesCache.AddRange(participatingHeroes);
        }

        public List<Hero> GetParticipatingHeroes() {
            return _participatingHeroesCache;
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
            OnNewTurnExecuted?.Invoke(battleTurn);
        }

        private void HandleEnemyDead(BattleTurn battleTurn) {
            battleTurn.BattleEnd = true;
            battleTurn.BattleEndResult = new BattleTurn.BattleResult() {
                PlayerWon = true
            };

            FillHeroResults(battleTurn);
            
            OnNewTurnExecuted?.Invoke(battleTurn);
        }

        private void FillHeroResults(BattleTurn battleTurn) {
            foreach (Hero hero in GetParticipatingHeroes()) {
                BattleTurn.HeroResult heroResult = new BattleTurn.HeroResult() {
                    Hero = hero
                };
                battleTurn.BattleEndResult.HeroResults.Add(heroResult);

                if (hero.IsAlive()) {
                    heroResult.WasDead = false;
                    AwardExperience(hero, _gameDefinitions.ExperienceGainedPerBattle, out int levelsGained);
                    heroResult.LevelsGained = levelsGained;
                    heroResult.ExperienceGained = _gameDefinitions.ExperienceGainedPerBattle;
                } else {
                    heroResult.WasDead = true;
                }
            }
        }

        private void AwardExperience(Hero hero, int experienceReward, out int levelsGained) {
            hero.Experience += experienceReward;
            if (hero.Experience >= _gameDefinitions.ExperienceLevelUp) {
                int experienceLeft = Math.DivRem(hero.Experience, _gameDefinitions.ExperienceLevelUp, out levelsGained);
                hero.Experience =  experienceLeft;
                for (int i = 0; i < levelsGained; i++) {
                    hero.AttackPower += Mathf.CeilToInt(hero.AttackPower * _gameDefinitions.AttackPowerPercentageGainedPerLevel);
                    hero.Health+=Mathf.CeilToInt(hero.Health *  _gameDefinitions.HealthPercentageGainedPerLevel);
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
            List<Hero> heroes = GetParticipatingHeroes();
            heroes.RemoveAll(possibleHero => !possibleHero.IsAlive());
            return heroes.Count > 0;
        }

        private Hero GetRandomAliveHero() {
            List<Hero> possibleHeroes = GetParticipatingHeroes();
            possibleHeroes.RemoveAll(possibleHero => !possibleHero.IsAlive());
            if (possibleHeroes.Count > 0) {
                int randomIndex = Random.Range(0, possibleHeroes.Count);
                return possibleHeroes[randomIndex];
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