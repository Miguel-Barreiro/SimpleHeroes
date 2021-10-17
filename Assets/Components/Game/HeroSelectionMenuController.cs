using System.Collections.Generic;
using Battle.Heroes;
using HeroSelectionMenu;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game
{
    public class HeroSelectionMenuController : MonoBehaviour
    {

        [SerializeField] private Button BattleButton;
        [SerializeField] private List<HeroPanel> HeroPanels;
        
        private GameModel _gameModel;
        private CharacterDatabase _characterDatabase;
        
        private void Start() {
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<GameModel>();
            _characterDatabase = BasicDependencyInjector.Instance().GetObjectByType<CharacterDatabase>();
            _gameModel.OnHeroCollectionChange += SetupHeroPanels;

            _gameModel.OnLogicStateChange +=OnLogicStateChange;
            _gameModel.OnSelectedHeroesChange += UpdateHeroSelection;
        }

        private void UpdateHeroSelection() {
            GameState gameState = _gameModel.GetGameState();
            int i = 0;
            foreach (HeroPanel heroPanel in HeroPanels) {
                heroPanel.SetSelected(gameState.SelectedHeroes.Contains(i));
                i++;
            }
        }

        private void OnLogicStateChange() {

            switch (_gameModel.GetGameState().CurrentLogicState) {
                case GameState.GameLogicState.HeroSelection:
                    SetupHeroPanels();
                    ShowSelectionMenu();
                    break;
                case GameState.GameLogicState.Battle:
                    HideSelectionMenu();
                    break;
            }
            
        }

        private void HideSelectionMenu() {
            
        }

        private void ShowSelectionMenu() {
            
        }

        private void SetupHeroPanels() {
            GameState gameState = _gameModel.GetGameState();
            List<Hero> heroesCollected = gameState.HeroesCollected;
            int i = 0;
            foreach (Hero hero in heroesCollected) {
                CharacterConfiguration charaterConfig = _characterDatabase.GetHeroCharacterConfigurationByName(hero.CharacterDataName);
                HeroPanels[i].SetHero(charaterConfig);
                HeroPanels[i].SetSelected(gameState.SelectedHeroes.Contains(i));
                i++;
            }

            for (; i < HeroPanels.Count; i++) {
                HeroPanels[i].SetEmpty();
            }
        }
    }
}