using Gram.Core;
using Gram.Model;
using Gram.UI;
using UnityEngine;

namespace Gram.Battle
{
    public class HeroBattleCharacter : BattleCharacter
    {
        
        [SerializeField]
        private SelectableHero SelectableHero;
        
        public override void Setup(Hero hero) {
            base.Setup(hero);
            
            CharacterConfiguration characterConfiguration = CharacterDatabase.GetHeroCharacterConfigurationById(hero.CharacterDataName);
            SelectableHero.SetHero(hero, characterConfiguration);
        }

        public SelectableHero GetSelectableHero() { return SelectableHero; }
    }
}