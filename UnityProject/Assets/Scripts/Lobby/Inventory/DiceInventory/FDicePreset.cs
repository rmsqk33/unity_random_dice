using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FDicePreset : MonoBehaviour
{
    [SerializeField]
    List<Button> TabList;
    [SerializeField]
    List<FDicePresetSlot> SlotList;

    int SelectedPresetIndex = 0;

    private void Start()
    {
        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if(presetController != null)
        {
            SetPreset(presetController.SelectedPresetIndex);
        }
    }

    public void SetPreset(int InPresetIndex)
    {
        UnselectTab(SelectedPresetIndex);
        SelectTab(InPresetIndex);

        SelectedPresetIndex = InPresetIndex;

        int i = 0;
        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
        if (presetController != null && diceController != null)
        {
            presetController.ForeachDicePreset(InPresetIndex, (int InID) =>
            {
                FDice dice = diceController.FindAcquiredDice(InID);
                if (dice != null)
                    SlotList[i].SetSlot(dice);

                ++i;
            });
        }
    }

    public void SetDicePreset(int InID, int InIndex)
    {
        FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
        if(diceController != null)
        {
            FDice dice = diceController.FindAcquiredDice(InID);
            if (dice != null)
            {
                SlotList[InIndex].SetSlot(dice);
            }
        }
    }

    public void OnClickTab(int InIndex)
    {
        if (SelectedPresetIndex == InIndex)
            return;

        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if(presetController != null)
        {
            presetController.SetPreset(InIndex);
        }
    }

    public void SetPresetRegistActive(bool InActive)
    {
        foreach(FDicePresetSlot slot in SlotList)
        {
            slot.SetPresetRegistActive(InActive);
        }
    }

    void SelectTab(int InIndex)
    {
        TabList[InIndex].GetComponent<Animator>().SetTrigger("Selected");
    }

    void UnselectTab(int InIndex)
    {
        TabList[InIndex].GetComponent<Animator>().SetTrigger("Normal");
    }
}
