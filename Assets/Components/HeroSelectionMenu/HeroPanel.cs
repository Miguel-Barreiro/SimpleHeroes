using System;
using System.Collections;
using Battle.Heroes;
using UnityEngine;
using UnityEngine.UI;

namespace HeroSelectionMenu
{
    public class HeroPanel : MonoBehaviour
    {
        
        [SerializeField]
        private Image PanelImage;
        
        public void SetHero(CharacterConfiguration config) {
            PanelImage.sprite = config.HeroPortrait;
        }

        public void SetEmpty() {
            PanelImage.sprite = _emptyImage;
        }

        public void SetSelected(bool selected) {
            PanelImage.color = selected ? Color.white : _notSelectedColor;
        }

        private Color _notSelectedColor;
        private Sprite _emptyImage;
        private void Awake() {
            _notSelectedColor = PanelImage.color;
            _emptyImage = PanelImage.sprite;
        }

    }
}