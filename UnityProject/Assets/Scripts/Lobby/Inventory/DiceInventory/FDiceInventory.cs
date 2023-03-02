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

    int SelectedDiceID = 0;
    Vector2 InitScrollPos = Vector2.zero;

    Dictionary<int, FAcquiredDiceSlot> AcquiredDiceMap = new Dictionary<int, FAcquiredDiceSlot>();
    Dictionary<int, FNotAcquiredDiceSlot> NotAcquiredDiceMap = new Dictionary<int, FNotAcquiredDiceSlot>();

    public int Critical { set { CriticalText.text = value.ToString() + "%"; } }

    private void Start()
    {
        InitScrollPos = DiceScrollRect.content.anchoredPosition;
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
                FDice acquiredDiceData = diceController.FindAcquiredDice(InData.ID);
                if (acquiredDiceData != null)
                    AddAcquiredDice(acquiredDiceData);
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
        if (!AcquiredDiceMap.ContainsKey(InID))
            return;

        AcquiredDiceMap[InID].CurrentCount = InCount;
    }
    
    public void SetDiceMaxExp(int InID, int InMaxExp)
    {
        if (!AcquiredDiceMap.ContainsKey(InID))
            return;

        AcquiredDiceMap[InID].MaxCount = InMaxExp;
    }

    public void SetDiceLevel(int InID, int InLevel)
    {
        if (!AcquiredDiceMap.ContainsKey(InID))
            return;

        AcquiredDiceMap[InID].Level = InLevel;
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
        DiceScrollRect.content.anchoredPosition = InitScrollPos;
    }

    public void OnClickUpgrade()
    {
        FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
        if (diceController == null)
            return;

        diceController.RequestUpgradeDice(SelectedDiceID);
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
            presetController.SetDicePreset(SelectedDiceID, InIndex);
        }
        SetPresetRegistActive(false);
    }

    void AddAcquiredDice(in FDice InAcquiredDiceData)
    {
        if (AcquiredDiceMap.ContainsKey(InAcquiredDiceData.id))
            return;

        FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InAcquiredDiceData.id);
        if (diceData == null)
            return;

        FAcquiredDiceSlot slot = Instantiate(AcquiredDiceSlotPrefab, AcquiredDiceListUI);
        slot.Init(diceData.Value, InAcquiredDiceData);
        slot.OnClickHandler = OnClickAcquiredDiceSlot;

        AcquiredDiceMap.Add(slot.ID, slot);

        List<int> sortList = AcquiredDiceMap.Keys.ToList();
        sortList.Sort();
        int index = sortList.IndexOf(slot.ID);
        slot.transform.SetSiblingIndex(index);
    }

    void AddNotAcquiredDice(in FDiceData InData)
    {
        if (NotAcquiredDiceMap.ContainsKey(InData.ID))
            return;

        FNotAcquiredDiceSlot slot = Instantiate(NotAcquiredDiceSlotPrefab, NotAcquiredDiceListUI);
        slot.Init(InData);
        slot.OnClickHandler = OnClickNotAcquiredDiceSlot;

        NotAcquiredDiceMap.Add(InData.ID, slot);
    }

    void RemoveNotAcquiredDice(in int InID)
    {
        if (NotAcquiredDiceMap.ContainsKey(InID))
        {
            Destroy(NotAcquiredDiceMap[InID].gameObject);
            NotAcquiredDiceMap.Remove(InID);
        }
    }

    void OnClickAcquiredDiceSlot(int InID)
    {
        if (AcquiredDiceMap.ContainsKey(InID))
        {
            SelectedDiceID = InID;
            FPopupManager.Instance.OpenAcquiredDiceInfoPopup(InID, OnClickUpgrade, OnClickPresetRegist);
        }
    }

    void OnClickNotAcquiredDiceSlot(int InID)
    {
        if (NotAcquiredDiceMap.ContainsKey(InID))
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

            FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(SelectedDiceID);
            if (diceData == null)
                return;

            FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
            if (diceController == null)
                return;

            FDice dice = diceController.FindAcquiredDice(SelectedDiceID);
            if (dice == null)
                return;

            slot.Init(diceData.Value, dice);
        }
        PresetRegistUI.gameObject.SetActive(InActive);
    }

    void ClearInventory()
    {
        foreach (var iter in AcquiredDiceMap)
        {
            Destroy(iter.Value.gameObject);
        }
        AcquiredDiceMap.Clear();

        foreach (var iter in NotAcquiredDiceMap)
        {
            Destroy(iter.Value.gameObject);
        }
        NotAcquiredDiceMap.Clear();
    }
}