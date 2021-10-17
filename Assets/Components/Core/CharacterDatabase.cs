using System.Collections.Generic;
using UnityEngine;

namespace Gram.Core
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "CharacterDatabase", order = 0)]
    public class CharacterDatabase : ScriptableObject, ICharacterDatabase
    {

        [SerializeField]
        private List<CharacterConfiguration> HeroCharacters;

        [SerializeField]
        private List<CharacterConfiguration> EnemyCharacters;

        public List<CharacterConfiguration> GetHeroCharactersData(int number, List<CharacterConfiguration> excludingList = null) {
            
            List<CharacterConfiguration> result = new List<CharacterConfiguration>();
            List<CharacterConfiguration> remainingCharacters = new List<CharacterConfiguration>();
            remainingCharacters.AddRange(HeroCharacters);
            if (excludingList != null) {
                remainingCharacters.RemoveAll(configuration => excludingList.Contains(configuration));
            }

            for (int i = 0; i < number; i++) {
                int index = Random.Range(0, remainingCharacters.Count);
                result.Add(remainingCharacters[index]);
                remainingCharacters.RemoveAt(index);
            }

            return result;
        }

        public CharacterConfiguration GetHeroCharacterConfigurationById(string id) {
            return HeroCharacters.Find(configuration => configuration.Id.Equals(id));
        }
        
        public CharacterConfiguration GetEnemyCharacterConfigurationById(string id) {
            return EnemyCharacters.Find(configuration => configuration.Id.Equals(id));
        }
        
        
    }
}