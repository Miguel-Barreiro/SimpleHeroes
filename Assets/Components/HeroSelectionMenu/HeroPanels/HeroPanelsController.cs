using System;
using System.Collections.Generic;
using Gram.Core;
using Gram.Model;
using Gram.UI;
using Gram.Utils;
using UnityEngine;

namespace Gram.HeroSelectionMenu.HeroPanels
{
    public class HeroPanelsController : MonoBehaviour
    {
        [SerializeField] private List<HeroPanel> HeroPanels;

        private IHeroCollectionModel _heroCollectionModel;
        private IGameModel _gameModel;
        private ICharacterDatabase _characterDatabase;
        
        
        
        private void Start() {
            _heroCollectionModel.OnHeroCollectionChange += UpdateHeroPanels;

            _gameModel.OnLogicStateChange += OnLogicStateChange;
            _gameModel.OnSelectedHeroesChange += UpdateHeroSelection;
            
            foreach (HeroPanel heroPanel in HeroPanels) {
                heroPanel.GetSelectableHero().OnSelected += OnSelectHero;
            }
        }

        private void Awake() {
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();
            _heroCollectionModel = BasicDependencyInjector.Instance().GetObjectByType<IHeroCollectionModel>();
            _characterDatabase = BasicDependencyInjector.Instance().GetObjectByType<ICharacterDatabase>();
        }

        private void OnDestroy() {
            foreach (HeroPanel heroPanel in HeroPanels) {
                heroPanel.GetSelectableHero().OnSelected -= OnSelectHero;
            }
        }

        
        private void OnSelectHero(SelectableHero selectable) {
            _gameModel.TrySelectHero(selectable.GetNameId());    
        }

        
        private void OnLogicStateChange() {
            GameLogicState logicState = _gameModel.GetCurrentLogicState();
            switch (logicState) {
                case GameLogicState.HeroSelection:
                    UpdateHeroPanels();
                    break;
            }
        }

        
        private void UpdateHeroSelection() {
            List<string> selectedHeroes = _gameModel.GetSelectedHeroNameIds();
            foreach (HeroPanel heroPanel in HeroPanels) {
                heroPanel.SetSelected(selectedHeroes.Contains(heroPanel.GetSelectableHero().GetNameId()));
            }
        }
        
        private void UpdateHeroPanels() {
            List<Hero> heroesCollected = _heroCollectionModel.GetCollectedHeroes();
            List<string> selectedHeroes = _gameModel.GetSelectedHeroNameIds();
            int i = 0;
            foreach (Hero hero in heroesCollected) {
                HeroPanel heroPanel = HeroPanels[i];
                
                CharacterConfiguration charaterConfig = _characterDatabase.GetCharacterConfigurationById(hero.CharacterNameId);
                heroPanel.SetHero(hero, charaterConfig);
                heroPanel.SetSelected(selectedHeroes.Contains(heroPanel.GetSelectableHero().GetNameId()));

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