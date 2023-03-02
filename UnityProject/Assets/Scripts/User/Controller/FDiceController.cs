using Packet;
using RandomDice;
using System.Collections.Generic;
using UnityEngine;

public class FDice
{
    public int id;
    public int level;
    public int count;
}

public class FDiceController : FControllerBase
{
    Dictionary<int, FDice> AcquiredDiceMap = new Dictionary<int, FDice>();

    public FDiceController(FLocalPlayer InOwner) : base(InOwner)
    {
    }

    public void Handle_S_USER_DATA(in S_USER_DATA InPacket)
    {
        foreach (S_USER_DATA.DICE_DATA diceData in InPacket.diceDataList)
        {
            if (diceData.id == 0)
                break;

            FDice dice = new FDice();
            dice.id = diceData.id;
            dice.count = diceData.count;
            dice.level = diceData.level;

            AcquiredDiceMap.Add(diceData.id, dice);
        }

        FDiceInventory diceInventory = FindDiceInventoryUI();
        if (diceInventory != null)
            diceInventory.InitInventory();
    }

    public void Handle_S_ADD_DICE(in S_ADD_DICE InPacket)
    {
        List<KeyValuePair<int, int>> addDiceList = new List<KeyValuePair<int, int>>();
        for(int i = 0; i < InPacket.diceCount; ++i)
        {
            int diceID = InPacket.diceList[i].id;
            int diceCount = InPacket.diceList[i].count;

            FDice dice = FindAcquiredDice(diceID);
            if (dice == null)
            {
                FDiceGradeData? gradeData = FDiceDataManager.Instance.FindGradeDataByID(diceID);
                if (gradeData != null)
                {
                    AddAcquiredDice(diceID, diceCount, gradeData.Value.InitialLevel);
                }
            }
            else
            {
                SetDiceCount(diceID, dice.count + diceCount);
            }

            addDiceList.Add(new KeyValuePair<int, int>(diceID, diceCount));
        }
     
        FPopupManager.Instance.OpenAcquiredDicePopup(addDiceList);
    }

    public void Handle_S_UPGRADE_DICE(in S_UPGRADE_DICE InPacket)
    {
        if((DiceUpgradeResult)InPacket.resultType == DiceUpgradeResult.DICE_UPGRADE_RESULT_SUCCESS)
        {
            int diceID = InPacket.id;
            int diceCount = InPacket.count;
            int diceLevel = InPacket.level;

            if (AcquiredDiceMap.ContainsKey(diceID))
            {
                SetDiceCount(diceID, diceCount);
                SetDiceLevel(diceID, diceLevel);

                FStatController statController = FLocalPlayer.Instance.FindController<FStatController>();
                if(statController != null)
                {
                    statController.AddCritical(diceID);
                }

                FPopupManager.Instance.OpenDiceUpgradeResultPopup(AcquiredDiceMap[diceID]);
            }
        }
        else
        {
            OpenDiceUpgradeResultPopup((DiceUpgradeResult)InPacket.resultType);
        }
    }

    public void RequestUpgradeDice(int InID)
    {
        if (AcquiredDiceMap.ContainsKey(InID) == false)
        {
            OpenDiceUpgradeResultPopup(DiceUpgradeResult.DICE_UPGRADE_RESULT_INVALID_DICE);
            return;
        }

        FDice dice = AcquiredDiceMap[InID];
        FDiceGradeData? gradeData = FDiceDataManager.Instance.FindGradeDataByID(dice.id);
        if(gradeData == null)
        {
            OpenDiceUpgradeResultPopup(DiceUpgradeResult.DICE_UPGRADE_RESULT_INVALID_DICE);
            return;
        }

        if(dice.level == gradeData.Value.MaxLevel)
        {
            OpenDiceUpgradeResultPopup(DiceUpgradeResult.DICE_UPGRADE_RESULT_MAX_LEVEL);
            return;
        }

        if(gradeData.Value.LevelDataMap.ContainsKey(dice.level) == false)
        {
            OpenDiceUpgradeResultPopup(DiceUpgradeResult.DICE_UPGRADE_RESULT_INVALID_DICE);
            return;
        }

        FDiceLevelData diceLevelData = gradeData.Value.LevelDataMap[dice.level];
        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if(inventoryController == null || inventoryController.Gold < diceLevelData.GoldCost)
        {
            OpenDiceUpgradeResultPopup(DiceUpgradeResult.DICE_UPGRADE_RESULT_NOT_ENOUGH_MONEY);
            return;
        }

        if(dice.count < diceLevelData.DiceCountCost)
        {
            OpenDiceUpgradeResultPopup(DiceUpgradeResult.DICE_UPGRADE_RESULT_NOT_ENOUGH_DICE);
            return;
        }

        C_UPGRADE_DICE packet = new C_UPGRADE_DICE();
        packet.id = InID;

        FServerManager.Instance.SendMessage(packet);
    }

