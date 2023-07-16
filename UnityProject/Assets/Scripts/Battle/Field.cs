using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomDice
{

    public class Field : MonoBehaviour
    {
    // - Member Variable
        // - PlayerDiceList
        public Transform[] _playerList = new Transform[MAX_DICELIST];
        // - TeamDiceList
        public Transform[] _teamList = new Transform[MAX_DICELIST];

        // - MaxDiceCount
        public const int MAX_DICELIST = 15;

        // - Fie

        // - Mothod
        /// Awake is called when the script instance is being loaded.
        void Awake()
        {
            
        }
        // Update is called once per frame
        void Update()
        {
            
        }
    }
}