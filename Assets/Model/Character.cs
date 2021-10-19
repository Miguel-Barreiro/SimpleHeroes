using System;
using Gram.Core;
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

        public void Damage(int damage) {
            Health = Mathf.Clamp(Health - damage, 0, Int32.MaxValue);
        }

        public bool IsAlive() {
            return Health > 0;
        }

    }
}