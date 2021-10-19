using System;
using System.Collections.Generic;
using Gram.Core;
using UnityEngine;

namespace Gram.Model
{
    public class HeroCollectionModel : IHeroCollectionModel
    {
        
        public event GameBasics.SimpleDelegate OnHeroCollectionChange;
        

        public List<Hero> GetCollectedHeroes() {
            return _state.HeroesCollected;
        }

        public Hero GetHeroById(int id) {
            return _state.HeroesCollected[id];
        }


        private readonly List<Hero> _getHeroesByIdResult = new List<Hero>();
        public List<Hero> GetHeroesById(List<int> stateHeroes) {
            
            _getHeroesByIdResult.Clear();
            foreach (int heroIndex in stateHeroes) {
                _getHeroesByIdResult.Add(GetHeroById(heroIndex));
            }
            return _getHeroesByIdResult;
        }


        public void GenerateInitialState() {
            _state = new State();
            
            int numberHeroes = _gameDefinitions.InitialNumberHeroes;
            
            List<CharacterConfiguration> newHeroCharacters = _characterDatabase.GetMultipleRandomHeroCharactersData(numberHeroes);
            foreach (CharacterConfiguration newHeroCharacter in newHeroCharacters) {
                _state.HeroesCollected.Add(GenerateNewHero(newHeroCharacter));
            }
            
            OnHeroCollectionChange?.Invoke();
        }

        
        
        public HeroCollectionModel(ICharacterDatabase characterDatabase, GameDefinitions gameDefinitions) {
            _characterDatabase = characterDatabase;
            _gameDefinitions = gameDefinitions;
        }

        
        [Serializable]
        private class State
        {
            [SerializeField] 
            public List<Hero> HeroesCollected = new List<Hero>();
        }
        
        private State _state;

        private ICharacterDatabase _characterDatabase;
        private GameDefinitions _gameDefinitions;

        
        private Hero GenerateNewHero(CharacterConfiguration heroCharacter) {
            Hero newHero = new Hero() {
                Experience = 0,
                Level = 0,
                Health = heroCharacter.InitialHealth,
                AttackPower = heroCharacter.InitialAttackPower,
                CharacterDataName = heroCharacter.Id
            };
            return newHero;
        }

      

        public string GetSerializedGameState() {
            return JsonUtility.ToJson(_state);
        }
        public void RestoreGameState(string newGameState) {
            State state = JsonUtility.FromJson<State>(newGameState);
            _state = state;
        }

    }
}