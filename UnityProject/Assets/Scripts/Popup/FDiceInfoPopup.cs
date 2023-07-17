using FEnum;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FDiceInfoPopup : FPopupBase
{
    [SerializeField]
    FAcquiredDiceSlot acquiredDiceSlot;
    [SerializeField]
    FNotAcquiredDiceSlot notAcquiredDiceSlot;
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI grade;
    [SerializeField]
    TextMeshProUGUI description;
    [SerializeField]
    List<FDiceStatInfo> statInfoList;
    [SerializeField]
    TextMeshProUGUI critical;
    [SerializeField]
    TextMeshProUGUI upgradeCritical;
    [SerializeField]
    TextMeshProUGUI upgradeCost;
    [SerializeField]
    Button upgradeBtn;
    [SerializeField]
    Button useBtn;

    int diceID;

    public void OpenAcquiredDiceInfo(int InID)
    {
        diceID = InID;

        FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
        if (diceController == null)
            return;

        FDice dice = diceController.FindAcquiredDice(InID);
        if (dice == null)
            return;

        FDiceData diceData = FDiceDataManager.Instance.FindDiceData(InID);
        if (diceData == null)
            return;

        upgradeBtn.gameObject.SetActive(true);
        useBtn.gameObject.SetActive(true);
        notAcquiredDiceSlot.gameObject.SetActive(false);
        acquiredDiceSlot.gameObject.SetActive(true);
        acquiredDiceSlot.Init(diceData, dice);
        InitStat(diceData);

        FDiceGradeData gradeData = FDiceDataManager.Instance.FindGradeData(diceData.grade);
        if (gradeData != null)
        {
            FDiceLevelData levelData = gradeData.FindDiceLevelData(dice.level);
            if(levelData != null)
            {
                upgradeCost.text = levelData.goldCost.ToString();
                SetUpgradable(levelData.diceCountCost <= dice.count);
                SetCommonDiceInfo(diceData, gradeData);
            }
        }
    }

    public void OpenNotAcquiredDiceInfo(int InID)
    {
        diceID = InID;

        FDiceData diceData = FDiceDataManager.Instance.FindDiceData(InID);
        if (diceData == null)
            return;

        upgradeBtn.gameObject.SetActive(false);
        useBtn.gameObject.SetActive(false);
        acquiredDiceSlot.gameObject.SetActive(false);
        notAcquiredDiceSlot.gameObject.SetActive(true);
        notAcquiredDiceSlot.Init(diceData);
        InitStat(diceData);
        SetUpgradable(false);

        FDiceGradeData gradeData = FDiceDataManager.Instance.FindGradeData(diceData.grade);
        if(gradeData != null)
            SetCommonDiceInfo(diceData, gradeData);
    }

    void InitStat(FDiceData InDiceData, FDice InDice = null)
    {
        int slotIndex = (int)AbilitySlotType.Max;
        InDiceData.ForeachSkillID((int InSkillID) => {
            FSkillData skillData = FSkillDataManager.Instance.FindSkillData(InSkillID);
            if (skillData == null)
                return;

            FEffectData effectData = null;
            FAbnormalityData abnormalityData = null;

            if (skillData.abnormalityID != 0)
                abnormalityData = FAbnormalityDataManager.Instance.FindAbnormalityData(skillData.abnormalityID);

            if (skillData.skillType == FEnum.SkillType.Basic)
            {
                FProjectileData projectileData = FEffectDataManager.Instance.FindProjectileData(skillData.projectileID);
                if (projectileData != null)
                {
                    int damage = InDice == null ? projectileData.damage : projectileData.damage + projectileData.damagePerLevel * InDice.level;
                    SetStat((int)AbilitySlotType.BasicAttackDamage, AbilityType.BasicAttackDamage, damage.ToString(), projectileData.damagePerLevel.ToString());
                    SetStat((int)AbilitySlotType.BasicAttackSpeed, AbilityType.BasicAttackSpeed, skillData.interval.ToString(), "");
                    SetStat((int)AbilitySlotType.BasicAttackTarget, AbilityType.BasicAttackTarget, FAbilityDataManager.Instance.GetTargetTypeString(skillData.targetType), "");

                    effectData = FEffectDataManager.Instance.FindEffectData(projectileData.effectID);

                    if (projectileData.abnormalityID != 0)
                        abnormalityData = FAbnormalityDataManager.Instance.FindAbnormalityData(projectileData.abnormalityID);
                }
            }

            if (effectData != null && effectData.abilityType != AbilityType.None)
            {
                SetStat(slotIndex++, effectData.abilityType, effectData.value.ToString(), effectData.valuePerLevel.ToString());
            }

            if (abnormalityData != null && abnormalityData.abilityType != AbilityType.None)
            {
                SetStat(slotIndex++, abnormalityData.abilityType, abnormalityData.value.ToString(), abnormalityData.valuePerLevel.ToString());
            }
        });

        for(int i = slotIndex; i < statInfoList.Count; ++i)
        {
            SetStat(i, AbilityType.None, "-", "-");
        }
    }

    void SetStat(int InIndex, AbilityType InType, string InValue, string InUpgradeValue)
    {
        if (InIndex < 0 || statInfoList.Count <= InIndex)
            return;

        statInfoList[InIndex].Title = FAbilityDataManager.Instance.GetAbilityTitle(InType);
        statInfoList[InIndex].Value = InValue;
        statInfoList[InIndex].UpgradeValue = InUpgradeValue;
        statInfoList[InIndex].StatIcon = Resources.Load<Sprite>(FAbilityDataManager.Instance.GetAbilityIcon(InType));
    }

    void SetUpgradable(bool InUpgradable)
    {
        foreach(FDiceStatInfo stat in statInfoList)
        {
            stat.Upgradable = InUpgradable;
        }

        upgradeCritical.gameObject.SetActive(InUpgradable);
        upgradeBtn.enabled = InUpgradable;
        upgradeBtn.GetComponent<Animator>().SetTrigger(InUpgradable ? "Normal" : "Disabled");
    }

    void SetCommonDiceInfo(in FDiceData InDiceData, in FDiceGradeData InGradeData)
    {
        nameText.text = InDiceData.name;
        description.text = InDiceData.description;

        FLocalPlayerStatController statController = FLocalPlayer.Instance.FindController<FLocalPlayerStatController>();
        if(statController != null)
        {
            critical.text = statController.Critical + "%";
        }

        grade.text = InGradeData.gradeName;
        upgradeCritical.text = InGradeData.critical + "%";
    }

    public void OnClickUpgrade()
    {
        FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
        if(diceController != null)
        {
            diceController.RequestUpgradeDice(diceID);
        }
    }

    public void OnClickUse()
    {
        FDiceInventory diceInventory = FUIManager.Instance.FindUI<FDiceInventory>();
        if(diceInventory != null)
        {
            diceInventory.OpenPresetRegist(diceID);
            Close();
        }
    }

    public void OnClose()
    {
        FPopupManager.Instance.ClosePopup();
    }
}
