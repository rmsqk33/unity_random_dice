using Packet;
using System.Collections.Generic;
using UnityEngine;

public struct FDice
{
    public int id;
    public int level;
    public int exp;
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
            dice.exp = diceData.count;
            dice.level = diceData.level;

            AcquiredDiceMap.Add(diceData.id, dice);
        }


        FDiceInventory diceInventory = FindDiceInventoryUI();
        if (diceInventory != null)
            diceInventory.InitInventory();
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

    FDiceInventory FindDiceInventoryUI()
    {
        GameObject gameObject = GameObject.Find("DiceInventory");
        if (gameObject != null)
            return gameObject.GetComponent<FDiceInventory>();

        return null;
    }
}
