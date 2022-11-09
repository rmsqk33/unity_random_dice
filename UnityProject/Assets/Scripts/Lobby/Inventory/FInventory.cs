using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FInventory : FLobbyScrollMenuBase
{
    [SerializeField]
    TextMeshProUGUI CriticalText;
    [SerializeField]
    Transform DiceListObject;
    [SerializeField]
    Transform DisableDiceListObject;
    [SerializeField]
    FDiceSlot DiceSlotPrefab;
    [SerializeField]
    FDisableDiceSlot DisableDiceSlotPrefab;
    [SerializeField]
    int DiceXCount;
    [SerializeField]
    int DiceSideSpace;
    [SerializeField]
    int DiceBottomSpace;

    Dictionary<int, FDiceSlot> m_DiceMap = new Dictionary<int, FDiceSlot>();
    Dictionary<int, FDisableDiceSlot> m_DisableDiceList = new Dictionary<int, FDisableDiceSlot>();

#if DEBUG
    private void Start()
    {
        Critical = FUserDataController.Instance.Critical;
        InitDiceSlot();
    }
#endif

    public void InitDiceSlot()
    {
        FDiceDataManager.Instance.ForeachDiceData((in FDiceData InData) =>
        {
            FDice? acquiredDiceData = FUserDataController.Instance.FindAcquiredDice(InData.ID);
            if (acquiredDiceData != null)
                AddDice(InData, acquiredDiceData.Value);
            else
                AddDisableDice(InData);
        });
    }
    
    public int Critical { set { CriticalText.text = value.ToString() + "%"; } }

    public void AcquireDice(in FDiceData InData, in FDice InAcquiredDiceData)
    {
        AddDice(InData, InAcquiredDiceData);
        RemoveDisableDice(InData.ID);
    }

    void AddDice(in FDiceData InData, in FDice InAcquiredDiceData)
    {
        if (m_DiceMap.ContainsKey(InData.ID))
            return;

        FDiceSlot slot = Instantiate(DiceSlotPrefab, DiceListObject);
        slot.Init(InData, InAcquiredDiceData);
        slot.OnClickHandler = OnClickAcquiredDiceSlot;

        m_DiceMap.Add(slot.ID, slot);

        List<int> sortList = m_DiceMap.Keys.ToList();
        int index = sortList.IndexOf(slot.ID);
        slot.transform.SetSiblingIndex(index);
    }

    void AddDisableDice(in FDiceData InData)
    {
        if (m_DisableDiceList.ContainsKey(InData.ID))
            return;

        FDisableDiceSlot slot = Instantiate(DisableDiceSlotPrefab, DisableDiceListObject);
        slot.Init(InData);
        slot.OnClickHandler = OnClickDisableDiceSlot;

        m_DisableDiceList.Add(InData.ID, slot);
    }

    void RemoveDisableDice(in int InID)
    {
        if(m_DisableDiceList.ContainsKey(InID))
        {
            Destroy(m_DisableDiceList[InID]);
            m_DisableDiceList.Remove(InID);
        }
    }

    public void OnClickAcquiredDiceSlot(int InID)
    {
        if (m_DiceMap.ContainsKey(InID))
        {
            FPopupManager.Instance.OpenAcquiredDiceInfoPopup(InID, OnClickUpgrade, OnClickUse);
        }
    }

    public void OnClickDisableDiceSlot(int InID)
    {
        if (m_DisableDiceList.ContainsKey(InID))
        {
            FPopupManager.Instance.OpenDisableDiceInfoPopup(InID);
        }
    }

    public void OnClickUpgrade(int InID)
    {

    }

    public void OnClickUse(int InID)
    {

    }
}