using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gram.Model
{
    [Serializable]
    public class BattleTurn
    {
        [SerializeField]
        public string HeroNameIdAttack;
        [SerializeField]
        public int DamageToEnemy;
        [SerializeField]
        public int NewEnemyHealth;
        [SerializeField]
        public float NewEnemyHealthPercentage;

        [SerializeField]
        public List<EnemyAttack> EnemyAttacks = new List<EnemyAttack>();

        [SerializeField]
        public bool BattleEnd;

        [SerializeField]
        public BattleResult BattleEndResult;
        
        [Serializable]
        public class BattleResult
        {
            [SerializeField]
            public bool PlayerWon;
            [SerializeField]
            public List<HeroResult> HeroResults = new List<BattleTurn.HeroResult>();
        }
        
        [Serializable]
        public class HeroResult
        {
            [SerializeField]
            public Hero Hero;
            [SerializeField]
            public int LevelsGained = 0;
            [SerializeField]
            public int ExperienceGained = 0;
            [SerializeField]
            public int HealthGained = 0;
            [SerializeField]
            public int AttackPowerGained = 0;
        }
        
        
        [Serializable]
        public class EnemyAttack
        {
            [SerializeField]
            public string HeroNameId;
            [SerializeField]
            public int Damage;
            [SerializeField]
            public int NewHeroHealth;
            [SerializeField]
            public float NewHeroHealthPercentage;
        }
        
    }
}