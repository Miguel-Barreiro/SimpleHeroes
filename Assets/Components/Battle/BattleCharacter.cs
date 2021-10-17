using Battle.Heroes;
using Game;
using Model;
using Unity.Mathematics;
using UnityEngine;
using Utils;
using Utils.AnimationUtils;

namespace Battle
{
    public class BattleCharacter : MonoBehaviour
    {

        [SerializeField]
        private HealthBar HealthBar;
        
        [SerializeField]
        private AnimationType AttackAnimationType;
        

        private EndAnimationController _endAnimationController;
        protected CharacterDatabase CharacterDatabase;
        
        protected void Start() {
            CharacterDatabase = BasicDependencyInjector.Instance().GetObjectByType<CharacterDatabase>();
        }

        public void Setup(Hero hero) {
            CharacterConfiguration characterConfiguration = CharacterDatabase.GetHeroCharacterConfigurationByName(hero.CharacterDataName);
            
            GameObject heroVisual = GameObject.Instantiate(characterConfiguration.BattlePrefab, transform.position, quaternion.identity, transform);
            _endAnimationController = heroVisual.GetComponent<EndAnimationController>(); 
            _endAnimationController.OnAnimationEnd += OnAnimationEnd;
        }
        

        private void OnAnimationEnd(AnimationType animationtype) {
            if (animationtype == AttackAnimationType) {
                Debug.Log("attack done");
                
            }
        }
        
        


        private void OnDestroy() {
            if (_endAnimationController != null) {
                _endAnimationController.OnAnimationEnd-=OnAnimationEnd;
            }
        }
    }
}
