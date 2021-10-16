using UnityEngine;
using Utils.AnimationUtils;

namespace Battle.Heroes
{
    public class Character : MonoBehaviour
    {

        [SerializeField]
        private HealthBar HealthBar;
        
        [SerializeField]
        private AnimationType AttackAnimationType;


        private EndAnimationController _endAnimationController;
        public void Setup() {
            // _endAnimationController
            _endAnimationController.OnAnimationEnd += OnAnimationEnd;
        }
        

        private void OnAnimationEnd(AnimationType animationtype) {
            if (animationtype == AttackAnimationType) {
                Debug.Log("attack done");
                
            }
        }


        private void Start() {
        }
        private void OnDestroy() {
            _endAnimationController.OnAnimationEnd-=OnAnimationEnd;
        }
    }
}
