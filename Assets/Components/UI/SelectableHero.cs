using System;
using Gram.Core;
using Gram.Model;
using Gram.UI.HeroDetailsPopup;
using Gram.Utils;
using Gram.Utils.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gram.UI
{
    public class SelectableHero : MonoBehaviour
    {
        public event GameBasics.SimpleDelegate OnSelected;
        
        [SerializeField] private HeroDetailPopup HeroDetailPopupPrefab;
        
        [SerializeField]
        private Selectable Selectable;
        
        private Hero _hero;
        private CharacterConfiguration _characterConfiguration;

        private GameDefinitions _gameDefinitions;
        
        public void SetHero(Hero hero, CharacterConfiguration characterConfiguration) {
            _hero = hero;
            _characterConfiguration = characterConfiguration;
        }

        public void ResetHero() {
            _hero = null;
            _characterConfiguration = null;
        }
        
        private void HandleSelected(PointerEventData data) { 
            OnSelected?.Invoke();
        }

        private HeroDetailPopup _heroDetailPopup;
        private void HandleSelectedMore(PointerEventData data) {
            if (_hero != null) {
                
                
                _heroDetailPopup = Instantiate(HeroDetailPopupPrefab);
                int experienceNeeded = _gameDefinitions.GetExperienceNeededToLevel(_hero.Level);
                _heroDetailPopup.ShowHeroDetails(_hero.CharacterDataName, _hero.Level, 
                                                 _hero.AttackPower, _hero.Experience, experienceNeeded);
            }
        }

        private void HandleSelectedMoreEnd(PointerEventData data) {
            if (_heroDetailPopup != null) {
                _heroDetailPopup.Close();
                _heroDetailPopup = null;
            }
        }


        private void OnDestroy() {
            Selectable.OnSelected -= HandleSelected;
            Selectable.OnSelectMore -= HandleSelectedMore;
            Selectable.OnSelectMoreEnd -= HandleSelectedMoreEnd;
        }


        private void Start() {
            
            _gameDefinitions = BasicDependencyInjector.Instance().GetObjectByType<GameDefinitions>();

            Selectable.OnSelected += HandleSelected;
            Selectable.OnSelectMore += HandleSelectedMore;
            Selectable.OnSelectMoreEnd += HandleSelectedMoreEnd;
        }

    }
}