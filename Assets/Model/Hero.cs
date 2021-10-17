using System;
using UnityEngine;

namespace Gram.Model
{
    [Serializable]
    public class Hero
    {

        [SerializeField]
        public string CharacterDataName;
        
        [SerializeField]
        public int Health;
        [SerializeField]
        public int AttackPower;
        [SerializeField]
        public int Experience;
        [SerializeField]
        public int Level;
        
    }
}