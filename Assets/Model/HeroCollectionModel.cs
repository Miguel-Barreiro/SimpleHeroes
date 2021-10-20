using System;
using System.Collections.Generic;
using Gram.Core;
using UnityEngine;

namespace Gram.Model
{
    public class HeroCollectionModel : IHeroCollectionModel
    {
        
        public event GameBasics.SimpleDelegate OnHeroCollectionChange;
        
        public Hero[] GetCollectedHeroes() {
            return _state.HeroesCollected.ToArray();
        }
        public Hero GetHeroByNameId(string nameId) { 
            return _state.HeroesCollected.Find(hero => hero.CharacterNameId.Equals(nameId));
        }


        //to reduce garbage
        private readonly List<Hero> _getHeroesByIdResult = new List<Hero>();
        public Hero[] GetHeroesByNameId(List<string> heroNameIds){
            _getHeroesByIdResult.Clear();
            if (heroNameIds != null) {
                foreach (string heroNameId in heroNameIds) {
                    _getHeroesByIdResult.Add(GetHeroByNameId(heroNameId));
                }
            }
            return _getHeroesByIdResult.ToArray();
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


        public void AddNewRandomHeroes(int numberHeroes) {
            
            List<CharacterConfiguration> excludingList = new List<CharacterConfiguration>();
            foreach (Hero hero in _state.HeroesCollected) {
                CharacterConfiguration characterConfiguration = _characterDatabase.GetCharacterConfigurationById(hero.CharacterNameId);
                excludingList.Add(characterConfiguration);
            }
            List<CharacterConfiguration> newHeroCharacters = _characterDatabase.GetMultipleRandomHeroCharactersData(numberHeroes, excludingList);
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
                CharacterNameId = heroCharacter.NameId
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