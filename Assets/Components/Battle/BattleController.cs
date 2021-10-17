using Gram.Model;
using Gram.Utils;
using UnityEngine;

namespace Gram.Battle
{
    public class BattleController : MonoBehaviour
    {
        private GameModel _gameModel;
        
        private void Start() {
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<GameModel>();
        }
    }
}