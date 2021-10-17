using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Model;
using UnityEngine;

namespace GameSerialization
{
    public class GameSerializationController : IGameSerialization 
    {
        
        public GameSerializationController() {
        }

        private void OnGameStateChange() {
            
            // we are going to save the game state every time it changes so we can always return to it if the game is closed
            
            
            
        }
        
        public void SaveGame(GameState state, Action doneCallack) {
            BinaryFormatter bf = new BinaryFormatter();
            string filePath = GetSaveFilePath();
            
            // FileStream file = File.Create(filePath);

            string json = JsonUtility.ToJson(state);
            
            // Debug.Log("SAVE gameState " + json);
            //
            // bf.Serialize(file, json);
            // file.Close();
            //
            
            StreamWriter writer = new StreamWriter(filePath, false);
            writer.WriteLine(json);
            writer.Close();
            
            
            
            
            
            
            doneCallack?.Invoke();
        }
        
        public void LoadGame(Action<GameState> doneCallack) {
            string filePath = GetSaveFilePath();
            if (File.Exists(filePath)) {

                
                StreamReader reader = new StreamReader(filePath);
                string json = reader.ReadToEnd();
                //Print the text from the file
                Debug.Log("read "  + json);
                reader.Close();

                
                
                // BinaryFormatter bf = new BinaryFormatter();
                // FileStream file = File.Open(filePath, FileMode.Open);
                // string json = (string)bf.Deserialize(file);
                //
                Debug.Log("LOAD gameState " + json);
                
                GameState gameState = JsonUtility.FromJson<GameState>(json);
                // file.Close();
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