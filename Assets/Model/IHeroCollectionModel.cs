using System.Collections.Generic;
using Gram.Core;

namespace Gram.Model
{
    public interface IHeroCollectionModel : ISerializableModel
    {
        event GameBasics.SimpleDelegate OnHeroCollectionChange;
        
        void GenerateInitialState();
        void AddNewRandomHeroes(int numberHeroesToReward);

        Hero GetHeroByNameId(string nameId);
        
        Hero[] GetCollectedHeroes();
        Hero[] GetHeroesByNameId(List<string> heroNameIds);
    }
}