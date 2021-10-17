using System.Collections.Generic;
using Gram.Core;
using UnityEngine;

namespace Gram.Model
{
    public class GameModel
    {

        public delegate void GameModelDelegate();

        public event GameModelDelegate OnLogicStateChange;
        public event GameModelDelegate OnHeroCollectionChange;
        
        public event GameModelDelegate OnSelectedHeroesChange;
        
        private GameState _currentState;

        private CharacterDatabase _characterDatabase;
        private GameDefinitions _gameDefinitions;
        
        
        public GameModel(GameDefinitions gameDefinitions, CharacterDatabase characterDatabase) {
            _characterDatabase = characterDatabase;
            _gameDefinitions = gameDefinitions;
        }

        //-------------------------------------------------------------------------------

        #region GameState


        public string GetSerializedGameState() {
            return JsonUtility.ToJson(_currentState);
        }
        public void RestoreGameState(string newGameState) {
            GameState gameState = JsonUtility.FromJson<GameState>(newGameState);
            _currentState = gameState;
        }
        public void GenerateInitialGameState() {

            _currentState = new GameState() {
                HeroesCollected = new List<Hero>(),
                BattleCount = 0,
                SelectedHeroes = new List<int>(),
                CurrentLogicState = GameState.GameLogicState.HeroSelection
            };

            GenerateInitialHeroes();
        }
        

        #endregion


        //-------------------------------------------------------------------------------

        #region Hero Collections

        public void TrySelectHero(int heroIndex) {
            if (_currentState.CurrentLogicState == GameState.GameLogicState.HeroSelection) {

                if (_currentState.SelectedHeroes.Contains(heroIndex)) {
                    _currentState.SelectedHeroes.Remove(heroIndex);
                    OnSelectedHeroesChange?.Invoke();
                } else if( _currentState.SelectedHeroes.Count < _gameDefinitions.MaximumHeroesInBattle){
                    _currentState.SelectedHeroes.Add(heroIndex);
                    OnSelectedHeroesChange?.Invoke();
                }
            }
        }

        public List<int> GetSelectedHeroIndexes() {
            return _currentState.SelectedHeroes;
        }

        public List<Hero> GetCollectedHeroes() {
            return _currentState.HeroesCollected;
        }

        
        #endregion




        //-------------------------------------------------------------------------------

        #region GameLogic state

        public GameState.GameLogicState GetCurrentLogicState() {
            return _currentState.CurrentLogicState;
        }
        

        public void StartGameLoop() {
            OnLogicStateChange?.Invoke();
        }

        #endregion




        //-------------------------------------------------------------------------------

        #region Internal utils

        private void GenerateInitialHeroes() {
            
            int numberHeroes = _gameDefinitions.InitialNumberHeroes;
            
            List<CharacterConfiguration> newHeroCharacters = _characterDatabase.GetHeroCharactersData(numberHeroes);
            foreach (CharacterConfiguration newHeroCharacter in newHeroCharacters) {
                _currentState.HeroesCollected.Add(GenerateNewHero(newHeroCharacter));
            }
        }

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

        #endregion


    }
}