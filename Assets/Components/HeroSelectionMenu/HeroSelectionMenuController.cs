using System.Collections.Generic;
using Gram.Core;
using Gram.Model;
using Gram.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Gram.HeroSelectionMenu
{
    public class HeroSelectionMenuController : MonoBehaviour
    {

        [SerializeField] private GameObject[] MenuItems;
        [SerializeField] private Button BattleButton;
        
        private IGameModel _gameModel;
        private GameDefinitions _gameDefinitions;

        private void Start() {
            _gameDefinitions = BasicDependencyInjector.Instance().GetObjectByType<GameDefinitions>();
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();

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
            GameLogicState logicState = _gameModel.GetCurrentLogicState();
            switch (logicState) {
                case GameLogicState.HeroSelection:
                    ShowSelectionMenu();
                    UpdateHeroSelection();
                    break;
                case GameLogicState.Battle:
                    Debug.Log("GO TO BATTLE");
                    HideSelectionMenu();
                    break;
            }
            
        }

        private void HideSelectionMenu() {
            foreach (GameObject menuItem in MenuItems) {
                menuItem.SetActive(false);
            }
        }

        private void ShowSelectionMenu() {
            foreach (GameObject menuItem in MenuItems) {
                menuItem.SetActive(true);
            }
        }


    }
}