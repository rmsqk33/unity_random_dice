using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FDiceInventory : FGroupMenuBase
{
    [SerializeField]
    TextMeshProUGUI CriticalText;
    [SerializeField]
    Transform AcquiredDiceListUI;
    [SerializeField]
    Transform NotAcquiredDiceListUI;
    [SerializeField]
    FAcquiredDiceSlot AcquiredDiceSlotPrefab;
    [SerializeField]
    FNotAcquiredDiceSlot NotAcquiredDiceSlotPrefab;
    [SerializeField]
    Transform PresetRegistUI;
    [SerializeField]
    FDicePreset DicePreset;
    [SerializeField]
    ScrollRect DiceScrollRect;

    int m_SelectedDiceID = 0;
    Vector2 m_InitScrollPos = Vector2.zero;

    Dictionary<int, FAcquiredDiceSlot> m_AcquiredDiceMap = new Dictionary<int, FAcquiredDiceSlot>();
    Dictionary<int, FNotAcquiredDiceSlot> m_NotAcquiredDiceMap = new Dictionary<int, FNotAcquiredDiceSlot>();

    public int Critical { set { CriticalText.text = value.ToString() + "%"; } }

    private void Start()
    {
        m_InitScrollPos = DiceScrollRect.content.anchoredPosition;
        InitInventory();
    }

    public void InitInventory()
    {
        FStatController statController = FLocalPlayer.Instance.FindController<FStatController>();
        if(statController != null)
        {
            Critical = statController.Critical;
        }

        FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
        if (diceController != null)
        {
            ClearInventory();
            FDiceDataManager.Instance.ForeachDiceData((in FDiceData InData) =>
            {
                FDice? acquiredDiceData = diceController.FindAcquiredDice(InData.ID);
                if (acquiredDiceData != null)
                    AddAcquiredDice(acquiredDiceData.Value);
                else
                    AddNotAcquiredDice(InData);
            });
        }
    }

    public void AcquireDice(in FDice InAcquiredDiceData)
    {
        AddAcquiredDice(InAcquiredDiceData);
        RemoveNotAcquiredDice(InAcquiredDiceData.id);
    }

    public void SetDiceCount(int InID, int InCount)
    {
        if (!m_AcquiredDiceMap.ContainsKey(InID))
            return;

        m_AcquiredDiceMap[InID].CurrentCount = InCount;
    }
    
    public void SetDiceMaxExp(int InID, int InExp, int InMaxExp)
    {
        if (!m_AcquiredDiceMap.ContainsKey(InID))
            return;

        m_AcquiredDiceMap[InID].SetCount(InExp, InMaxExp);
    }

    public override void OnActive()
    {
        base.OnActive();

        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if (presetController != null)
        {
            DicePreset.SetPreset(presetController.SelectedPresetIndex);
        }
    }

    public override void OnDeactive()
    {
        base.OnDeactive();
        SetPresetRegistActive(false);

        DiceScrollRect.velocity = Vector2.zero;
        DiceScrollRect.content.anchoredPosition = m_InitScrollPos;
    }

    public void OnClickUpgrade()
    {

    }

    public void OnClickPresetRegist()
    {
        SetPresetRegistActive(true);
        FPopupManager.Instance.ClosePopup();
    }

    public void OnClickPresetRegistCancel()
    {
        SetPresetRegistActive(false);
    }

    public void OnChangeDiceInPreset(int InIndex)
    {
        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if(presetController != null)
        {
            presetController.SetDicePreset(m_SelectedDiceID, InIndex);
        }
        SetPresetRegistActive(false);
    }

    void AddAcquiredDice(in FDice InAcquiredDiceData)
    {
        if (m_AcquiredDiceMap.ContainsKey(InAcquiredDiceData.id))
            return;

        FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InAcquiredDiceData.id);
        if (diceData == null)
            return;

        FAcquiredDiceSlot slot = Instantiate(AcquiredDiceSlotPrefab, AcquiredDiceListUI);
        slot.Init(diceData.Value, InAcquiredDiceData);
        slot.OnClickHandler = OnClickAcquiredDiceSlot;

        m_AcquiredDiceMap.Add(slot.ID, slot);

        List<int> sortList = m_AcquiredDiceMap.Keys.ToList();
        int index = sortList.IndexOf(slot.ID);
        slot.transform.SetSiblingIndex(index);
    }

    void AddNotAcquiredDice(in FDiceData InData)
    {
        if (m_NotAcquiredDiceMap.ContainsKey(InData.ID))
            return;

        FNotAcquiredDiceSlot slot = Instantiate(NotAcquiredDiceSlotPrefab, NotAcquiredDiceListUI);
        slot.Init(InData);
        slot.OnClickHandler = OnClickNotAcquiredDiceSlot;

        m_NotAcquiredDiceMap.Add(InData.ID, slot);
    }

    void RemoveNotAcquiredDice(in int InID)
    {
        if (m_NotAcquiredDiceMap.ContainsKey(InID))
        {
            Destroy(m_NotAcquiredDiceMap[InID].gameObject);
            m_NotAcquiredDiceMap.Remove(InID);
        }
    }

    void OnClickAcquiredDiceSlot(int InID)
    {
        if (m_AcquiredDiceMap.ContainsKey(InID))
        {
            m_SelectedDiceID = InID;
            FPopupManager.Instance.OpenAcquiredDiceInfoPopup(InID, OnClickUpgrade, OnClickPresetRegist);
        }
    }

    void OnClickNotAcquiredDiceSlot(int InID)
    {
        if (m_NotAcquiredDiceMap.ContainsKey(InID))
        {
            FPopupManager.Instance.OpenNotAcquiredDiceInfoPopup(InID);
        }
    }

    void SetPresetRegistActive(bool InActive)
    {
        DicePreset.SetPresetRegistActive(InActive);
        DiceScrollRect.gameObject.SetActive(!InActive);

        if (InActive)
        {
            FAcquiredDiceSlot slot = PresetRegistUI.Find("DiceSlot").GetComponent<FAcquiredDiceSlot>();

            FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(m_SelectedDiceID);
            if (diceData == null)
                return;

            FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
            if (diceController == null)
                return;

            FDice? dice = diceController.FindAcquiredDice(m_SelectedDiceID);
            if (dice == null)
                return;

            slot.Init(diceData.Value, dice.Value);
        }
        PresetRegistUI.gameObject.SetActive(InActive);
    }

    void ClearInventory()
    {
        foreach (var iter in m_AcquiredDiceMap)
        {
            Destroy(iter.Value.gameObject);
        }
        m_AcquiredDiceMap.Clear();

        foreach (var iter in m_NotAcquiredDiceMap)
        {
            Destroy(iter.Value.gameObject);
        }
        m_NotAcquiredDiceMap.Clear();
    }
}