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

        [SerializeField] private Button BattleButton;
        [SerializeField] private List<HeroPanel> HeroPanels;
        
        private GameModel _gameModel;
        private ICharacterDatabase _characterDatabase;
        
        private void Start() {
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<GameModel>();
            _characterDatabase = BasicDependencyInjector.Instance().GetObjectByType<ICharacterDatabase>();
            _gameModel.OnHeroCollectionChange += UpdateHeroPanels;

            _gameModel.OnLogicStateChange += OnLogicStateChange;
            _gameModel.OnSelectedHeroesChange += UpdateHeroSelection;
            
            int panelIndex = 0;
            foreach (HeroPanel heroPanel in HeroPanels) {
                int heroIndex = panelIndex;
                heroPanel.OnSelect += selected => {
                    if (heroIndex < _gameModel.GetCollectedHeroes().Count) {
                        _gameModel.TrySelectHero(heroIndex);
                    }
                };
                panelIndex++;
            }
        }

        private void UpdateHeroSelection() {
            List<int> selectedHeroes = _gameModel.GetSelectedHeroIndexes();
            int i = 0;
            foreach (HeroPanel heroPanel in HeroPanels) {
                heroPanel.SetSelected(selectedHeroes.Contains(i));
                i++;
            }
        }

        private void OnLogicStateChange() {
            GameState.GameLogicState logicState = _gameModel.GetCurrentLogicState();
            switch (logicState) {
                case GameState.GameLogicState.HeroSelection:
                    UpdateHeroPanels();
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

        private void UpdateHeroPanels() {
            List<Hero> heroesCollected = _gameModel.GetCollectedHeroes();
            List<int> selectedHeroes = _gameModel.GetSelectedHeroIndexes();
            int i = 0;
            foreach (Hero hero in heroesCollected) {
                CharacterConfiguration charaterConfig = _characterDatabase.GetHeroCharacterConfigurationById(hero.CharacterDataName);
                HeroPanels[i].SetHero(charaterConfig);
                HeroPanels[i].SetSelected(selectedHeroes.Contains(i));
                i++;
            }

            for (; i < HeroPanels.Count; i++) {
                HeroPanels[i].SetEmpty();
            }
        }
    }
}