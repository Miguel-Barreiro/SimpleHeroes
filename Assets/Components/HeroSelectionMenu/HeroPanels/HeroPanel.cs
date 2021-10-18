using Gram.Core;
using Gram.Model;
using Gram.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gram.HeroSelectionMenu.HeroPanels
{
    public class HeroPanel : MonoBehaviour 
    {
        
        [SerializeField]
        private Image PanelImage;

        [SerializeField]
        private Image FrameImage;

        [SerializeField]
        private SelectableHero SelectableHero;
        
        public SelectableHero GetSelectableHero() { return SelectableHero; }
        
        public void SetHero(Hero hero, CharacterConfiguration characterConfiguration) {
            PanelImage.sprite = characterConfiguration.HeroPortrait;

            SelectableHero.SetHero(hero, characterConfiguration);
        }

        public void SetEmpty() {
            SelectableHero.ResetHero();
            PanelImage.sprite = _emptyImage;
        }

        public void SetSelected(bool selected) {
            FrameImage.color = selected ? Color.white : _notSelectedColor;
        }

        private Color _notSelectedColor;
        private Sprite _emptyImage;
        private void Awake() {
            _notSelectedColor = FrameImage.color;
            _emptyImage = PanelImage.sprite;
        }

    }
}