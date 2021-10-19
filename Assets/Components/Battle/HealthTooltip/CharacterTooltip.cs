using System;
using TMPro;
using UnityEngine;

namespace Gram.Battle
{
    public class CharacterTooltip : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI DamageTooltip;
        [SerializeField]
        private TextMeshProUGUI AttackTooltip;
        [SerializeField]
        private TextMeshProUGUI HealTooltip;


        [Range(0.5f,100)]
        private float AnimationDeltaY = 10.5f;

        [Range(0.1f,5)]
        private float AnimationTotalTime = 1.5f;

        private Vector3 _initialDamagePosition;
        private Vector3 _initialAttackPosition;
        private Vector3 _initialHealPosition;
        private void Awake() {
            _initialAttackPosition = AttackTooltip.rectTransform.localPosition;
            _initialHealPosition = HealTooltip.rectTransform.localPosition;
            _initialDamagePosition = DamageTooltip.rectTransform.localPosition;

            DamageTooltip.enabled = false;
            HealTooltip.enabled = false;
            AttackTooltip.enabled = false;
        }

        public void TriggerDamage(int damage) {
            DamageTooltip.text = $"-{damage}";
            TriggerTooltip(DamageTooltip, _initialDamagePosition);
        }


        public void TriggerHeal(int healValue) {
            HealTooltip.text = $"+{healValue}";
            TriggerTooltip(HealTooltip, _initialHealPosition);
        }

        public void TriggerAttack(int attack) {
            AttackTooltip.text = $"{attack}";
            TriggerTooltip(AttackTooltip, _initialAttackPosition);
        }
        
        private void TriggerTooltip(TextMeshProUGUI tooltip, Vector3 initialLocalPosition) {
            tooltip.enabled = true;
            tooltip.rectTransform.localPosition = initialLocalPosition;
            LeanTween.moveLocalY(tooltip.gameObject, AnimationDeltaY , AnimationTotalTime).setOnComplete(() => { tooltip.enabled = false; });
        }
        
    }
}