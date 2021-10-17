using UnityEngine;

namespace Gram.Model
{
    [CreateAssetMenu(fileName = "GameDefinitions", menuName = "GameDefinitions", order = 0)]
    public class GameDefinitions : ScriptableObject
    {
        public int InitialNumberHeroes = 3;

        public int MaximumHeroesInBattle = 3;
    }
}