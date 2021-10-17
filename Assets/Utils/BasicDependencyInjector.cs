using System;
using System.Collections.Generic;
using Gram.Core;
using Gram.GameSerialization;
using Gram.Model;
using UnityEngine;
using Object = System.Object;

namespace Gram.Utils
{
    
    //seeing we cannot use an external dependency injector I created a very basic one just as an example 
    // SINGLETON: for time saving as we dont want to create a full library  we are going to use a singleton and have everything here
    public class BasicDependencyInjector : MonoBehaviour
    {

        [SerializeField]
        private CharacterDatabase CharacterDatabase;

        [SerializeField]
        private GameDefinitions GameDefinitions;
        
        
        private void SetupInstances() {
            GameModel gameModel = new GameModel(GameDefinitions, CharacterDatabase);
            GameSerializationController gameSerializationController = new GameSerializationController();
            
            _objectsByType.Add(typeof(GameModel), gameModel);
            _objectsByType.Add(typeof(IGameSerialization), gameSerializationController);
            _objectsByType.Add(typeof(CharacterDatabase), CharacterDatabase);
            _objectsByType.Add(typeof(GameDefinitions), GameDefinitions);
        }

        
        
        
        
        
        private readonly Dictionary<Type, Object> _objectsByType = new Dictionary<Type, Object>();
        
        private void Awake() {
            if (_instance != null && _instance != this) {
                Debug.LogError("BasicDependencyInjector objects found");
            } else {
                _instance = this;
            }
            SetupInstances();
        }



        public T GetObjectByType<T>() where T : class {
            Type type = typeof(T);
            if (_objectsByType.ContainsKey(type)) {
                return _objectsByType[type] as T;
            } else {
                Debug.LogError("injector for type " + type.Name + " wasnt setup");
                return null;
            }
        }
        
        
        private static BasicDependencyInjector _instance;
        public static BasicDependencyInjector Instance() {
            if (_instance == null) {
                Debug.LogError("No BasicDependencyInjector.Instance found");
            }
            return _instance;
        }

    }
}