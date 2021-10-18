using System.Collections.Generic;
using Gram.Model;
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
        
        
        private void Start() {
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();
            _gameModel.OnLogicStateChange += OnLogicStateChange;

            foreach (HeroBattleCharacter heroBattleCharacter in HeroBattleCharacters) {
                // heroBattleCharacter.GetSelectableHero().OnSelected += OnSelectHero;
            }
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
            List<Hero> selectedHeroIndexes = _gameModel.GetSelectedHeroes();
            for (int i = 0; i < selectedHeroIndexes.Count; i++) {
                Hero hero = selectedHeroIndexes[i];
                HeroBattleCharacters[i].Setup(hero);
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
        
    }
}