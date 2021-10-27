using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gram.Core
{
    [CreateAssetMenu(fileName = "CharacterDatabase", menuName = "CharacterDatabase", order = 0)]
    public class CharacterDatabase : ScriptableObject, ICharacterDatabase
    {

        [SerializeField]
        private List<CharacterConfiguration> HeroCharacters;

        [SerializeField]
        private List<CharacterConfiguration> EnemyCharacters;

        [SerializeField]
        private List<CharacterConfiguration> AllCharacters;

        
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

        
        private readonly Dictionary<string, CharacterConfiguration> _characterConfigurationsById = new Dictionary<string, CharacterConfiguration>();
        

        public CharacterConfiguration GetCharacterConfigurationById(string id) {
            return _characterConfigurationsById[id];
        }

        public CharacterConfiguration GetRandomEnemyCharacterData() {
            int randomIndex = Random.Range(0, EnemyCharacters.Count);
            return EnemyCharacters[randomIndex];
        }


        private void OnValidate() {

            foreach (CharacterConfiguration enemyCharacter in EnemyCharacters) {
                if (!AllCharacters.Contains(enemyCharacter)) {
                    AllCharacters.Add(enemyCharacter);
                }
            }
            foreach (CharacterConfiguration heroConfiguration in HeroCharacters) {
                if (!AllCharacters.Contains(heroConfiguration)) {
                    AllCharacters.Add(heroConfiguration);
                }
            }

            int index = 0;
            foreach (CharacterConfiguration characterConfiguration in AllCharacters) {
                List<CharacterConfiguration> similarIds = AllCharacters.FindAll(configuration => configuration.NameId.Equals(characterConfiguration.NameId) );
                if (similarIds.Count > 1 ) {
                    Debug.LogError( $"non unique id found for { characterConfiguration.NameId } in {index}" );
                }

                index++;
            }
        }


        //-------------------------------------------------------------------------------

        #region Setup

        public void Awake() { CreateCharacterConfigurationDictionary(); }
        
#if UNITY_EDITOR
        private void OnEnable() {
            // use platform dependent compilation so it only exists in editor, otherwise it'll break the build
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                CreateCharacterConfigurationDictionary();
        }
#endif
        
        private void CreateCharacterConfigurationDictionary() {
            foreach (CharacterConfiguration characterConfiguration in AllCharacters) {
                _characterConfigurationsById.Add(characterConfiguration.NameId, characterConfiguration);
            }
        }
        

        #endregion

    }
}