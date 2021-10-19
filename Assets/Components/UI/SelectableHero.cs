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
        public event GameBasics.SingleParameterDelegate<SelectableHero> OnSelected;
        
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

        private bool _selectable = true;
        private void HandleSelected(PointerEventData data) {
            if (_selectable) {
                OnSelected?.Invoke(this);
            }
        }

        public void ToggleSelectable(bool isSelectable) {
            _selectable = isSelectable;
        }

        

        private HeroDetailPopup _heroDetailPopup;
        private void HandleSelectedMore(PointerEventData data) {
            if (_hero != null) {
                _heroDetailPopup = Instantiate(HeroDetailPopupPrefab);
                int experienceNeeded = _gameDefinitions.GetExperienceNeededToLevel(_hero.Level);
                _heroDetailPopup.ShowHeroDetails(_hero.CharacterNameId, _hero.Level, 
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
            Selectable.OnSelected += HandleSelected;
            Selectable.OnSelectMore += HandleSelectedMore;
            Selectable.OnSelectMoreEnd += HandleSelectedMoreEnd;
        }

        private void Awake() {
            _gameDefinitions = BasicDependencyInjector.Instance().GetObjectByType<GameDefinitions>();
        }

        public string GetNameId() { return _hero?.CharacterNameId; }
    }
}