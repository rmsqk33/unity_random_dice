using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomDice;

public class GameMgr : Singleton<GameMgr>
{
// - Member Variable
    private GameObject _field;

// - Method
    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Temp()
    {

    }

    public void SetField(FieldNumber fieldnumber)
    {
        switch (fieldnumber)
        {
            case FieldNumber.Normal :
            {
                return;
            }
            default:
            {
                return;
            }
        }
    }
}
