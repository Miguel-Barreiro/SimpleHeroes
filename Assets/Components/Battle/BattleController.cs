using Model;
using UnityEngine;
using Utils;

namespace Battle
{
    public class BattleController : MonoBehaviour
    {
        private GameModel _gameModel;
        
        private void Start() {
            _gameModel = BasicDependencyInjector.Instance().GetObjectByType<GameModel>();
        }
    }
}