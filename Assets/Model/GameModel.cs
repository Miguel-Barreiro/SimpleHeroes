using System;
using System.Collections.Generic;
using Gram.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gram.Model
{
    [Serializable]
    public class GameModel : IGameModel
    {

        public event GameBasics.SimpleDelegate OnLogicStateChange;
        public event GameBasics.SimpleDelegate OnHeroCollectionChange;
        
        public event GameBasics.SimpleDelegate OnSelectedHeroesChange;
        
        [SerializeField]
        private GameState CurrentState;

        private CharacterDatabase _characterDatabase;
        private GameDefinitions _gameDefinitions;
        
        
        public GameModel(GameDefinitions gameDefinitions, CharacterDatabase characterDatabase) {
            _characterDatabase = characterDatabase;
            _gameDefinitions = gameDefinitions;
        }

        //-------------------------------------------------------------------------------

        #region GameState


        public string GetSerializedGameState() {
            return JsonUtility.ToJson(CurrentState);
        }
        public void RestoreGameState(string newGameState) {
            GameState gameState = JsonUtility.FromJson<GameState>(newGameState);
            CurrentState = gameState;
        }
        public void GenerateInitialGameState() {

            CurrentState = new GameState() {
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
            if (CurrentState.CurrentLogicState == GameState.GameLogicState.HeroSelection) {

                if (CurrentState.SelectedHeroes.Contains(heroIndex)) {
                    CurrentState.SelectedHeroes.Remove(heroIndex);
                    OnSelectedHeroesChange?.Invoke();
                } else if( CurrentState.SelectedHeroes.Count < _gameDefinitions.MaximumHeroesInBattle &&
                           heroIndex < CurrentState.HeroesCollected.Count &&  heroIndex >= 0){
                    CurrentState.SelectedHeroes.Add(heroIndex);
                    OnSelectedHeroesChange?.Invoke();
                }
            }
        }

        public List<int> GetSelectedHeroIndexes() {
            return CurrentState.SelectedHeroes;
        }

        public List<Hero> GetSelectedHeroes() {
            List<Hero> result = new List<Hero>();

            foreach (int selectedHeroIndex in CurrentState.SelectedHeroes) {
                Hero selectedHero = CurrentState.HeroesCollected[selectedHeroIndex];
                result.Add(selectedHero);
            }
            
            return result;
        }

        public List<Hero> GetCollectedHeroes() {
            return CurrentState.HeroesCollected;
        }

        
        #endregion




        //-------------------------------------------------------------------------------

        #region GameLogic state

        public GameState.GameLogicState GetCurrentLogicState() {
            return CurrentState.CurrentLogicState;
        }
        

        public void StartGameLoop() {
            OnLogicStateChange?.Invoke();
        }

        public void GoToBattle() {
            CurrentState.CurrentLogicState = GameState.GameLogicState.Battle;
            OnLogicStateChange?.Invoke();
        }

        public void Retreat() {
            CurrentState.CurrentLogicState = GameState.GameLogicState.HeroSelection;
            OnLogicStateChange?.Invoke();
        }


        #endregion




        //-------------------------------------------------------------------------------

        #region Internal utils

        private void GenerateInitialHeroes() {
            
            int numberHeroes = _gameDefinitions.InitialNumberHeroes;
            
            List<CharacterConfiguration> newHeroCharacters = _characterDatabase.GetHeroCharactersData(numberHeroes);
            foreach (CharacterConfiguration newHeroCharacter in newHeroCharacters) {
                CurrentState.HeroesCollected.Add(GenerateNewHero(newHeroCharacter));
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