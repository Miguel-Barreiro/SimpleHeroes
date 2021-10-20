using System;
using System.Collections;
using Gram.GameSerialization;
using Gram.Model;
using Gram.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gram.Game
{
    public class GameController : MonoBehaviour
    {
        
        private IGameSerialization _gameSerializationService; 
        private IGameModel _gameModel;

        [SerializeField]
        private string HeroSelectionSceneName;
        [SerializeField]
        private string BattleSceneName;

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
                        LoadScenes(() => { _gameModel.StartGameLoop(); });
                    });
                } else {
                    _gameModel.RestoreGameState(state);
                    LoadScenes(() => { _gameModel.StartGameLoop(); });
                }
            });
        }

        private void LoadScenes(Action doneCallback) {
            StartCoroutine(LoadScenesCoroutine(doneCallback));
        }
        
        private IEnumerator LoadScenesCoroutine(Action doneCallback)
        {

            AsyncOperation asyncLoadHeroSelection = SceneManager.LoadSceneAsync(HeroSelectionSceneName,LoadSceneMode.Additive);
            AsyncOperation asyncLoadBattle = SceneManager.LoadSceneAsync(BattleSceneName,LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoadHeroSelection.isDone || !asyncLoadBattle.isDone)
            {
                yield return null;
            }
            
            doneCallback?.Invoke();
        }
        
    }
}