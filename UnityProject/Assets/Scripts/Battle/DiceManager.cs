using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : Singleton<DiceManager>
{
// - Member Variable
    // - DiceList
    public List<Dice>   _DiceList= new List<Dice>();
    public int          _DiceCount = 0;



// - Method
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SummonDice(Transform transform)
    {
        // - SetPrefabPath
        string path = "Prefabs/Dice/Dice_Fire";
        // - Load Field Prefab as GameObject & Instantiate Prefab
        GameObject dice = Instantiate(Resources.Load<GameObject>(path), gameObject.transform);
        // - Set localPosition
        dice.transform.localPosition = transform.localPosition;
        // - Increase DiceCount
        _DiceCount++;
    }
}
