using System;
using UnityEngine;

namespace Gram.Model
{
    [Serializable]
    public class Character : IDamageable
    {
        [SerializeField]
        public string CharacterNameId;

        [SerializeField]
        public int Health;

        [SerializeField]
        public int CurrentHealth;


        public int GetCurrentHealth() {
            return CurrentHealth;
        }
        
        public float GetCurrentHealthPercentage() {
            return CurrentHealth / (float)Health;
        }

        
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