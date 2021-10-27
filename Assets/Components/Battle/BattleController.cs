using System;
using System.Collections;
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

        [SerializeField] 
        private BattleResultScreen BattleResultScreen;
        
        private IGameModel _gameModel;
        private IBattleModel _battleModel;

        
        
        private void Start() {
            _gameModel.OnLogicStateChange += OnLogicStateChange;
            
            _battleModel.OnNewTurnExecuted += ExecuteNewTurn;
            
            foreach (HeroBattleCharacter heroBattleCharacter in HeroBattleCharacters) {
                heroBattleCharacter.GetSelectableHero().OnSelected += OnSelectHero;
            }
        }

        private void Awake() {
            _battleModel = BasicDependencyInjector.Instance().GetObjectByType<IBattleModel>();
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();
        }

        //-------------------------------------------------------------------------------

        #region Turn Execution

        private void ExecuteNewTurn(BattleTurn turn) {
            // Debug.Log("new turn " + JsonUtility.ToJson(turn));
            StartCoroutine(TurnExecuteCoroutine(turn));
        }

        private IEnumerator TurnExecuteCoroutine(BattleTurn turn) {
            ToggleHeroSelection(false);
            
            bool executingAttacks = true;
            ExecuteAttacks(turn, () => { executingAttacks = false; });

            while ( executingAttacks) {
                yield return null;
            }
            
            if (turn.BattleEnd) {
                BattleResultScreen.ShowResult(turn);
            }
            ToggleHeroSelection(true);
        }

        private void ExecuteAttacks(BattleTurn turn, Action doneCallback) {

            HeroBattleCharacter attackingHero = GetHeroBattleCharacterByNameId(turn.HeroNameIdAttack);
            BattleCharacter attackedEnemy = EnemyBattleCharacters[0];

            PerformAttack(attackingHero, attackedEnemy, turn.DamageToEnemy, 
                          turn.NewEnemyHealth, turn.NewEnemyHealthPercentage,
                          () => {
                              ExecuteEnemyAttacks(turn, doneCallback);
                          });
        }


        private void ExecuteEnemyAttacks(BattleTurn turn, Action doneCallback) {
            StartCoroutine(EnemyAttacksCoroutine(turn, doneCallback));
        }
        
        private IEnumerator EnemyAttacksCoroutine(BattleTurn turn, Action doneCallback) {
            foreach (BattleTurn.EnemyAttack attack in turn.EnemyAttacks) {

                BattleCharacter heroAttacked = GetHeroBattleCharacterByNameId(attack.HeroNameId);
                
                bool attackInProgress = true;
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


        private void SetupBattleCharacters() {
            Hero[] participatingHeroes = _battleModel.GetParticipatingHeroes();
            for (int i = 0; i < participatingHeroes.Length; i++) {
                Hero hero = participatingHeroes[i];
                HeroBattleCharacter heroBattleCharacter = HeroBattleCharacters[i];
                heroBattleCharacter.Setup(hero);
                Vector3 localScale = heroBattleCharacter.GetVisuals().transform.localScale;
                heroBattleCharacter.GetVisuals().transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
            }

            Enemy enemy =_battleModel.GetEnemy();
            
            EnemyBattleCharacters[0].Setup(enemy);
        }

        private HeroBattleCharacter GetHeroBattleCharacterByNameId(string heroNameId) {
            foreach (HeroBattleCharacter heroBattleCharacter in HeroBattleCharacters) {
                if (heroBattleCharacter.GetSelectableHero().GetNameId().Equals(heroNameId)) {
                    return heroBattleCharacter;
                }
            }
            return null;
        }


        //-------------------------------------------------------------------------------

        #region Game Event handling


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
                case GameLogicState.ShowResult:
                    ShowBattleScreen();
                    BattleResultScreen.ShowResult(_battleModel.GetLastTurn());
                    break;
            }
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
            _gameModel.GotoHeroSelection();
        }

        private void OnDestroy() {
            foreach (HeroBattleCharacter heroBattleCharacter in HeroBattleCharacters) {
                heroBattleCharacter.GetSelectableHero().OnSelected -= OnSelectHero;
            }
        }

        #endregion
    }
}