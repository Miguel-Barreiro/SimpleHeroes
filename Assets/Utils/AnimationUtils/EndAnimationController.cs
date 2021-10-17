using UnityEngine;

namespace Gram.Utils.AnimationUtils
{
    [RequireComponent(typeof(Animator))]
    public class EndAnimationController : MonoBehaviour
    {

        public delegate void AnimationDelegate(AnimationType animationType);
        public event AnimationDelegate OnAnimationEnd;
        
        internal void AnimationEnd(AnimationType animationType) {
            OnAnimationEnd?.Invoke(animationType);
        }
    }
}