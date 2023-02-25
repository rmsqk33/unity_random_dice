using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FBattleFieldInventory : FGroupMenuBase
{
    [SerializeField]
    private FBattleFieldPreset m_BattleFieldPreset;
    [SerializeField]
    ScrollRect m_BattleFieldScrollRect;
    [SerializeField]
    Transform m_AcquiredBattleFieldListUI;
    [SerializeField]
    Transform m_NotAcquiredBattleFieldListUI;
    [SerializeField]
    FBattleFieldSlot m_BattleFieldPrefab;

    int m_SelectedBattleFieldID;
    Vector2 m_InitScrollPos;

    Dictionary<int, FBattleFieldSlot> m_AcquiredBattleFieldMap = new Dictionary<int, FBattleFieldSlot>();
    Dictionary<int, FBattleFieldSlot> m_NotAcquiredBattleFieldMap = new Dictionary<int, FBattleFieldSlot>();

    private void Start()
    {
        m_InitScrollPos = m_BattleFieldScrollRect.content.anchoredPosition;
        InitBattleFieldList();
    }

    public override void OnActive()
    {
        base.OnActive();

        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if(presetController != null)
        {
            m_BattleFieldPreset.SetPreset(presetController.SelectedPresetIndex);
        }
    }

    public override void OnDeactive()
    {
        base.OnDeactive();

        m_BattleFieldScrollRect.velocity = Vector2.zero;
        m_BattleFieldScrollRect.content.anchoredPosition = m_InitScrollPos;
    }

    public void OnClickAcquiredBattleFieldSlot(int InID)
    {
        if(m_AcquiredBattleFieldMap.ContainsKey(InID))
        {
            m_SelectedBattleFieldID = InID;
            FPopupManager.Instance.OpenAcquiredBattleFieldInfoPopup(InID, OnUpgradeBattleField, OnClickUseBattleField);
        }
    }

    public void OnClickNotAcquiredBattleFieldSlot(int InID)
    {
        //if(m_NotAcquiredBattleFieldMap.ContainsKey(InID))
        //{
        //    m_SelectedBattleFieldID = InID;
        //    FPopupManager.Instance.OpenNotAcquiredBattleFieldInfoPopup(InID, OnPurchaseBattleField);
        //}
    }

    public void OnClickUseBattleField()
    {
        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if(presetController != null)
        {
            presetController.SetBattleFieldPreset(m_SelectedBattleFieldID);
        }

        FPopupManager.Instance.ClosePopup();
    }

    public void OnUpgradeBattleField()
    {

    }

    public void OnPurchaseBattleField()
    {

    }

    public void InitBattleFieldList()
    {
        FBattlefieldController battlefieldController = FLocalPlayer.Instance.FindController<FBattlefieldController>();
        if (battlefieldController != null)
        {
            FBattleFieldDataManager.Instance.ForeachBattleFieldData((FBattleFieldData InData) =>
            {
                if (battlefieldController.IsAcquiredBattleField(InData.ID))
                    AddAcquiredBattleField(InData);
                else
                    AddNotAcquiredBattleField(InData);
            });
        }
    }

    private void AddAcquiredBattleField(FBattleFieldData InData)
    {
        if (m_AcquiredBattleFieldMap.ContainsKey(InData.ID))
            return;

        FBattleFieldSlot slot = Instantiate(m_BattleFieldPrefab, m_AcquiredBattleFieldListUI);
        slot.Init(InData);
        slot.GetComponent<Button>().onClick.AddListener(() => { OnClickAcquiredBattleFieldSlot(InData.ID); });

        m_AcquiredBattleFieldMap.Add(InData.ID, slot);

        List<int> sortList = m_AcquiredBattleFieldMap.Keys.ToList();
        int index = sortList.IndexOf(InData.ID);
        slot.transform.SetSiblingIndex(index);
    }

    private void AddNotAcquiredBattleField(FBattleFieldData InData)
    {
        if (m_NotAcquiredBattleFieldMap.ContainsKey(InData.ID))
            return;

        FBattleFieldSlot slot = Instantiate(m_BattleFieldPrefab, m_NotAcquiredBattleFieldListUI);
        slot.Init(InData);
        slot.GetComponent<Button>().onClick.AddListener(() => { OnClickNotAcquiredBattleFieldSlot(InData.ID); });

        m_NotAcquiredBattleFieldMap.Add(InData.ID, slot);
    }

    private void RemoveNotAcquiredBattleField(in int InID)
    {
        if (m_NotAcquiredBattleFieldMap.ContainsKey(InID))
        {
            Destroy(m_NotAcquiredBattleFieldMap[InID]);
            m_NotAcquiredBattleFieldMap.Remove(InID);
        }
    }
}
