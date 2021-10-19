using System.Collections.Generic;
using Gram.Core;

namespace Gram.Model
{
    public interface IHeroCollectionModel : ISerializableModel
    {
        event GameBasics.SimpleDelegate OnHeroCollectionChange;
        
        void GenerateInitialState();

        Hero GetHeroById(int id);
        
        List<Hero> GetCollectedHeroes();
        List<Hero> GetHeroesById(List<int> heroIndexes);
    }
}