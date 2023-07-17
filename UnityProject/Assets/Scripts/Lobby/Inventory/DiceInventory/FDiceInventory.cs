using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FDiceInventory : FUIBase
{
    [SerializeField]
    TextMeshProUGUI criticalText;
    [SerializeField]
    Transform acquiredDiceListUI;
    [SerializeField]
    Transform notAcquiredDiceListUI;
    [SerializeField]
    FAcquiredDiceSlot acquiredDiceSlotPrefab;
    [SerializeField]
    FNotAcquiredDiceSlot notAcquiredDiceSlotPrefab;
    [SerializeField]
    GameObject presetRegistUI;
    [SerializeField]
    FDiceImage presetRegistUIDiceSlot;
    [SerializeField]
    ScrollRect diceScrollRect;
    [SerializeField]
    List<Button> presetTabList;
    [SerializeField]
    List<FDicePresetSlot> presetSlotList;

    int selectedDiceID = 0;
    int selectedPresetIndex = 0;
    Vector2 initScrollPos = Vector2.zero;

    Dictionary<int, FAcquiredDiceSlot> acquiredDiceMap = new Dictionary<int, FAcquiredDiceSlot>();
    Dictionary<int, FNotAcquiredDiceSlot> notAcquiredDiceMap = new Dictionary<int, FNotAcquiredDiceSlot>();

    public int Critical { set { criticalText.text = value.ToString() + "%"; } }

    private void Start()
    {
        initScrollPos = diceScrollRect.content.anchoredPosition;
        InitInventory();        
    }

    public void InitInventory()
    {
        FLocalPlayerStatController statController = FLocalPlayer.Instance.FindController<FLocalPlayerStatController>();
        if (statController != null)
        {
            Critical = statController.Critical;
        }

        FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
        if (diceController != null)
        {
            ClearInventory();
            FDiceDataManager.Instance.ForeachDiceData((in FDiceData InData) =>
            {
                FDice acquiredDiceData = diceController.FindAcquiredDice(InData.id);
                if (acquiredDiceData != null)
                    AddAcquiredDice(acquiredDiceData);
                else
                    AddNotAcquiredDice(InData);
            });
        }

        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if (presetController != null)
        {
            SetPresetTab(presetController.SelectedPresetIndex);
        }
    }

    public void OnEnable()
    {
        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if (presetController != null)
        {
            SetPresetTab(presetController.SelectedPresetIndex);
        }
    }

    public void OnDeactive()
    {
        ClosePresetRegist();

        diceScrollRect.velocity = Vector2.zero;
        diceScrollRect.content.anchoredPosition = initScrollPos;
    }

    public void OnClickPresetRegistCancel()
    {
        ClosePresetRegist();
    }

    public void OnChangeDiceInPreset(int InIndex)
    {
        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if (presetController != null)
        {
            presetController.SetDicePreset(selectedDiceID, InIndex);
        }
        ClosePresetRegist();
    }

    public void OnClickPresetTab(int InIndex)
    {
        if (selectedPresetIndex == InIndex)
            return;

        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if (presetController != null)
        {
            presetController.SetPreset(InIndex);
        }
    }

    public void AcquireDice(in FDice InAcquiredDiceData)
    {
        AddAcquiredDice(InAcquiredDiceData);
        RemoveNotAcquiredDice(InAcquiredDiceData.id);
    }

    public void SetDiceCount(int InID, int InCount)
    {
        if (!acquiredDiceMap.ContainsKey(InID))
            return;

        acquiredDiceMap[InID].CurrentCount = InCount;
    }
    
    public void SetDiceMaxExp(int InID, int InMaxExp)
    {
        if (!acquiredDiceMap.ContainsKey(InID))
            return;

        acquiredDiceMap[InID].MaxCount = InMaxExp;
    }

    public void SetDiceLevel(int InID, int InLevel)
    {
        if (!acquiredDiceMap.ContainsKey(InID))
            return;

        acquiredDiceMap[InID].Level = InLevel;
    }

    public void OpenPresetRegist(int InDiceID)
    {
        selectedDiceID = InDiceID;
        diceScrollRect.gameObject.SetActive(false);
        foreach (FDicePresetSlot slot in presetSlotList)
        {
            slot.SetPresetRegistActive(true);
        }
        presetRegistUIDiceSlot.SetImage(InDiceID);
        presetRegistUI.gameObject.SetActive(true);
    }

    public void ClosePresetRegist()
    {
        diceScrollRect.gameObject.SetActive(true);
        foreach (FDicePresetSlot slot in presetSlotList)
        {
            slot.SetPresetRegistActive(false);
        }
        presetRegistUI.gameObject.SetActive(false);
    }

    public void SetPresetTab(int InTabIndex)
    {
        UnselectPresetTab(selectedPresetIndex);
        SelectPresetTab(InTabIndex);

        selectedPresetIndex = InTabIndex;

        int i = 0;
        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if (presetController != null)
        {
            presetController.ForeachDicePreset(InTabIndex, (int InID) =>
            {
                presetSlotList[i].SetSlot(InID);
                ++i;
            });
        }
    }

    public void SetDicePreset(int InID, int InIndex)
    {
        if (0 <= InIndex && InIndex < presetSlotList.Count)
        {
            presetSlotList[InIndex].SetSlot(InID);
        }
    }

    void SelectPresetTab(int InIndex)
    {
        if (0 <= InIndex && InIndex < presetTabList.Count)
        {
            presetTabList[InIndex].GetComponent<Animator>().SetTrigger("Selected");
        }
    }

    void UnselectPresetTab(int InIndex)
    {
        if(0 <= InIndex && InIndex < presetTabList.Count)
        {
            presetTabList[InIndex].GetComponent<Animator>().SetTrigger("Normal");
        }
    }

    void AddAcquiredDice(in FDice InAcquiredDiceData)
    {
        if (acquiredDiceMap.ContainsKey(InAcquiredDiceData.id))
            return;

        FDiceData diceData = FDiceDataManager.Instance.FindDiceData(InAcquiredDiceData.id);
        if (diceData == null)
            return;

        FAcquiredDiceSlot slot = Instantiate(acquiredDiceSlotPrefab, acquiredDiceListUI);
        slot.Init(diceData, InAcquiredDiceData);

        acquiredDiceMap.Add(slot.ID, slot);

        List<int> sortList = acquiredDiceMap.Keys.ToList();
        sortList.Sort();
        int index = sortList.IndexOf(slot.ID);
        slot.transform.SetSiblingIndex(index);
    }

    void AddNotAcquiredDice(in FDiceData InData)
    {
        if (notAcquiredDiceMap.ContainsKey(InData.id))
            return;

        FNotAcquiredDiceSlot slot = Instantiate(notAcquiredDiceSlotPrefab, notAcquiredDiceListUI);
        slot.Init(InData);

        notAcquiredDiceMap.Add(InData.id, slot);
    }

    void RemoveNotAcquiredDice(in int InID)
    {
        if (notAcquiredDiceMap.ContainsKey(InID))
        {
            Destroy(notAcquiredDiceMap[InID].gameObject);
            notAcquiredDiceMap.Remove(InID);
        }
    }

    void ClearInventory()
    {
        foreach (var iter in acquiredDiceMap)
        {
            Destroy(iter.Value.gameObject);
        }
        acquiredDiceMap.Clear();

        foreach (var iter in notAcquiredDiceMap)
        {
            Destroy(iter.Value.gameObject);
        }
        notAcquiredDiceMap.Clear();
    }
}