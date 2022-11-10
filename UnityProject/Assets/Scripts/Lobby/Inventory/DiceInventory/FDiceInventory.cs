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
    Transform m_DiceListUI;
    [SerializeField]
    Transform m_DisableDiceListUI;
    [SerializeField]
    FDiceSlot m_DiceSlotPrefab;
    [SerializeField]
    FDisableDiceSlot m_DisableDiceSlotPrefab;
    [SerializeField]
    Transform m_PresetRegistUI;
    [SerializeField]
    GameObject m_DiceScrollView;

    int m_SelectedDiceID = 0;
    Vector2 m_InitScrollPos = Vector2.zero;

    Dictionary<int, FDiceSlot> m_DiceMap = new Dictionary<int, FDiceSlot>();
    Dictionary<int, FDisableDiceSlot> m_DisableDiceList = new Dictionary<int, FDisableDiceSlot>();

    private void Start()
    {
        m_InitScrollPos = m_DiceScrollView.GetComponent<ScrollRect>().content.anchoredPosition;
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
    }

    public override void OnDeactive()
    {
        base.OnDeactive();
        SetPresetRegistActive(false);

        ScrollRect scrollRect = m_DiceScrollView.GetComponent<ScrollRect>();
        scrollRect.velocity = Vector2.zero;
        scrollRect.content.anchoredPosition = m_InitScrollPos;
    }

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

    public int Critical { set { m_CriticalText.text = value.ToString() + "%"; } }

    public void AcquireDice(in FDiceData InData, in FDice InAcquiredDiceData)
    {
        AddDice(InData, InAcquiredDiceData);
        RemoveDisableDice(InData.ID);
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
        FUserDataController.Instance.SetPreset(m_SelectedDiceID, InIndex);
        SetPresetRegistActive(false);
    }

    void AddDice(in FDiceData InData, in FDice InAcquiredDiceData)
    {
        if (m_DiceMap.ContainsKey(InData.ID))
            return;

        FDiceSlot slot = Instantiate(m_DiceSlotPrefab, m_DiceListUI);
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

        FDisableDiceSlot slot = Instantiate(m_DisableDiceSlotPrefab, m_DisableDiceListUI);
        slot.Init(InData);
        slot.OnClickHandler = OnClickDisableDiceSlot;

        m_DisableDiceList.Add(InData.ID, slot);
    }

    void RemoveDisableDice(in int InID)
    {
        if (m_DisableDiceList.ContainsKey(InID))
        {
            Destroy(m_DisableDiceList[InID]);
            m_DisableDiceList.Remove(InID);
        }
    }

    void OnClickAcquiredDiceSlot(int InID)
    {
        if (m_DiceMap.ContainsKey(InID))
        {
            m_SelectedDiceID = InID;
            FPopupManager.Instance.OpenAcquiredDiceInfoPopup(InID, OnClickUpgrade, OnClickPresetRegist);
        }
    }

    void OnClickDisableDiceSlot(int InID)
    {
        if (m_DisableDiceList.ContainsKey(InID))
        {
            FPopupManager.Instance.OpenDisableDiceInfoPopup(InID);
        }
    }

    void SetPresetRegistActive(bool InActive)
    {
        FDicePreset presetUI = FindDicePreset();
        if (presetUI != null)
            presetUI.SetPresetRegistActive(InActive);

        m_DiceScrollView.SetActive(!InActive);

        if (InActive)
        {
            FDiceSlot slot = m_PresetRegistUI.Find("DiceSlot").GetComponent<FDiceSlot>();

            FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(m_SelectedDiceID);
            FDice? dice = FUserDataController.Instance.FindAcquiredDice(m_SelectedDiceID);
            if (diceData != null && dice != null)
            {
                slot.Init(diceData.Value, dice.Value);
            }
        }
        m_PresetRegistUI.gameObject.SetActive(InActive);
    }

    FDicePreset FindDicePreset()
    {
        GameObject gameObject = GameObject.Find("DicePreset");
        if (gameObject != null)
            return gameObject.GetComponent<FDicePreset>();

        return null;
    }

    private void OnEnable()
    {
        Debug.Log(1111);
    }
}