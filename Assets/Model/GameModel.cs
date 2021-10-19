using System;
using System.Collections.Generic;
using Gram.Core;
using UnityEngine;

namespace Gram.Model
{
    public class GameModel : IGameModel
    {

        public event GameBasics.SimpleDelegate OnLogicStateChange;
        public event GameBasics.SimpleDelegate OnSelectedHeroesChange;

        public event GameBasics.SimpleDelegate OnChange;

        public GameModel(HeroCollectionModel heroCollectionModel, IBattleModel battleModel, GameDefinitions gameDefinitions, CharacterDatabase characterDatabase) {
            _characterDatabase = characterDatabase;
            _gameDefinitions = gameDefinitions;
            _battleModel = battleModel;
            _heroCollectionModel = heroCollectionModel;
        }

        //-------------------------------------------------------------------------------

        #region GameState

        private class FullModelState
        {
            [SerializeField]
            public string GameModelState;
            [SerializeField]
            public string HeroCollectionModelState;
            [SerializeField]
            public string BattleModelState;
        }
        private static readonly FullModelState FullModelStateUtility = new FullModelState();

        public string GetSerializedGameState() {
            
            FullModelStateUtility.GameModelState =JsonUtility.ToJson(_state);
            FullModelStateUtility.BattleModelState = _battleModel.GetSerializedGameState();
            FullModelStateUtility.HeroCollectionModelState = _heroCollectionModel.GetSerializedGameState();
            
            return JsonUtility.ToJson(FullModelStateUtility);
        }
        public void RestoreGameState(string newGameState) {
            FullModelState fullModelState = JsonUtility.FromJson<FullModelState>(newGameState);
            _battleModel.RestoreGameState(fullModelState.BattleModelState);
            _heroCollectionModel.RestoreGameState(fullModelState.HeroCollectionModelState);
            _state = JsonUtility.FromJson<State>(fullModelState.GameModelState);
        }
        public void GenerateInitialGameState() {

            _state = new State() {
                BattleCount = 0,
                SelectedHeroes = new List<int>(),
                CurrentLogicState = GameLogicState.HeroSelection
            };

            _heroCollectionModel.GenerateInitialState();
        }
        

        #endregion


        //-------------------------------------------------------------------------------

        #region Hero Selection

        public void TrySelectHero(int heroIndex) {
            if (_state.CurrentLogicState == GameLogicState.HeroSelection) {

                if (_state.SelectedHeroes.Contains(heroIndex)) {
                    _state.SelectedHeroes.Remove(heroIndex);
                    OnSelectedHeroesChange?.Invoke();
                } else if( _state.SelectedHeroes.Count < _gameDefinitions.MaximumHeroesInBattle &&
                           _heroCollectionModel.GetHeroById(heroIndex) != null){
                    _state.SelectedHeroes.Add(heroIndex);
                    OnSelectedHeroesChange?.Invoke();
                }
            }
        }

        public List<int> GetSelectedHeroIndexes() {
            return _state.SelectedHeroes;
        }

        public List<Hero> GetSelectedHeroes() {
            List<Hero> result = new List<Hero>();

            foreach (int selectedHeroIndex in _state.SelectedHeroes) {
                Hero selectedHero = _heroCollectionModel.GetHeroById(selectedHeroIndex);
                result.Add(selectedHero);
            }
            
            return result;
        }

        #endregion




        //-------------------------------------------------------------------------------

        #region GameLogic state

        public GameLogicState GetCurrentLogicState() {
            return _state.CurrentLogicState;
        }
        

        public void StartGameLoop() {
            OnChange?.Invoke();
            OnLogicStateChange?.Invoke();
        }

        public void GoToBattle() {
            Enemy enemyForBattle = GenerateEnemyForBattle(_state.BattleCount);
            _battleModel.GenerateBattle(_state.SelectedHeroes, enemyForBattle);
            
            _state.CurrentLogicState = GameLogicState.Battle;
            OnChange?.Invoke();
            OnLogicStateChange?.Invoke();
        }


        public void Retreat() {
            _state.CurrentLogicState = GameLogicState.HeroSelection;
            _state.SelectedHeroes.Clear();
            OnChange?.Invoke();
            OnLogicStateChange?.Invoke();
        }


        #endregion




        //-------------------------------------------------------------------------------

        #region Internal utils

        [Serializable]
        private class State
        {
            [SerializeField]
            public List<int> SelectedHeroes;

            [SerializeField]
            public int BattleCount;

            [SerializeField]
            public GameLogicState CurrentLogicState;

        }
        private State _state;

        private IHeroCollectionModel _heroCollectionModel;
        private IBattleModel _battleModel;

        private ICharacterDatabase _characterDatabase;
        private GameDefinitions _gameDefinitions;



        private Enemy GenerateEnemyForBattle(int stateBattleCount) {
            CharacterConfiguration enemyCharacterData = _characterDatabase.GetRandomEnemyCharacterData();
            Enemy newEnemy = new Enemy() {
                Health = enemyCharacterData.InitialHealth,
                AttackPower = enemyCharacterData.InitialAttackPower,
                CharacterDataName = enemyCharacterData.Id
            };
            return newEnemy;
        }

        
        
        #endregion


    }
}