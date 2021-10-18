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
        
        public GameModel DebugameModel;
        
        private void Start() {
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();
            DebugameModel = (GameModel)_gameModel;
            _gameSerializationService = BasicDependencyInjector.Instance().GetObjectByType<IGameSerialization>();

            _gameModel.OnChange+= SaveState;

            StartCoroutine(StartGameCoroutine());
        }

        private void SaveState() {
            _gameSerializationService.SaveGame(_gameModel.GetSerializedGameState(), () => {
                
            });
        }

        private IEnumerator StartGameCoroutine() {
            
            //we need to wait until the unity start phase ends
            
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