using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomDice;

public class GameMgr : Singleton<GameMgr>
{
// - Member Variable
    // - Field ID, Object
    private int _fieldID = 0;
    public GameObject _field;

// - Method
    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        // - Get FieldID
        _fieldID = 0;
    }
    void Start()
    {
        SetField(FieldNumber.Normal);
    }

    void Update()
    {
        
    }

    public void SetField(FieldNumber fieldnumber)
    {
        // - SetFieldName
        string FieldName = "Prefabs/Field_"+((int)fieldnumber).ToString();
        // - Load Field Prefab as GameObject
        GameObject field = Resources.Load(FieldName) as GameObject;
        // - Instantiate Prefab
        Instantiate(field, _field.transform);
    }
}
