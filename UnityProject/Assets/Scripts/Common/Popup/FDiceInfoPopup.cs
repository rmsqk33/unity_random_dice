using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static FPopupManager;

public class FDiceInfoPopup : FPopupBase
{
    [SerializeField]
    FDiceSlot m_DiceSlot;
    [SerializeField]
    FDisableDiceSlot m_DisableDiceSlot;
    [SerializeField]
    TextMeshProUGUI m_NameText;
    [SerializeField]
    TextMeshProUGUI m_Grade;
    [SerializeField]
    TextMeshProUGUI m_Description;
    [SerializeField]
    List<FDiceStatInfo> m_StatInfoList;
    [SerializeField]
    TextMeshProUGUI m_Critical;
    [SerializeField]
    TextMeshProUGUI m_UpgradeCritical;
    [SerializeField]
    TextMeshProUGUI m_UpgradeCost;
    [SerializeField]
    Button m_UpgradeBtn;
    [SerializeField]
    Button m_UseBtn;

    public delegate void ButtonHandler();
    ButtonHandler m_UpgradeBtnHandler;
    ButtonHandler m_UseBtnHandler;

    public ButtonHandler UpgradeHandler { set { m_UpgradeBtnHandler = value; } }
    public ButtonHandler UseHandler { set { m_UseBtnHandler = value; } }

    public void OpenAcquiredDiceInfo(int InID)
    {
        FDice? dice = FUserDataController.Instance.FindAcquiredDice(InID);
        if (dice == null)
            return;

        FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InID);
        if (diceData == null)
            return;

        m_UpgradeBtn.gameObject.SetActive(true);
        m_UseBtn.gameObject.SetActive(true);
        m_DisableDiceSlot.gameObject.SetActive(false);
        m_DiceSlot.gameObject.SetActive(true);
        m_DiceSlot.Init(diceData.Value, dice.Value);
        
        FDiceGradeData? gradeData = FDiceDataManager.Instance.FindGradeData(diceData.Value.Grade);
        if (gradeData != null)
        {
            if(gradeData.Value.LevelDataMap.ContainsKey(dice.Value.level))
            {
                FDiceLevelData levelData = gradeData.Value.LevelDataMap[dice.Value.level];
                m_UpgradeCost.text = levelData.GoldCost.ToString();
                SetUpgradable(levelData.MaxExp <= dice.Value.exp);
                SetCommonDiceInfo(diceData.Value, gradeData.Value);
            }
        }
    }

    public void OpenDisableDiceInfo(int InID)
    {
        FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InID);
        if (diceData == null)
            return;

        m_UpgradeBtn.gameObject.SetActive(false);
        m_UseBtn.gameObject.SetActive(false);
        m_DiceSlot.gameObject.SetActive(false);
        m_DisableDiceSlot.gameObject.SetActive(true);
        m_DisableDiceSlot.Init(diceData.Value);
        SetUpgradable(false);

        FDiceGradeData? gradeData = FDiceDataManager.Instance.FindGradeData(diceData.Value.Grade);
        if(gradeData != null)
            SetCommonDiceInfo(diceData.Value, gradeData.Value);
    }

    void SetUpgradable(bool InUpgradable)
    {
        foreach(FDiceStatInfo stat in m_StatInfoList)
        {
            stat.Upgradable = InUpgradable;
        }
        m_UpgradeCritical.gameObject.SetActive(InUpgradable);
        m_UpgradeBtn.enabled = InUpgradable;
        m_UpgradeBtn.GetComponent<Animator>().SetTrigger(InUpgradable ? "Normal" : "Disabled");
    }

    void SetCommonDiceInfo(in FDiceData InDiceData, in FDiceGradeData InGradeData)
    {
        m_NameText.text = InDiceData.Name;
        m_Description.text = InDiceData.Description;
        m_Critical.text = FUserDataController.Instance.Critical + "%";

        m_Grade.text = InGradeData.GradeName;
        m_UpgradeCritical.text = InGradeData.Critical + "%";
    }

    public void OnClickUpgrade()
    {
        m_UpgradeBtnHandler();
    }

    public void OnClickUse()
    {
        m_UseBtnHandler();
    }

    public void OnClose()
    {
        FPopupManager.Instance.ClosePopup();
    }
}
