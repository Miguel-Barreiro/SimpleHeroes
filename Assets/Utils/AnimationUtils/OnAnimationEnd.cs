using UnityEngine;

namespace Utils.AnimationUtils
{
    public class OnAnimationEnd : StateMachineBehaviour
    {
        public AnimationType AnimationName;
        
        override public void OnStateExit (Animator animator, 
                                          AnimatorStateInfo stateInfo, 
                                          int layerIndex) {
            EndAnimationController endAnimationController = animator.GetComponent<EndAnimationController>();
            if (endAnimationController != null) {
                endAnimationController.AnimationEnd(AnimationName);
            } else {
                Debug.LogError("expected <EndAnimationController> component in gameobject " + animator.gameObject.name);
            }
        }
    }
}