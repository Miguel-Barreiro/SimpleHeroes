using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Gram.GameSerialization
{
    public class GameSerializationController : IGameSerialization 
    {
        
        public GameSerializationController() {
        }

        private void OnGameStateChange() {
            
            // we are going to save the game state every time it changes so we can always return to it if the game is closed
            
            
            
        }
        
        public void SaveGame(string state, Action doneCallack) {
            BinaryFormatter bf = new BinaryFormatter();
            string filePath = GetSaveFilePath();
            
            StreamWriter writer = new StreamWriter(filePath, false);
            writer.WriteLine(state);
            writer.Close();
            
            doneCallack?.Invoke();
        }
        
        public void LoadGame(Action<string> doneCallack) {
            string filePath = GetSaveFilePath();
            if (File.Exists(filePath)) {
                StreamReader reader = new StreamReader(filePath);
                string gameState = reader.ReadToEnd();
                reader.Close();
                doneCallack?.Invoke(gameState);
            } else {
                doneCallack?.Invoke(null);
            }
        }
        
        
        private static string GetSaveFilePath() {
#if UNITY_EDITOR
            return Application.dataPath + "/gamesave.json";
#else
            return Application.persistentDataPath + "/gamesave.json";
#endif
        }

    }
}