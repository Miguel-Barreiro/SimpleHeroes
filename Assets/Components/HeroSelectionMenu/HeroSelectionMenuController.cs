using System.Collections.Generic;
using Gram.Model;
using Gram.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Gram.HeroSelectionMenu
{
    public class HeroSelectionMenuController : MonoBehaviour
    {

        [SerializeField] private Button BattleButton;
        
        private GameModel _gameModel;
        private GameDefinitions _gameDefinitions;

        private void Start() {
            _gameDefinitions = BasicDependencyInjector.Instance().GetObjectByType<GameDefinitions>();
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<GameModel>();

            _gameModel.OnLogicStateChange += OnLogicStateChange;
            _gameModel.OnSelectedHeroesChange += UpdateHeroSelection;
            
            BattleButton.onClick.AddListener(OnBattlePressed);
        }

        private void OnBattlePressed() {
            _gameModel.GoToBattle();
        }

        private void UpdateHeroSelection() {
            List<int> selectedHeroes = _gameModel.GetSelectedHeroIndexes();
            BattleButton.interactable = selectedHeroes.Count == _gameDefinitions.MaximumHeroesInBattle;
        }

        private void OnLogicStateChange() {
            GameState.GameLogicState logicState = _gameModel.GetCurrentLogicState();
            switch (logicState) {
                case GameState.GameLogicState.HeroSelection:
                    ShowSelectionMenu();
                    UpdateHeroSelection();
                    break;
                case GameState.GameLogicState.Battle:
                    Debug.Log("GO TO BATTLE");
                    HideSelectionMenu();
                    break;
            }
            
        }

        private void HideSelectionMenu() {
            
        }

        private void ShowSelectionMenu() {
        }


    }
}