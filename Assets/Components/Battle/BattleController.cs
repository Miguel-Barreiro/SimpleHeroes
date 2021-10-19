using System;
using System.Collections;
using System.Collections.Generic;
using Gram.Core;
using Gram.Model;
using Gram.UI;
using Gram.Utils;
using UnityEngine;

namespace Gram.Battle
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField] private GameObject[] BattleGameObjects;

        [SerializeField] private HeroBattleCharacter[] HeroBattleCharacters;
        [SerializeField] private BattleCharacter[] EnemyBattleCharacters;
        
        private IGameModel _gameModel;
        private IBattleModel _battleModel;
        private ICharacterDatabase _characterDatabase;

        private void Start() {
            _gameModel.OnLogicStateChange += OnLogicStateChange;
            
            _battleModel.OnNewTurnExecuted += ExecuteNewTurn;
            
            foreach (HeroBattleCharacter heroBattleCharacter in HeroBattleCharacters) {
                heroBattleCharacter.GetSelectableHero().OnSelected += OnSelectHero;
            }
        }

        private void Awake() {
            _characterDatabase = BasicDependencyInjector.Instance().GetObjectByType<ICharacterDatabase>();
            
            _battleModel = BasicDependencyInjector.Instance().GetObjectByType<IBattleModel>();
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();
        }

        //-------------------------------------------------------------------------------

        #region Turn Execution

        private void ExecuteNewTurn(BattleTurn turn) {
            Debug.Log("new turn " + JsonUtility.ToJson(turn));

            ToggleHeroSelection(false);

            ExecutePlayerAttacks(turn, () => {ToggleHeroSelection(true);});
        }

        private void ExecutePlayerAttacks(BattleTurn turn, Action doneCallback) {

            HeroBattleCharacter attackingHero = GetHeroCharacterByNameId(turn.HeroNameIdAttack);
            BattleCharacter attackedEnemy = EnemyBattleCharacters[0];

            PerformAttack(attackingHero, attackedEnemy, turn.DamageToEnemy, 
                          turn.NewEnemyHealth, turn.NewEnemyHealthPercentage,
                          () => {
                              ExecuteEnemyAttacks(turn, doneCallback);
                          });
        }

        private HeroBattleCharacter GetHeroCharacterByNameId(string heroNameId) {
            foreach (HeroBattleCharacter heroBattleCharacter in HeroBattleCharacters) {
                if (heroBattleCharacter.GetSelectableHero().GetNameId().Equals(heroNameId)) {
                    return heroBattleCharacter;
                }
            }
            return null;
        }

        private void ExecuteEnemyAttacks(BattleTurn turn, Action doneCallback) {
            StartCoroutine(EnemyAttacksCoroutine(turn, doneCallback));
        }
        
        private IEnumerator EnemyAttacksCoroutine(BattleTurn turn, Action doneCallback) {
            foreach (BattleTurn.EnemyAttack attack in turn.EnemyAttacks) {
                
                bool attackInProgress = true;

                BattleCharacter heroAttacked = GetHeroCharacterByNameId(attack.HeroNameId);
                
                PerformAttack(EnemyBattleCharacters[0], heroAttacked, attack.Damage, 
                              attack.NewHeroHealth, attack.NewHeroHealthPercentage,
                              () => {
                                  attackInProgress = false;
                              });
                while (attackInProgress) {
                    yield return null;
                }
            }
            
            doneCallback?.Invoke();
        }

        private void PerformAttack(BattleCharacter attacker, BattleCharacter attacked, int damage, int newAttackedHealth, float newAttackedHealthPercentage, Action doneCallback) {
            attacker.Attack(damage, () => {
                attacked.Damage(damage, newAttackedHealth, newAttackedHealthPercentage,doneCallback);
            });
        }
        

        private void ToggleHeroSelection(bool activated) {
            foreach (HeroBattleCharacter heroBattleCharacter in HeroBattleCharacters) {
                heroBattleCharacter.GetSelectableHero().ToggleSelectable(activated);
            }
        }

        #endregion


        private void OnLogicStateChange() {
            GameLogicState logicState = _gameModel.GetCurrentLogicState();
            switch (logicState) {
                case GameLogicState.HeroSelection:
                    foreach (BattleCharacter battleCharacter in EnemyBattleCharacters) {
                        battleCharacter.ResetCharacter();
                    }
                    foreach (HeroBattleCharacter battleCharacter in HeroBattleCharacters) {
                        battleCharacter.ResetCharacter();
                    }
                    HideBattleScreen();
                    break;
                case GameLogicState.Battle:
                    SetupBattleCharacters();
                    ToggleHeroSelection(true);
                    ShowBattleScreen();
                    break;
            }
        }

        private void SetupBattleCharacters() {
            List<Hero> selectedHeroIndexes = _battleModel.GetHeroes();
            for (int i = 0; i < selectedHeroIndexes.Count; i++) {
                Hero hero = selectedHeroIndexes[i];
                HeroBattleCharacter heroBattleCharacter = HeroBattleCharacters[i];
                heroBattleCharacter.Setup(hero);
                Vector3 localScale = heroBattleCharacter.GetVisuals().transform.localScale;
                heroBattleCharacter.GetVisuals().transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
            }

            Enemy enemy =_battleModel.GetEnemy();


            EnemyBattleCharacters[0].Setup(enemy);
        }

        private void OnSelectHero(SelectableHero selectableHero) {
            _battleModel.ExecuteTurn(selectableHero.GetNameId());
        }
        
   
        private void HideBattleScreen() {
            foreach (GameObject battleGameObject in BattleGameObjects) {
                battleGameObject.SetActive(false);
            }
        }

        private void ShowBattleScreen() {
            foreach (GameObject battleGameObject in BattleGameObjects) {
                battleGameObject.SetActive(true);
            }
        }


        public void GoBackMenuSelection() {
            _gameModel.Retreat();
        }

        private void OnDestroy() {
            foreach (HeroBattleCharacter heroBattleCharacter in HeroBattleCharacters) {
                heroBattleCharacter.GetSelectableHero().OnSelected -= OnSelectHero;
            }
        }
    }
}