using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gram.Model
{
    
    public class BattleModel : IBattleModel
    {

        [Serializable]
        private class State
        {
            [SerializeField]
            public Enemy Enemy;
            [SerializeField]
            public List<int> Heroes;
        }
        

        public BattleModel(HeroCollectionModel heroCollectionModel) {
            _heroCollectionModel = heroCollectionModel;
        }

        public void GenerateBattle(List<int> participatingHeroes, Enemy enemy) {
            _state = new State() {
                Enemy = enemy,
                Heroes = participatingHeroes
            };
        }

        public string GetSerializedGameState() {
            return JsonUtility.ToJson(_state);
        }
        public void RestoreGameState(string newGameState) {
            State state = JsonUtility.FromJson<State>(newGameState);
            _state = state;
        }

        private HeroCollectionModel _heroCollectionModel;
        private State _state;

    }
}