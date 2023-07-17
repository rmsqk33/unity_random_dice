using RandomDice;
using static RandomDice.Field;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
// - Member Variable

    // - Field, DiceSpots
    private Field       _field          = null;
    private int[]       _dicespots      = new int[MAX_DICELIST];

// - Method
    // - Base
    // Start is called before the first frame update
    void Start()
    {
        // - Initiation
        for (int i = 0; i < MAX_DICELIST; i++)
        { _dicespots[i] = i; }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // - Origin
    // - Get & Set
    public void SetField( Field field)
    { 
        _field = field; 
    }

    public Transform GetRandomDiceTransform()
    {
        // - Create index & GetRandomIndex
        int index = GetRandomIndex();

        // - CheckUseDiceSpot
        while (IsCheckUseDiceSpot(index))
        {
            index = GetRandomIndex();
        }
        // - UseDiceSpot
        UseDiceSpot(index);
        // - return Transform
        return GetDiceTransform(index);
    }
    // - GetTransform
    private Transform GetDiceTransform(int index)
    {
        return _field._playerList[index];
    }
    private int GetRandomIndex()
    {
        return Random.Range(0, MAX_DICELIST);
    }

    // - CheckIndex
    private bool IsCheckUseDiceSpot(int index)
    {
        if (_dicespots[index] == MAX_DICELIST)
        {
            return true;
        }
        return false;
    }

    // - DiceSpot Use/Recall
    private void UseDiceSpot(int index)
    {
        _dicespots[index] = MAX_DICELIST;
    }
    private void RecallDiceSpot(int index)
    {
        _dicespots[index] = index;
    }

    // - Event
    public void SummonDiceBtn()
    {
        if (DiceManager.Instance._DiceCount < MAX_DICELIST)
        {
            DiceManager.Instance.SummonDice(GetRandomDiceTransform());
        }
    }
}
