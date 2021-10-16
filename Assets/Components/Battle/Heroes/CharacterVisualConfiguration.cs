using UnityEngine;

namespace Battle.Heroes
{
    [CreateAssetMenu(fileName = "CharacterVisualConfiguration", menuName = "CharacterVisualConfiguration", order = 0)]
    public class CharacterVisualConfiguration : ScriptableObject
    {

        public GameObject VisualPrefab;
        public Vector3 Scale;
        
    }
}