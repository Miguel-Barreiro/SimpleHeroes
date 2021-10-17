using System;
using System.Collections.Generic;
using Gram.Core;
using Gram.Model;
using Gram.Utils;
using UnityEngine;

namespace Gram.HeroSelectionMenu.HeroPanels
{
    public class HeroPanelsController : MonoBehaviour
    {
        [SerializeField] private List<HeroPanel> HeroPanels;

        private IGameModel _gameModel;
        private ICharacterDatabase _characterDatabase;
        
        
        private void Start() {
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();
            _characterDatabase = BasicDependencyInjector.Instance().GetObjectByType<ICharacterDatabase>();
            _gameModel.OnHeroCollectionChange += UpdateHeroPanels;

            _gameModel.OnLogicStateChange += OnLogicStateChange;
            _gameModel.OnSelectedHeroesChange += UpdateHeroSelection;
            
            int panelIndex = 0;
            foreach (HeroPanel heroPanel in HeroPanels) {
                int heroIndex = panelIndex;
                heroPanel.OnSelected += selected => {
                    if (heroIndex < _gameModel.GetCollectedHeroes().Count) {
                        _gameModel.TrySelectHero(heroIndex);
                    }
                };
                panelIndex++;
            }
            
        }
        
        
        private void OnLogicStateChange() {
            GameState.GameLogicState logicState = _gameModel.GetCurrentLogicState();
            switch (logicState) {
                case GameState.GameLogicState.HeroSelection:
                    UpdateHeroPanels();
                    break;
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


        private void OnValidate() {
            int index = 0;
            
            foreach (HeroPanel currentPanel in HeroPanels) {
                if (currentPanel == null) {
                    Debug.LogWarning($"Null element deleted from Hero Panels at index {index}");
                } else {
                    List<HeroPanel> duplicated = HeroPanels.FindAll(panel => panel == currentPanel);
                    if (duplicated.Count > 1) {
                        Debug.LogError($"Same Hero Panel ({currentPanel.name}) added twice at index {index}");
                    }
                }
                index++;
            }

            HeroPanels.RemoveAll(panel => panel == null);
        }
    }
}