using UnityEngine;

namespace Gram.Core
{
    [CreateAssetMenu(fileName = "GameDefinitions", menuName = "GameDefinitions", order = 0)]
    public class GameDefinitions : ScriptableObject
    {
        [Range(1,10)]
        public int InitialNumberHeroes = 3;

        [Range(1,10)]
        public int MaximumHeroesInBattle = 3;

        [Range(3,99)]
        public int MaximumHeroes = 10;

        [Range(1,99)]
        public int ExperienceLevelUp = 5;


        private void OnValidate() {
            InitialNumberHeroes = Mathf.Clamp(InitialNumberHeroes, 1, MaximumHeroes);
            MaximumHeroesInBattle = Mathf.Clamp(MaximumHeroesInBattle, 1, MaximumHeroes);
        }

        public int GetExperienceNeededToLevel(int currentLevel) {
            return ExperienceLevelUp;
        }
        
    }
}