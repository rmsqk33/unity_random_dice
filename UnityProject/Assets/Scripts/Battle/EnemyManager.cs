using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomDice;

public class EnemyManager : Singleton<EnemyManager>
{
    // - Member Variable
    // - EnemyList
    public List<Enemy> _EnemyList = new List<Enemy>();
    // - Check
    // - First Enemy
    public GameObject _FirstEnemy;
    // - Most HP Enemy
    public GameObject _MostHPEnemy;

// - Method
    // - base
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
}
