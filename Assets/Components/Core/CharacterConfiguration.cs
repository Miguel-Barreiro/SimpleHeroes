using UnityEngine;
using UnityEngine.Serialization;

namespace Gram.Core
{
    [CreateAssetMenu(fileName = "CharacterConfiguration", menuName = "Character Configuration", order = 0)]
    public class CharacterConfiguration : ScriptableObject
    {
        
        [FormerlySerializedAs("HeroPortrait")] public Sprite Portrait; 
        public GameObject BattlePrefab;
        public Vector3 Scale;

        public string Id;
        public int InitialHealth;
        public int InitialAttackPower;
        
        public string History;
        
    }
}