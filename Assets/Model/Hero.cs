using System;
using Battle.Heroes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Model
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