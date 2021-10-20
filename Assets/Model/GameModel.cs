using System;
using System.Collections.Generic;
using Gram.Core;
using UnityEngine;
using UnityEngine.Serialization;

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
            
            _battleModel.OnNewTurnExecuted += turn => {
                OnChange?.Invoke();
            };

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
            _heroCollectionModel.RestoreGameState(fullModelState.HeroCollectionModelState);
            _battleModel.RestoreGameState(fullModelState.BattleModelState);
            _state = JsonUtility.FromJson<State>(fullModelState.GameModelState);
        }
        public void GenerateInitialGameState() {

            _state = new State() {
                BattleCount = 0,
                SelectedHeroNameIds = new List<string>(),
                CurrentLogicState = GameLogicState.HeroSelection
            };

            _heroCollectionModel.GenerateInitialState();
            _battleModel.GenerateInitialState();
        }
        

        #endregion


        //-------------------------------------------------------------------------------

        #region Hero Selection

        public void TrySelectHero(string heroNameId) {
            if (_state.CurrentLogicState == GameLogicState.HeroSelection) {

                if (_state.SelectedHeroNameIds.Contains(heroNameId)) {
                    _state.SelectedHeroNameIds.Remove(heroNameId);
                    OnSelectedHeroesChange?.Invoke();
                } else if( _state.SelectedHeroNameIds.Count < _gameDefinitions.MaximumHeroesInBattle &&
                           _heroCollectionModel.GetHeroByNameId(heroNameId) != null ){
                    _state.SelectedHeroNameIds.Add(heroNameId);
                    OnSelectedHeroesChange?.Invoke();
                }
            }
        }

        public List<string> GetSelectedHeroNameIds() {
            return _state.SelectedHeroNameIds;
        }

        public List<Hero> GetSelectedHeroes() {
            List<Hero> result = new List<Hero>();

            foreach (string selectedHeroNameId in _state.SelectedHeroNameIds) {
                Hero selectedHero = _heroCollectionModel.GetHeroByNameId(selectedHeroNameId);
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
            _battleModel.GenerateBattle(_state.SelectedHeroNameIds, enemyForBattle);
            
            _state.CurrentLogicState = GameLogicState.Battle;
            OnChange?.Invoke();
            OnLogicStateChange?.Invoke();
        }


        public void GotoHeroSelection() {
            _state.CurrentLogicState = GameLogicState.HeroSelection;
            _state.SelectedHeroNameIds.Clear();
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
            public List<string> SelectedHeroNameIds;

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
                CharacterNameId = enemyCharacterData.NameId
            };
            return newEnemy;
        }

        
        
        #endregion


    }
}