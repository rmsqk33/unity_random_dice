using Packet;
using System.Collections.Generic;
using FEnum;
using UnityEngine;

public class FBattlefieldController : FControllerBase
{
    List<int> acquiredBattleFielDList = new List<int>();

    public FBattlefieldController(FLocalPlayer InOwner) : base(InOwner)
    {
    }

    public void Handle_S_USER_DATA(in S_USER_DATA InPacket)
    {
        acquiredBattleFielDList.Clear();
        foreach (int id in InPacket.battleFieldIDList)
        {
            if (id == 0)
                break;

            acquiredBattleFielDList.Add(id);
        }

        FBattleFieldInventory battleFieldInventory = FindBattleFieldInventoryUI();
        if (battleFieldInventory != null)
            battleFieldInventory.InitInventory();
    }

    public void Handle_S_PURCHASE_BATTLEFIELD(in S_PURCHASE_BATTLEFIELD InPacket)
    {
        BattlefieldPurchaseResult result = (BattlefieldPurchaseResult)InPacket.resultType;
        if (result == BattlefieldPurchaseResult.BATTLEFIELD_PURCHASE_RESULT_SUCCESS)
        {
            int battlefieldID = InPacket.id;
            acquiredBattleFielDList.Add(battlefieldID);

            FBattleFieldInventory battlefieldInventory = FindBattleFieldInventoryUI();
            if(battlefieldInventory != null)
            {
                FBattleFieldData battlefieldData = FBattleFieldDataManager.Instance.FindBattleFieldData(battlefieldID);
                if(battlefieldData != null)
                {
                    battlefieldInventory.AcquiredBattlefield(battlefieldData);

                    FPopupManager.Instance.OpenAcquiredBattlefieldPopup(battlefieldID);
                }
            }
        }
        else
        {
            OpenPurchaseBattlefieldResultPopup(result);
        }
    }

    public void RequestPurchaseBattlefield(int InID)
    {
        FBattleFieldData battlefieldData = FBattleFieldDataManager.Instance.FindBattleFieldData(InID);
        if(battlefieldData == null)
        {
            OpenPurchaseBattlefieldResultPopup(BattlefieldPurchaseResult.BATTLEFIELD_PURCHASE_RESULT_INVALID);
            return;
        }

        if(acquiredBattleFielDList.Contains(InID))
        {
            OpenPurchaseBattlefieldResultPopup(BattlefieldPurchaseResult.BATTLEFIELD_PURCHASE_RESULT_ALEADY_ACQUIRED);
            return;
        }

        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if(inventoryController == null || inventoryController.Dia < battlefieldData.price)
        {
            OpenPurchaseBattlefieldResultPopup(BattlefieldPurchaseResult.BATTLEFIELD_PURCHASE_RESULT_NOT_ENOUGH_MONEY);
            return;
        }

        C_PURCHASE_BATTLEFIELD packet = new C_PURCHASE_BATTLEFIELD();
        packet.id = InID;

        FServerManager.Instance.SendMessage(packet);
    }

    public bool IsAcquiredBattleField(int InID)
    {
        return acquiredBattleFielDList.Contains(InID);
    }

    FBattleFieldInventory FindBattleFieldInventoryUI()
    {
        return FUIManager.Instance.FindUI<FBattleFieldInventory>();
    }

    void OpenPurchaseBattlefieldResultPopup(BattlefieldPurchaseResult InResult)
    {
        string msg = new string("");
        switch (InResult)
        {
            case BattlefieldPurchaseResult.BATTLEFIELD_PURCHASE_RESULT_NOT_ENOUGH_MONEY: msg = "잔액이 부족합니다."; break;
            case BattlefieldPurchaseResult.BATTLEFIELD_PURCHASE_RESULT_INVALID: msg = "잘못된 데이터입니다."; break;
            case BattlefieldPurchaseResult.BATTLEFIELD_PURCHASE_RESULT_ALEADY_ACQUIRED: msg = "이미 획득한 배틀필드입니다."; break;
        }

        FPopupManager.Instance.OpenMsgPopup("구매 실패", msg);
    }
}
