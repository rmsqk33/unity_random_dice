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
    
    public void OpenPopup(List<KeyValuePair<int, int>> InDiceList)
    {
        foreach (KeyValuePair<int, int> iter in InDiceList)
        {
            FAcquiredDicePopupSlot slot = Instantiate(DicePrefab, DiceListParent);
            slot.SetSlot(iter.Key, iter.Value);
        }
    }
}
