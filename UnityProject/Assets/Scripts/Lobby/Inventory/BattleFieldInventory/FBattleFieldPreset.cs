using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FBattleFieldPreset : MonoBehaviour
{
    [SerializeField]
    List<Button> m_TabList;
    [SerializeField]
    Image m_BattleFieldImage;
    [SerializeField]
    TextMeshProUGUI m_BattleFieldName;

    int m_SelectedPresetIndex = 0;

    private void Start()
    {
        SetPreset(FUserDataController.Instance.SelectedPresetIndex);
    }

    public void SetPreset(int InPresetIndex)
    {
        UnselectTab(m_SelectedPresetIndex);
        SelectTab(InPresetIndex);

        int battleFieldID = FUserDataController.Instance.GetBattleFieldPresetID(InPresetIndex);
        SetBattleFieldPreset(battleFieldID);

        m_SelectedPresetIndex = InPresetIndex;
    }

    public void SetBattleFieldPreset(int InID)
    {
        FBattleFieldData? battleFieldData = FBattleFieldDataManager.Instance.FindBattleFieldData(InID);
        if (battleFieldData != null)
        {
            m_BattleFieldName.text = battleFieldData.Value.Name;
            m_BattleFieldImage.sprite = Resources.Load<Sprite>(battleFieldData.Value.SkinImage);
        }
    }

    public void OnClickTab(int InIndex)
    {
        if (m_SelectedPresetIndex == InIndex)
            return;

        FUserDataController.Instance.SetPreset(InIndex);
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
