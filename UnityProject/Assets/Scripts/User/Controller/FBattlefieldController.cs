using Packet;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FBattlefieldController : FControllerBase
{
    List<int> AcquiredBattleFielDList = new List<int>();

    public FBattlefieldController(FLocalPlayer InOwner) : base(InOwner)
    {
    }

    public void Handle_S_USER_DATA(in S_USER_DATA InPacket)
    {
        foreach(int id in InPacket.battleFieldIDList)
        {
            if (id == 0)
                break;

            AcquiredBattleFielDList.Add(id);
        }

        FBattleFieldInventory battleFieldInventory = FindBattleFieldInventoryUI();
        if (battleFieldInventory != null)
            battleFieldInventory.InitBattleFieldList();
    }

    public bool IsAcquiredBattleField(int InID)
    {
        return AcquiredBattleFielDList.Contains(InID);
    }

    FBattleFieldInventory FindBattleFieldInventoryUI()
    {
        return GameObject.FindObjectOfType<FBattleFieldInventory>();
    }

    
}
