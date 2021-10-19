using System.Collections.Generic;

namespace Gram.Model
{
    public class BattleTurn
    {
        public int HeroIndexAttack;
        public int DamageToEnemy;
        public int NewEnemyHealth;

        public List<EnemyAttack> EnemyAttacks = new List<EnemyAttack>();

        public bool BattleEnd;
        public BattleResult BattleResult;


        public class EnemyAttack
        {
            public int HeroIndex;
            public int Damage;
            public int NewHeroHealth;
        }
        
    }
}