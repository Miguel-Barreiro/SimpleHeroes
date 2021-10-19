using System.Collections.Generic;
using Gram.Core;

namespace Gram.Model
{
    public interface IBattleModel : ISerializableModel
    {
        
        event GameBasics.SingleParameterDelegate<BattleTurn> OnNewTurnExecuted;

        void GenerateBattle(List<int> participatingHeroIds, Enemy enemy);

        void ExecuteTurn(int chosenHeroIndex);
        
        Enemy GetEnemy();

        List<Hero> GetHeroes();
    }
}