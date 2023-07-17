using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomDice;

public class GameManager : Singleton<GameManager>
{
// - Member Variable
    // - ID, Object
    // - Field
    private int _fieldID = 0;
    public GameObject _field;
    // - Player, Team
    private Player  _player;
    private Team    _team;

    // - WaveCount
    private int _wavecount = 1;

    // - Method
    // - Base
    private void Awake()
    {
    // - Setting
        // - UserData
        // - DiceManager
        // - EnemyManager
    }

    void Start()
    {
        // - Get FieldID
        _fieldID = 0;
        SetField(_fieldID);
    }

    void Update()
    {
        
    }

    // - SetField
    public void SetField(int fieldnumber)
    {
        // - SetFieldName
        string FieldName = "Prefabs/Field/Field_"+(fieldnumber).ToString();
        // - Load Field Prefab as GameObject
        GameObject field = Resources.Load<GameObject>(FieldName);
        // - Instantiate Prefab
        _field = Instantiate(field, _field.transform);
        UIManager.Instance.SetField(_field.GetComponent<Field>());
    }

    // - Dice : Summon, Attack, LevelUp, GradeUp, Move(Swap).. etc
    public void SummonDice()
    {
        // - Summon 1Grade Dice to Random Position

    }

    // - Enemy : SetSpawnInfo, TakeAttack, 
}
