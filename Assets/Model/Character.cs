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

        public event GameBasics.SimpleDelegate OnDeath;

        public void Damage(int damage) {
            Health -= damage;
            if (Health <= 0) {
                OnDeath?.Invoke();
            }
        }

    }
}