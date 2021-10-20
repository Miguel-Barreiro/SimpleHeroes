using TMPro;
using UnityEngine;

namespace Gram.UI.Tooltips
{
    public class HeroDetailsTooltip : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI NameValue;
        [SerializeField]
        private TextMeshProUGUI LevelValue;
        [SerializeField]
        private TextMeshProUGUI AttackPowerValue;
        [SerializeField]
        private TextMeshProUGUI ExperienceValue;
        

        public void ShowHeroDetails(string name, int level, int attackPower, int experience, int experienceNeeded) {
            gameObject.SetActive(true);
            NameValue.text = name;
            LevelValue.text = $"{level}";
            AttackPowerValue.text = $"{attackPower}";
            ExperienceValue.text = $"{experience}/{experienceNeeded}";

        }

        public void Hide() {
            gameObject.SetActive(false);
        }

 
    }
}