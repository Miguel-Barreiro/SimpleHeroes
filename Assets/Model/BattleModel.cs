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
            public List<int> HeroIds;
        }
        

        public BattleModel(HeroCollectionModel heroCollectionModel) {
            _heroCollectionModel = heroCollectionModel;
        }

        public void GenerateBattle(List<int> participatingHeroes, Enemy enemy) {
            enemy.Heal();
            _state = new State() {
                Enemy = enemy,
                HeroIds = participatingHeroes
            };
            List<Hero> heroes = GetHeroes();
            foreach (Hero hero in heroes) {
                hero.Heal();
            }
        }

        public Enemy GetEnemy() {
            return _state.Enemy;
        }


        public List<Hero> GetHeroes() {
            return _heroCollectionModel.GetHeroesById(_state.HeroIds);
        }


        //-------------------------------------------------------------------------------

        #region Execute Turn

        public void ExecuteTurn(int chosenHeroIndex) {
            Hero chosenHero = GetAvailableAttackingHero(chosenHeroIndex);
            if (chosenHero == null) {
                return;
            }

            int damageToEnemy = chosenHero.AttackPower;

            _state.Enemy.Damage(damageToEnemy);
            if (!_state.Enemy.IsAlive()) {
                HandleEnemyDead(chosenHeroIndex, damageToEnemy);
            } else {
                HandleEnemyAttack(chosenHeroIndex, damageToEnemy);
            }
        }

        private void HandleEnemyAttack(int chosenHeroIndex, int damageToEnemy) {
            int id = GetRandomAliveHero(out Hero attackedHero);
            int damage = _state.Enemy.AttackPower;

            if (attackedHero != null) {
                HandleHeroDamage(chosenHeroIndex, damageToEnemy, attackedHero, damage, id);
            } else {
                Debug.LogError("Battle is in an invalid state where no player is alive");
            }
        }

        private void HandleHeroDamage(int chosenHeroIndex, int damageToEnemy, Hero attackedHero, int damage, int id) {
            attackedHero.Damage(damage);
            var heroHealthLeft = attackedHero.CurrentHealth;

            BattleTurn.EnemyAttack enemyAttack = new BattleTurn.EnemyAttack() {
                HeroIndex = id, 
                Damage = damage, 
                NewHeroHealth = heroHealthLeft,
                NewHeroHealthPercentage = heroHealthLeft/ (float)attackedHero.Health
            };
            BattleTurn battleTurn = new BattleTurn() {
                DamageToEnemy = damageToEnemy,
                HeroIndexAttack = chosenHeroIndex,
                NewEnemyHealth = _state.Enemy.CurrentHealth,
                NewEnemyHealthPercentage = _state.Enemy.CurrentHealth/(float)_state.Enemy.Health,
                EnemyAttacks = new List<BattleTurn.EnemyAttack>() { enemyAttack },
            };
            
            if (IsAnyHeroLeft()) {
                battleTurn.BattleEnd = false;
                battleTurn.BattleResult = new BattleResult() { };
            } else {
                battleTurn.BattleEnd = true;
                battleTurn.BattleResult = new BattleResult() { };
                
            }
            OnNewTurnExecuted?.Invoke(battleTurn);
        }

        private void HandleEnemyDead(int chosenHeroIndex, int damageToEnemy) {
            var battleTurn = new BattleTurn() {
                DamageToEnemy = damageToEnemy,
                HeroIndexAttack = chosenHeroIndex,
                NewEnemyHealth = _state.Enemy.Health,

                BattleEnd = true,
                BattleResult = new BattleResult() { }
            };
            OnNewTurnExecuted?.Invoke(battleTurn);
        }

        #endregion

        //-------------------------------------------------------------------------------

        #region Private Utils


        private bool IsAnyHeroLeft() {
            List<Hero> heroes = GetHeroes();
            heroes.RemoveAll(possibleHero => !possibleHero.IsAlive());
            return heroes.Count > 0;
        }

        private int GetRandomAliveHero(out Hero hero) {
            List<Hero> heroes = GetHeroes();
            heroes.RemoveAll(possibleHero => !possibleHero.IsAlive());
            if (heroes.Count > 0) {
                int randomIndex = Random.Range(0, heroes.Count);
                int id = _state.HeroIds[randomIndex];
                hero = _heroCollectionModel.GetHeroById(id);
                return id;
            } else {
                hero = null;
                return -1;
            }
        }

        private Hero GetAvailableAttackingHero(int chosenHeroIndex) {
            if (chosenHeroIndex >= 0 && chosenHeroIndex < _state.HeroIds.Count) {
                int chosenHeroId = _state.HeroIds[chosenHeroIndex];
                Hero chosenHero = _heroCollectionModel.GetHeroById(chosenHeroId);
                if (chosenHero.IsAlive()) {
                    return chosenHero;
                }
            }
            return null;
        }
        

        #endregion

        public string GetSerializedGameState() {
            return JsonUtility.ToJson(_state);
        }
        public void RestoreGameState(string newGameState) {
            State state = JsonUtility.FromJson<State>(newGameState);
            _state = state;
        }

        private IHeroCollectionModel _heroCollectionModel;
        private State _state;

    }
}