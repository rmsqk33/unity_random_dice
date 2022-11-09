using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FDicePreset : MonoBehaviour
{
    [SerializeField]
    List<Button> m_TabList;
    [SerializeField]
    List<FDicePresetSlot> m_SlotList;

    int m_SelectedPresetIndex = 0;

    private void Start()
    {
        SetPresetList(FUserDataController.Instance.SelectedPresetIndex);
    }

    public void SetPresetList(int InPresetIndex)
    {
        UnselectTab(m_SelectedPresetIndex);
        SelectTab(InPresetIndex);

        m_SelectedPresetIndex = InPresetIndex;

        int i = 0;
        FUserDataController.Instance.ForeachDicePreset(InPresetIndex, (int InID) => 
        {
            FDice? dice = FUserDataController.Instance.FindAcquiredDice(InID);
            if (dice != null)
                m_SlotList[i].SetSlot(dice.Value);
           
            ++i;
        });
    }

    public void SetPreset(int InID, int InIndex)
    {
        FDice? dice = FUserDataController.Instance.FindAcquiredDice(InID);
        if (dice != null)
            m_SlotList[InIndex].SetSlot(dice.Value);
    }

    public void OnClickTab(int InIndex)
    {
        FUserDataController.Instance.SetPresetList(InIndex);
    }

    public void SetPresetRegistActive(bool InActive)
    {
        foreach(FDicePresetSlot slot in m_SlotList)
        {
            slot.SetPresetRegistActive(InActive);
        }
    }

    void SelectTab(int InIndex)
    {
        m_TabList[InIndex].GetComponent<Animator>().SetTrigger("Selected");
    }

    void UnselectTab(int InIndex)
    {
        m_TabList[InIndex].GetComponent<Animator>().SetTrigger("Normal");
    }
}
