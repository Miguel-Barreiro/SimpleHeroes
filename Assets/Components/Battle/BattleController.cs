using Gram.Model;
using Gram.Utils;
using UnityEngine;

namespace Gram.Battle
{
    public class BattleController : MonoBehaviour
    {
        private IGameModel _gameModel;
        
        private void Start() {
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<IGameModel>();
        }


        public void GoBackMenuSelection() {
            _gameModel.Retreat();
        }
        
    }
}