using System;
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
        
        public void Setup(Hero hero) {
            CharacterConfiguration characterConfiguration = CharacterDatabase.GetHeroCharacterConfigurationById(hero.CharacterDataName);
            SelectableHero.SetHero(hero, characterConfiguration);

            base.Setup(characterConfiguration, false);
        }

        public SelectableHero GetSelectableHero() { return SelectableHero; }

    }
}