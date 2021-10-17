using UnityEngine;

namespace Gram.Core
{
    [CreateAssetMenu(fileName = "CharacterConfiguration", menuName = "Character Configuration", order = 0)]
    public class CharacterConfiguration : ScriptableObject
    {
        
        public Sprite HeroPortrait; 
        public GameObject BattlePrefab;
        public Vector3 Scale;

        public string Id;
        public int InitialHealth;
        public int InitialAttackPower;

        public string History;
        
    }
}