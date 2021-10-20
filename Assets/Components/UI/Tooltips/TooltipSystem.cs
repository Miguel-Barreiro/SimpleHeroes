using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gram.UI.Tooltips
{
    [RequireComponent(typeof(Canvas))]
    public class TooltipSystem : MonoBehaviour
    {

        [SerializeField]
        private HeroDetailsTooltip HeroDetailsTooltip;
        
        public void ShowHeroDetails(string heroCharacterNameId, int heroLevel, int heroAttackPower, 
                                        int heroExperience, int experienceNeeded, PointerEventData pointerEventData) 
        {
            
            
            HeroDetailsTooltip.ShowHeroDetails(heroCharacterNameId, heroLevel, heroAttackPower, heroExperience, experienceNeeded);
        }

        public void CloseHeroDetailsTooltip() 
        {
            HeroDetailsTooltip.Hide();
        }

        private void Start() {
            HeroDetailsTooltip.Hide();
        }
    }
    
    
}
