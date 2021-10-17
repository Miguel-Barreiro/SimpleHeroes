using Gram.Core;
using Gram.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Gram.HeroSelectionMenu
{
    public class HeroPanel : SelectableHero
    {
        
        [SerializeField]
        private Image PanelImage;

        [SerializeField]
        private Image FrameImage;

        public void SetHero(CharacterConfiguration config) {
            PanelImage.sprite = config.HeroPortrait;
        }

        public void SetEmpty() {
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