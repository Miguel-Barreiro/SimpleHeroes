using UnityEngine;
using UnityEngine.UI;

namespace Gram.Battle
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Image FillImage;

        
        public void SetPercentage(float percentage) {
            FillImage.fillAmount = Mathf.Clamp01(percentage);
        }

    }
}