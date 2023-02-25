using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FBattleFieldPreset : MonoBehaviour
{
    [SerializeField]
    List<Button> TabList;
    [SerializeField]
    Image BattleFieldImage;
    [SerializeField]
    TextMeshProUGUI BattleFieldName;

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

        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if (presetController != null)
        {
            int battleFieldID = presetController.GetBattleFieldPresetID(InPresetIndex);
            SetBattleFieldPreset(battleFieldID);
        }

        SelectedPresetIndex = InPresetIndex;
    }

    public void SetBattleFieldPreset(int InID)
    {
        FBattleFieldData? battleFieldData = FBattleFieldDataManager.Instance.FindBattleFieldData(InID);
        if (battleFieldData != null)
        {
            BattleFieldName.text = battleFieldData.Value.Name;
            BattleFieldImage.sprite = Resources.Load<Sprite>(battleFieldData.Value.SkinImage);
        }
    }

    public void OnClickTab(int InIndex)
    {
        if (SelectedPresetIndex == InIndex)
            return;

        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if (presetController != null)
        {
            presetController.SetPreset(InIndex);
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
