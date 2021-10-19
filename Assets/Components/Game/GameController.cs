using System.Collections;
using Gram.GameSerialization;
using Gram.Model;
using Gram.Utils;
using UnityEngine;

namespace Gram.Game
{
    public class GameController : MonoBehaviour
    {
        
        private IGameSerialization _gameSerializationService; 
        private IGameModel _gameModel;

        private void Start() {
            _gameModel.OnChange+= SaveState;

            StartCoroutine(StartGameCoroutine());
        }

        private void Awake() {
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();
            
            _gameSerializationService = BasicDependencyInjector.Instance().GetObjectByType<IGameSerialization>();
        }

        private void SaveState() {
            _gameSerializationService.SaveGame(_gameModel.GetSerializedGameState(), () => {
                
            });
        }

        private IEnumerator StartGameCoroutine() {
            yield return null;
            
            _gameSerializationService.LoadGame(state => {
                if (state == null) {
                    _gameModel.GenerateInitialGameState();
                    _gameSerializationService.SaveGame(_gameModel.GetSerializedGameState(), () => {
                        _gameModel.StartGameLoop();
                    });
                } else {
                    _gameModel.RestoreGameState(state);
                    _gameModel.StartGameLoop();
                }
            });
        }
        
    }
}