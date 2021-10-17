using System.Collections.Generic;
using Battle.Heroes;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "CharacterDatabase", order = 0)]
    public class CharacterDatabase : ScriptableObject
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

        public CharacterConfiguration GetHeroCharacterConfigurationByName(string name) {
            return HeroCharacters.Find(configuration => configuration.Name.Equals(name));
        }
        
        public CharacterConfiguration GetEnemyCharacterConfigurationByName(string name) {
            return EnemyCharacters.Find(configuration => configuration.Name.Equals(name));
        }
        
        
    }
}