using System;
using System.Collections.Generic;
using Gram.Core;
using Gram.GameSerialization;
using Gram.Model;
using Gram.UI.Tooltips;
using UnityEngine;

namespace Gram.Utils
{
    public class DependencyBinds : MonoBehaviour
    {
        [SerializeField]
        private CharacterDatabase CharacterDatabase;

        [SerializeField]
        private GameDefinitions GameDefinitions;
        
        [SerializeField]
        private TooltipSystem TooltipSystem;

        public virtual void FillBinds(Dictionary<Type, object> objectsByType) {
            HeroCollectionModel heroCollectionModel = new HeroCollectionModel(CharacterDatabase, GameDefinitions);
            BattleModel battleModel = new BattleModel(heroCollectionModel);
            GameModel gameModel = new GameModel(heroCollectionModel, battleModel,GameDefinitions, CharacterDatabase);
            GameSerializationController gameSerializationController = new GameSerializationController();
            
            
            objectsByType.Add(typeof(TooltipSystem), TooltipSystem);
            objectsByType.Add(typeof(IHeroCollectionModel), heroCollectionModel);
            objectsByType.Add(typeof(IBattleModel), battleModel);
            objectsByType.Add(typeof(IGameModel), gameModel);
            objectsByType.Add(typeof(IGameSerialization), gameSerializationController);
            objectsByType.Add(typeof(ICharacterDatabase), CharacterDatabase);
            objectsByType.Add(typeof(GameDefinitions), GameDefinitions);
        }
    }
}