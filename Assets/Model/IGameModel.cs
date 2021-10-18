using System.Collections.Generic;
using Gram.Core;

namespace Gram.Model
{
    public interface IGameModel : ISerializableModel
    {
        
        event GameBasics.SimpleDelegate OnLogicStateChange;
        
        event GameBasics.SimpleDelegate OnSelectedHeroesChange;

        //this is to be used whenever there is a significant change that needs saved
        event GameBasics.SimpleDelegate OnChange;
        
        void GenerateInitialGameState();


        void TrySelectHero(int heroIndex);
        List<int> GetSelectedHeroIndexes();
        
        List<Hero> GetSelectedHeroes();

        GameLogicState GetCurrentLogicState();
        void StartGameLoop();
        void GoToBattle();
        void Retreat();
    }
    
    public enum GameLogicState
    {
        HeroSelection,
        Battle,
        ShowResult
    }

}