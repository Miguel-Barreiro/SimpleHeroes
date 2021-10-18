using System;
using UnityEngine;

namespace Gram.Model
{
    [Serializable]
    public class Hero : Character, IAttacks
    {
        [SerializeField]
        public int AttackPower;
        [SerializeField]
        public int Experience;
        [SerializeField]
        public int Level;

        public void Damage(int attackPower) { throw new NotImplementedException(); }

        public int GetAttackDamage() { return AttackPower; }

    }
}