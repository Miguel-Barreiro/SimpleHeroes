using UnityEngine;
using UnityEngine.UI;

namespace Battle.Heroes
{
    [CreateAssetMenu(fileName = "CharacterConfiguration", menuName = "Character Configuration", order = 0)]
    public class CharacterConfiguration : ScriptableObject
    {
        
        public Sprite HeroPortrait; 
        public GameObject BattlePrefab;
        public Vector3 Scale;

        public string Name;
        public int InitialHealth;
        public int InitialAttackPower;

        public string History;
        
    }
}