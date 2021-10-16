using UnityEngine;

namespace Utils.AnimationUtils
{
    [CreateAssetMenu(fileName = "AnimationType", menuName = "AnimationType", order = 0)]
    public class AnimationType : ScriptableObject
    {
        [SerializeField]
        private string Name;
    }
}