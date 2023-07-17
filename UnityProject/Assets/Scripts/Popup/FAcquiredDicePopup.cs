using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FAcquiredDicePopup : FPopupBase
{
    [SerializeField]
    Transform diceListParent;
    [SerializeField]
    FAcquiredDicePopupSlot dicePrefab;
    [SerializeField]
    float delaySec;

    private List<KeyValuePair<int, int>> diceList;
    
    public void OpenPopup(List<KeyValuePair<int, int>> InDiceList)
    {
        diceList = InDiceList;
        AddDice();
    }

    private void AddDice()
    {
        FAcquiredDicePopupSlot slot = Instantiate(dicePrefab, diceListParent);
        slot.SetSlot(diceList[0].Key, diceList[0].Value);

        diceList.RemoveAt(0);

        if (diceList.Count != 0)
        {
            Invoke("AddDice", delaySec);
        }
    }
}
