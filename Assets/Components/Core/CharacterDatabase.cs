using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gram.Core
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "CharacterDatabase", order = 0)]
    public class CharacterDatabase : ScriptableObject, ICharacterDatabase
    {
        //TODO: add easy to access collection for access any config by id

        [SerializeField]
        private List<CharacterConfiguration> HeroCharacters;

        [SerializeField]
        private List<CharacterConfiguration> EnemyCharacters;

        public List<CharacterConfiguration> GetMultipleRandomHeroCharactersData(int number, List<CharacterConfiguration> excludingList = null) {
            
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



        public CharacterConfiguration GetRandomEnemyCharacterData() {
            int randomIndex = Random.Range(0, EnemyCharacters.Count);
            return EnemyCharacters[randomIndex];
        }


        private void OnValidate() {
            int index = 0;
            foreach (CharacterConfiguration characterConfiguration in HeroCharacters) {
                List<CharacterConfiguration> similarIds = HeroCharacters.FindAll(configuration => configuration.Id == characterConfiguration.Id);
                if (similarIds.Count > 1) {
                    Debug.LogError( $"non unique id found for { characterConfiguration.Id } in {index}" );
                }

                index++;
            }
        }


    }
}