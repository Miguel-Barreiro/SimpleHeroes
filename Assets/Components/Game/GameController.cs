using System.Collections;
using System.Collections.Generic;
using GameSerialization;
using Model;
using UnityEngine;
using Utils;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        
        private IGameSerialization _gameSerializationService; 
        private GameModel _gameModel;
        
        
        private void Start() {
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<GameModel>();
            _gameSerializationService = BasicDependencyInjector.Instance().GetObjectByType<IGameSerialization>();

            _gameModel.OnLogicStateChange += OnLogicStateGameStateChange;

            StartCoroutine(StartGameCoroutine());
        }

        private IEnumerator StartGameCoroutine() {
            
            //we need to wait until the unity start phase ends
            
            yield return null;
            
            _gameSerializationService.LoadGame(state => {
                if (state == null) {
                    _gameModel.GenerateInitialGameState();
                    _gameSerializationService.SaveGame(_gameModel.GetGameState(), () => {
                        _gameModel.Start();
                    });
                } else {
                    _gameModel.SetGameState(state);
                    _gameModel.Start();
                }
            });
        }

        private void OnLogicStateGameStateChange() {
            _gameSerializationService.SaveGame(_gameModel.GetGameState(), () => {
                
            });
        }
    }
}