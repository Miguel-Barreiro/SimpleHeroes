using System;
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
            _characterDatabase = BasicDependencyInjector.Instance().GetObjectByType<ICharacterDatabase>();
            
            _battleModel = BasicDependencyInjector.Instance().GetObjectByType<IBattleModel>();
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();
            _gameModel.OnLogicStateChange += OnLogicStateChange;

            foreach (HeroBattleCharacter heroBattleCharacter in HeroBattleCharacters) {
                heroBattleCharacter.GetSelectableHero().OnSelected += OnSelectHero;
            }
            
            _battleModel.OnNewTurnExecuted += BattleModelOnOnNewTurnExecuted;
        }

        private void BattleModelOnOnNewTurnExecuted(BattleTurn turn) {
            Debug.Log("new turn " + JsonUtility.ToJson(turn));

            HeroBattleCharacters[turn.HeroIndexAttack].Attack(() => {
                EnemyBattleCharacters[0].Damage(() => {
                    EnemyBattleCharacters[0].Attack(() => {});            
                });
            });


        }


        private void OnLogicStateChange() {
            GameLogicState logicState = _gameModel.GetCurrentLogicState();
            switch (logicState) {
                case GameLogicState.HeroSelection:
                    HideBattleScreen();
                    break;
                case GameLogicState.Battle:
                    SetupBattleCharacters();
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
            }

            Enemy enemy =_battleModel.GetEnemy();
            CharacterConfiguration enemyConfiguration = _characterDatabase.GetEnemyCharacterConfigurationById(enemy.CharacterDataName);

            EnemyBattleCharacters[0].Setup(enemyConfiguration, true);
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