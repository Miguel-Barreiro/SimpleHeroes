using System;
using UnityEngine;

namespace Gram.Model
{
    [Serializable]
    public class Enemy : Character, IAttacks
    {
        [SerializeField]
        public int AttackPower;

        public int GetAttackDamage() { return AttackPower; }
    }
}