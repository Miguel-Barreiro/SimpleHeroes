using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gram.Model
{
    public class BattleTurn
    {
        public string HeroNameIdAttack;
        public int DamageToEnemy;
        public int NewEnemyHealth;
        public float NewEnemyHealthPercentage;

        [SerializeField]
        public List<EnemyAttack> EnemyAttacks = new List<EnemyAttack>();

        public bool BattleEnd;
        public BattleResult BattleResult;

        [Serializable]
        public class EnemyAttack
        {
            public string HeroNameId;
            public int Damage;
            public int NewHeroHealth;
            public float NewHeroHealthPercentage;
        }
        
    }
}