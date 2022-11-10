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
    TextMeshProUGUI m_CriticalText;
    [SerializeField]
    Transform m_AcquiredDiceListUI;
    [SerializeField]
    Transform m_NotAcquiredDiceListUI;
    [SerializeField]
    FAcquiredDiceSlot m_AcquiredDiceSlotPrefab;
    [SerializeField]
    FNotAcquiredDiceSlot m_NotAcquiredDiceSlotPrefab;
    [SerializeField]
    Transform m_PresetRegistUI;
    [SerializeField]
    FDicePreset m_DicePreset;
    [SerializeField]
    ScrollRect m_DiceScrollRect;

    int m_SelectedDiceID = 0;
    Vector2 m_InitScrollPos = Vector2.zero;

    Dictionary<int, FAcquiredDiceSlot> m_AcquiredDiceMap = new Dictionary<int, FAcquiredDiceSlot>();
    Dictionary<int, FNotAcquiredDiceSlot> m_NotAcquiredDiceList = new Dictionary<int, FNotAcquiredDiceSlot>();

    private void Start()
    {
        m_InitScrollPos = m_DiceScrollRect.content.anchoredPosition;
        Critical = FUserDataController.Instance.Critical;
        InitDiceSlot();
    }

    public void On_S_USER_DATA()
    {
        Critical = FUserDataController.Instance.Critical;
        InitDiceSlot();
    }

    public override void OnActive()
    {
        base.OnActive();
        m_DicePreset.SetPreset(FUserDataController.Instance.SelectedPresetIndex);
    }

    public override void OnDeactive()
    {
        base.OnDeactive();
        SetPresetRegistActive(false);

        m_DiceScrollRect.velocity = Vector2.zero;
        m_DiceScrollRect.content.anchoredPosition = m_InitScrollPos;
    }

    public void InitDiceSlot()
    {
        FDiceDataManager.Instance.ForeachDiceData((in FDiceData InData) =>
        {
            FDice? acquiredDiceData = FUserDataController.Instance.FindAcquiredDice(InData.ID);
            if (acquiredDiceData != null)
                AddAcquiredDice(InData, acquiredDiceData.Value);
            else
                AddNotAcquiredDice(InData);
        });
    }

    public int Critical { set { m_CriticalText.text = value.ToString() + "%"; } }

    public void AcquireDice(in FDiceData InData, in FDice InAcquiredDiceData)
    {
        AddAcquiredDice(InData, InAcquiredDiceData);
        RemoveNotAcquiredDice(InData.ID);
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
        FUserDataController.Instance.SetDicePreset(m_SelectedDiceID, InIndex);
        SetPresetRegistActive(false);
    }

    void AddAcquiredDice(in FDiceData InData, in FDice InAcquiredDiceData)
    {
        if (m_AcquiredDiceMap.ContainsKey(InData.ID))
            return;

        FAcquiredDiceSlot slot = Instantiate(m_AcquiredDiceSlotPrefab, m_AcquiredDiceListUI);
        slot.Init(InData, InAcquiredDiceData);
        slot.OnClickHandler = OnClickAcquiredDiceSlot;

        m_AcquiredDiceMap.Add(slot.ID, slot);

        List<int> sortList = m_AcquiredDiceMap.Keys.ToList();
        int index = sortList.IndexOf(slot.ID);
        slot.transform.SetSiblingIndex(index);
    }

    void AddNotAcquiredDice(in FDiceData InData)
    {
        if (m_NotAcquiredDiceList.ContainsKey(InData.ID))
            return;

        FNotAcquiredDiceSlot slot = Instantiate(m_NotAcquiredDiceSlotPrefab, m_NotAcquiredDiceListUI);
        slot.Init(InData);
        slot.OnClickHandler = OnClickNotAcquiredDiceSlot;

        m_NotAcquiredDiceList.Add(InData.ID, slot);
    }

    void RemoveNotAcquiredDice(in int InID)
    {
        if (m_NotAcquiredDiceList.ContainsKey(InID))
        {
            Destroy(m_NotAcquiredDiceList[InID]);
            m_NotAcquiredDiceList.Remove(InID);
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
        if (m_NotAcquiredDiceList.ContainsKey(InID))
        {
            FPopupManager.Instance.OpenNotAcquiredDiceInfoPopup(InID);
        }
    }

    void SetPresetRegistActive(bool InActive)
    {
        m_DicePreset.SetPresetRegistActive(InActive);
        m_DiceScrollRect.gameObject.SetActive(!InActive);

        if (InActive)
        {
            FAcquiredDiceSlot slot = m_PresetRegistUI.Find("DiceSlot").GetComponent<FAcquiredDiceSlot>();

            FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(m_SelectedDiceID);
            FDice? dice = FUserDataController.Instance.FindAcquiredDice(m_SelectedDiceID);
            if (diceData != null && dice != null)
            {
                slot.Init(diceData.Value, dice.Value);
            }
        }
        m_PresetRegistUI.gameObject.SetActive(InActive);
    }
}