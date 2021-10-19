using Gram.Model;
using Gram.UI;
using UnityEngine;

namespace Gram.Battle
{
    public class HeroBattleCharacter : BattleCharacter
    {
        
        [SerializeField]
        private SelectableHero SelectableHero;
        
        public void Setup(Hero hero) {
            base.Setup(hero, false);
            SelectableHero.SetHero(hero, CharacterConfiguration);
        }

        public SelectableHero GetSelectableHero() { return SelectableHero; }

    }
}