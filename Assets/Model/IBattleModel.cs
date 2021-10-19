using System.Collections.Generic;
using Gram.Core;

namespace Gram.Model
{
    public interface IBattleModel : ISerializableModel
    {
        
        event GameBasics.SingleParameterDelegate<BattleTurn> OnNewTurnExecuted;

        void GenerateBattle(List<string> participatingHeroNameIds, Enemy enemy);

        void ExecuteTurn(string chosenHeroNameId);
        
        Enemy GetEnemy();

        List<Hero> GetHeroes();
        
        void GenerateInitialState();
    }
}