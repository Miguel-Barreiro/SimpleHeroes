using System;
using System.Collections.Generic;
using Gram.Core;
using Gram.GameSerialization;
using Gram.Model;
using UnityEngine;

namespace Gram.Utils
{
    public class DependencyBinds : MonoBehaviour
    {
        [SerializeField]
        private CharacterDatabase CharacterDatabase;

        [SerializeField]
        private GameDefinitions GameDefinitions;

        public virtual void FillBinds(Dictionary<Type, object> objectsByType) {
            GameModel gameModel = new GameModel(GameDefinitions, CharacterDatabase);
            GameSerializationController gameSerializationController = new GameSerializationController();
            
            objectsByType.Add(typeof(IGameModel), gameModel);
            objectsByType.Add(typeof(IGameSerialization), gameSerializationController);
            objectsByType.Add(typeof(ICharacterDatabase), CharacterDatabase);
            objectsByType.Add(typeof(GameDefinitions), GameDefinitions);
        }
    }
}