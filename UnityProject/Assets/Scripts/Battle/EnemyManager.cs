using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomDice;

namespace RandomDice
{
    public struct EnemyInfo
    {
        public int      _HP;
        public Vector3  _Direction;
        public float    _Speed;
    }

    public class EnemyManager : Singleton<EnemyManager>
    {
        // - Member Variable
        // - EnemyList
        public List<Enemy> _PlayerList  = new List<Enemy>();
        public List<Enemy> _TeamList    = new List<Enemy>();
        public List<Enemy> _PartyList   = new List<Enemy>();
        // - Check
        // - First Enemy
        public GameObject _FirstEnemy;
        // - Most HP Enemy
        public GameObject _MostHPEnemy;


        // - Method
        // - base
        private void Awake()
        {
            // - Set List
            ListClaer();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // - SummonEnemy
        public void SummonEnemy()
        {
            // - SetPrefabPath
            string path = "Prefabs/Enemy/Enemy_Normal";
            // - Load Field Prefab as GameObject & Instantiate Prefab
            GameObject enemy = Instantiate(Resources.Load<GameObject>(path), gameObject.transform);
            // - Set localPosition
            enemy.transform.localPosition = transform.localPosition;
            // - Increase DiceCount
        }

        // - Wave Start

        // - ClearList
        private void ListClaer()
        {
            _PlayerList.Clear();
            _TeamList.Clear();
            _PartyList.Clear();
        }
    }
}