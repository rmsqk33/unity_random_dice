using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FAcquiredDicePopup : FPopupBase
{
    [SerializeField]
    Transform DiceListParent;
    [SerializeField]
    FAcquiredDicePopupSlot DicePrefab;
    [SerializeField]
    float DelaySec;

    private List<KeyValuePair<int, int>> DiceList;
    
    public void OpenPopup(List<KeyValuePair<int, int>> InDiceList)
    {
        DiceList = InDiceList;
        AddDice();
    }

    private void AddDice()
    {
        FAcquiredDicePopupSlot slot = Instantiate(DicePrefab, DiceListParent);
        slot.SetSlot(DiceList[0].Key, DiceList[0].Value);

        DiceList.RemoveAt(0);

        if (DiceList.Count != 0)
        {
            Invoke("AddDice", DelaySec);
        }
    }
}
