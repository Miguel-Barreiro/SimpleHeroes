using System.Collections.Generic;
using Gram.Core;

namespace Gram.Model
{
    public interface IHeroCollectionModel : ISerializableModel
    {
        event GameBasics.SimpleDelegate OnHeroCollectionChange;
        
        void GenerateInitialState();

        Hero GetHeroByNameId(string nameId);
        
        List<Hero> GetCollectedHeroes();
        List<Hero> GetHeroesByNameId(List<string> heroNameIds);
    }
}