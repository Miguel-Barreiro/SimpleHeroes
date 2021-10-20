using System;
using System.Collections.Generic;
using UnityEngine;

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
        public BattleResult BattleEndResult;
        
        [Serializable]
        public class BattleResult
        {
            public bool PlayerWon;
            [SerializeField]
            public List<HeroResult> HeroResults = new List<BattleTurn.HeroResult>();
        }
        
        [Serializable]
        public class HeroResult
        {
            [SerializeField]
            public Hero Hero;
            
            public bool WasDead;
            public int LevelsGained = 0;
            public int ExperienceGained = 0;
        }
        
        
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