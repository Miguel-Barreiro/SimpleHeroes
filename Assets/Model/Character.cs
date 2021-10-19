using System;
using UnityEngine;

namespace Gram.Model
{
    [Serializable]
    public class Character : IDamageable
    {
        [SerializeField]
        public string CharacterDataName;

        [SerializeField]
        public int Health;

        [SerializeField]
        public int CurrentHealth;

        
        public void Damage(int damage) {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, Health);
        }

        public void Heal(int value) {
            CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, Health);
        }

        public void Heal() {
            CurrentHealth = Health;
        }

        
        public bool IsAlive() {
            return CurrentHealth > 0;
        }

    }
}