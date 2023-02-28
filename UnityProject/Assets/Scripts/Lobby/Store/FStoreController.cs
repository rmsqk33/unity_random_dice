using System.Collections.Generic;
using UnityEngine;
using Packet;
using System;
using RandomDice;
using UnityEditor;

public struct FDiceGoods
{
    public int id;
    public int count;
    public int price;
    public bool soldOut;
}

public class FStoreController : FControllerBase
{
    Dictionary<int, FDiceGoods> DiceGoodsMap = new Dictionary<int, FDiceGoods>();
    public Int64 ResetTime { get; private set; }

    public FStoreController(FLocalPlayer InOwner) : base(InOwner)
    {
    }

    public void Handle_S_STORE_DICE_LIST(S_STORE_DICE_LIST InPacket)
    {
        DiceGoodsMap.Clear();
        for (int i = 0; i < InPacket.diceCount; ++i)
        {
            FDiceGoods goods = new FDiceGoods();
            goods.id = InPacket.diceList[i].id;
            goods.count = InPacket.diceList[i].count;
            goods.price = InPacket.diceList[i].price;
            goods.soldOut = InPacket.diceList[i].soldOut;

            DiceGoodsMap.Add(goods.id, goods);
        }

        ResetTime = InPacket.resetTime;

        FStoreMenu storeMenu = FindStoreUI();
        if (storeMenu != null)
            storeMenu.UpdateDiceGoodsList();
    }

    public void Handle_S_PURCHASE_DICE(S_PURCHASE_DICE InPacket)
    {
        StorePurchaseResult result = (StorePurchaseResult)InPacket.resultType;
        if (result != StorePurchaseResult.STORE_PURCHASE_RESULT_SUCCESS)
        {
            OpenPurchaseResultPopup(result);
            return;
        }

        if (DiceGoodsMap.ContainsKey(InPacket.id))
        {
            FDiceGoods diceGoods = DiceGoodsMap[InPacket.id];
            diceGoods.soldOut = true;

            FStoreMenu storeMenu = FindStoreUI();
            if (storeMenu != null)
            {
                storeMenu.SetDiceSoldOut(InPacket.id);
            }
        }
    }

    public void Handle_S_PURCHASE_BOX(S_PURCHASE_BOX InPacket)
    {
        StorePurchaseResult result = (StorePurchaseResult)InPacket.resultType;
        if (result != StorePurchaseResult.STORE_PURCHASE_RESULT_SUCCESS)
        {
            OpenPurchaseResultPopup(result);
        }
    }

    public void RequestPurchaseDice(int InID)
    {
        FDiceGoods? goods = FindDiceGoods(InID);
        if (goods == null)
        {
            OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_INVALID_GOODS);
            return;
        }

        if (goods.Value.soldOut)
        {
            OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_SOLDOUT);
            return;
        }

        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if (inventoryController == null || inventoryController.Gold < goods.Value.price)
        {
            OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY);
            return;
        }

        C_PURCHASE_DICE packet = new C_PURCHASE_DICE();
        packet.id = InID;

        FServerManager.Instance.SendMessage(packet);
    }

    public void RequestPurchaseBox(int InID)
    {
        FStoreBoxData? boxData = FStoreDataManager.Instance.FindStoreBoxData(InID);
        if(boxData == null)
        {
            OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_INVALID_GOODS);
            return;
        }

        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if (inventoryController == null || inventoryController.Gold < boxData.Value.price)
        {
            OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY);
            return;
        }

        C_PURCHASE_BOX packet = new C_PURCHASE_BOX();
        packet.id = InID;

        FServerManager.Instance.SendMessage(packet);
    }

    private void OpenPurchaseResultPopup(StorePurchaseResult InResult)
    {
        switch (InResult)
        {
            case StorePurchaseResult.STORE_PURCHASE_RESULT_INVALID_GOODS:
                FPopupManager.Instance.OpenMsgPopup("구매 오류", "존재하지 않는 주사위입니다.");
                break;

            case StorePurchaseResult.STORE_PURCHASE_RESULT_SOLDOUT:
                FPopupManager.Instance.OpenMsgPopup("구매 오류", "품절된 상품입니다.");
                break;

            case StorePurchaseResult.STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY:
                FPopupManager.Instance.OpenMsgPopup("구매 오류", "잔액이 모자라 구매할 수 없습니다.");
                break;
        }
    }

    public delegate void ForeachDiceGoodsListFunc(in FDiceGoods InGoods);
    public void ForeachDiceGoodsList(in ForeachDiceGoodsListFunc InFunc)
    {
        foreach (var pair in DiceGoodsMap)
        {
            InFunc(pair.Value);
        }
    }

    public FDiceGoods? FindDiceGoods(int InID)
    {
        if(DiceGoodsMap.ContainsKey(InID))
            return DiceGoodsMap[InID];

        return null;
    }

    private FStoreMenu FindStoreUI()
    {
        return GameObject.FindObjectOfType<FStoreMenu>();
    }


}
