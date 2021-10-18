using System.Collections.Generic;
using Gram.Core;

namespace Gram.Model
{
    public interface IGameModel
    {
        
        event GameBasics.SimpleDelegate OnLogicStateChange;
        event GameBasics.SimpleDelegate OnHeroCollectionChange;
        
        event GameBasics.SimpleDelegate OnSelectedHeroesChange;

        //this is to be used whenever there is a significant change that needs saved
        event GameBasics.SimpleDelegate OnChange;
        
        string GetSerializedGameState();
        void RestoreGameState(string newGameState);
        void GenerateInitialGameState();


        void TrySelectHero(int heroIndex);
        List<int> GetSelectedHeroIndexes();
        
        List<Hero> GetSelectedHeroes();
        List<Hero> GetCollectedHeroes();


        GameState.GameLogicState GetCurrentLogicState();
        void StartGameLoop();
        void GoToBattle();
        void Retreat();
    }
}