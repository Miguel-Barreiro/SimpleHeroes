using System.Collections;
using TMPro;
using UnityEngine;

namespace Gram.UI.HeroDetailsPopup
{
    public class HeroDetailPopup : MonoBehaviour
    {
        [SerializeField]
        private RectTransform Frame;

        [SerializeField]
        private TextMeshProUGUI NameValue;
        [SerializeField]
        private TextMeshProUGUI LevelValue;
        [SerializeField]
        private TextMeshProUGUI AttackPowerValue;
        [SerializeField]
        private TextMeshProUGUI ExperienceValue;
        

        public void ShowHeroDetails(string name, int level, int attackPower, int experience, int experienceNeeded) {
            NameValue.text = name;
            LevelValue.text = $"{level}";
            AttackPowerValue.text = $"{attackPower}";
            ExperienceValue.text = $"{experience}/{experienceNeeded}";

        }


        public void Close() {
            Destroy(gameObject);
        }
    }
}