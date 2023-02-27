using Packet;
using System.Collections.Generic;
using UnityEngine;

public struct FDice
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

            FDice? dice = FindAcquiredDice(diceID);
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
                SetDiceCount(diceID, dice.Value.count + diceCount);
            }

            addDiceList.Add(new KeyValuePair<int, int>(diceID, diceCount));
            FPopupManager.Instance.OpenAcquiredDicePopup(addDiceList);
        }
    }

    public delegate void ForeachAcquiredDiceFunc(FDice InDice);
    public void ForeachAcquiredDice(ForeachAcquiredDiceFunc InFunc)
    {
        foreach(var iter in AcquiredDiceMap)
        {
            InFunc(iter.Value);
        }
    }

    public FDice? FindAcquiredDice(int InID)
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

        FDice dice = AcquiredDiceMap[InID];
        dice.count = InCount;

        FDiceInventory diceInventory = FindDiceInventoryUI();
        if (diceInventory != null)
            diceInventory.SetDiceCount(InID, InCount);
    }

    bool IsAcquiredDice(int InID)
    {
        return AcquiredDiceMap.ContainsKey(InID);
    }

    FDiceInventory FindDiceInventoryUI()
    {
        return GameObject.FindObjectOfType<FDiceInventory>();
    }
}
