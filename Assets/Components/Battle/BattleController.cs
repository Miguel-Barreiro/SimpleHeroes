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
            
            _battleModel.OnNewTurnExecuted += BattleModelOnOnNewTurnExecuted;
            
            foreach (HeroBattleCharacter heroBattleCharacter in HeroBattleCharacters) {
                heroBattleCharacter.GetSelectableHero().OnSelected += OnSelectHero;
            }
        }

        private void Awake() {
            _characterDatabase = BasicDependencyInjector.Instance().GetObjectByType<ICharacterDatabase>();
            
            _battleModel = BasicDependencyInjector.Instance().GetObjectByType<IBattleModel>();
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();
        }

        private void BattleModelOnOnNewTurnExecuted(BattleTurn turn) {
            Debug.Log("new turn " + JsonUtility.ToJson(turn));

            ToggleHeroSelection(false);
            
            HeroBattleCharacters[turn.HeroIndexAttack].Attack(turn.DamageToEnemy, () => {
                EnemyBattleCharacters[0].Damage(turn.DamageToEnemy, turn.NewEnemyHealth, 
                                                    turn.NewEnemyHealthPercentage, () => {
                    StartCoroutine(EnemyAttacksCoroutine(turn, () => {
                        ToggleHeroSelection(true);
                    }));
                });
            });
        }


        private IEnumerator EnemyAttacksCoroutine(BattleTurn turn, Action doneCallback) {
            foreach (BattleTurn.EnemyAttack attack in turn.EnemyAttacks) {
                
                bool attackInProgress = true;
                EnemyBattleCharacters[0].Attack(attack.Damage, () => {
                    HeroBattleCharacters[turn.EnemyAttacks[0].HeroIndex].Damage(attack.Damage, attack.NewHeroHealth, 
                                                                                attack.NewHeroHealthPercentage,() => {
                        attackInProgress = false;
                    });
                });

                while (attackInProgress) {
                    yield return null;
                }
            }
            
            doneCallback?.Invoke();
        }

        private void ToggleHeroSelection(bool activated) {
            foreach (HeroBattleCharacter heroBattleCharacter in HeroBattleCharacters) {
                heroBattleCharacter.GetSelectableHero().ToggleSelectable(activated);
            }
        }


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
            int index = 0;
            foreach (HeroBattleCharacter battleCharacter in HeroBattleCharacters) {
                if (battleCharacter.GetSelectableHero() == selectableHero) {
                    Debug.Log($"OnSelectHero index={index}");
                    _battleModel.ExecuteTurn(index);
                }
                index++;
            }
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