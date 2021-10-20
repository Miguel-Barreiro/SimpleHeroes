using UnityEngine;
using UnityEngine.Serialization;

namespace Gram.Core
{
    [CreateAssetMenu(fileName = "CharacterConfiguration", menuName = "Character Configuration", order = 0)]
    public class CharacterConfiguration : ScriptableObject
    {
        
        public Sprite Portrait; 
        public GameObject BattlePrefab;

        public string NameId;
        public int InitialHealth;
        public int InitialAttackPower;
        
        public string History;
        
    }
}