    public delegate void ForeachAcquiredDiceFunc(FDice InDice);
    public void ForeachAcquiredDice(ForeachAcquiredDiceFunc InFunc)
    {
        foreach(var iter in AcquiredDiceMap)
        {
            InFunc(iter.Value);
        }
    }

    public FDice FindAcquiredDice(int InID)
    {
        return AcquiredDiceMap.ContainsKey(InID) ? AcquiredDiceMap[InID] : null;
    }

    void AddAcquiredDice(int InID, int InCount, int InLevel)
    {
        FDice dice = new FDice();
        dice.id = InID;
        dice.count = InCount;
        dice.level = InLevel;

        AcquiredDiceMap.Add(InID, dice);

        FStatController statController = FLocalPlayer.Instance.FindController<FStatController>();
        if (statController != null)
            statController.AddCritical(InID, InLevel);

        FDiceInventory diceInventory =  FindDiceInventoryUI();
        if(diceInventory != null)
            diceInventory.AcquireDice(dice);
    }

    void SetDiceCount(int InID, int InCount)
    {
        if (!AcquiredDiceMap.ContainsKey(InID))
            return;

        AcquiredDiceMap[InID].count = InCount;

        FDiceInventory diceInventory = FindDiceInventoryUI();
        if (diceInventory != null)
            diceInventory.SetDiceCount(InID, InCount);
    }

    void SetDiceLevel(int InID, int InLevel)
    {
        if (!AcquiredDiceMap.ContainsKey(InID))
            return;

        FDiceLevelData? diceLevelData = FDiceDataManager.Instance.FindDiceLevelData(InID, InLevel);
        if (diceLevelData == null)
            return;

        AcquiredDiceMap[InID].level = InLevel;

        FDiceInventory diceInventory = FindDiceInventoryUI();
        if (diceInventory != null)
        {
            diceInventory.SetDiceLevel(InID, InLevel);
            diceInventory.SetDiceMaxExp(InID, diceLevelData.Value.DiceCountCost);
        }
    }

    void OpenDiceUpgradeResultPopup(DiceUpgradeResult InResult)
    {
        string contentStr = new string("");
        switch (InResult)
        {
            case DiceUpgradeResult.DICE_UPGRADE_RESULT_INVALID_DICE: contentStr = "존재하지 않는 주사위입니다."; break;
            case DiceUpgradeResult.DICE_UPGRADE_RESULT_NOT_ENOUGH_MONEY: contentStr = "골드가 부족합니다."; break;
            case DiceUpgradeResult.DICE_UPGRADE_RESULT_NOT_ENOUGH_DICE: contentStr = "주사위가 부족합니다."; break;
            case DiceUpgradeResult.DICE_UPGRADE_RESULT_MAX_LEVEL: contentStr = "이미 최대 레벨입니다."; break;
        }

        FPopupManager.Instance.OpenMsgPopup("강화 실패", contentStr);
    }

    FDiceInventory FindDiceInventoryUI()
    {
        return GameObject.FindObjectOfType<FDiceInventory>();
    }
}
