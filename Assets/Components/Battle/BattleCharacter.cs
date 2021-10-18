using Gram.Core;
using Gram.Model;
using Gram.Utils;
using Gram.Utils.AnimationUtils;
using Unity.Mathematics;
using UnityEngine;

namespace Gram.Battle
{
    public class BattleCharacter : MonoBehaviour
    {

        [SerializeField]
        private HealthBar HealthBar;
        
        [SerializeField]
        private AnimationType AttackAnimationType;
        

        private EndAnimationController _endAnimationController;
        protected ICharacterDatabase CharacterDatabase;
        
        protected void Start() {
            CharacterDatabase = BasicDependencyInjector.Instance().GetObjectByType<ICharacterDatabase>();
        }

        public virtual void Setup(Hero hero) {
            CharacterConfiguration characterConfiguration = CharacterDatabase.GetHeroCharacterConfigurationById(hero.CharacterDataName);
            
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
