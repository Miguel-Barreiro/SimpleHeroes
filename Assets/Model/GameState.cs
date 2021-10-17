using System.Collections.Generic;
using UnityEngine;

namespace Gram.Model
{
    public class GameState
    {
        [SerializeField]
        public List<Hero> HeroesCollected;
        [SerializeField]
        public List<int> SelectedHeroes;

        [SerializeField]
        public int BattleCount;

        [SerializeField]
        public GameLogicState CurrentLogicState;
        
        public enum GameLogicState
        {
            HeroSelection,
            Battle,
            Result
        }

    }
}

// {"SelectedHeroes":[],"BattleCount":0,"CurrentLogicState":0}