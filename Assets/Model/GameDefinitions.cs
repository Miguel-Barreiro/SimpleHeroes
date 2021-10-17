using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "GameDefinitions", menuName = "GameDefinitions", order = 0)]
    public class GameDefinitions : ScriptableObject
    {
        public int InitialNumberHeroes = 3;
        
    }
}