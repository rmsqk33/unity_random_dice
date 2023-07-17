using System.Collections.Generic;
using UnityEngine;
using Packet;
using System;
using FEnum;

public class FDiceGoods
{
    public readonly int id;
    public readonly int count;
    public readonly int price;
    public bool soldOut = false;

    public FDiceGoods(int id, int count, int price, bool soldOut)
    {
        this.id = id;
        this.count = count;
        this.price = price;
        this.soldOut = soldOut;
    }
}

public class FStoreController : FControllerBase
{
    Dictionary<int, FDiceGoods> diceGoodsMap = new Dictionary<int, FDiceGoods>();

    public Int64 ResetTime { get; private set; }

    public FStoreController(FLocalPlayer InOwner) : base(InOwner)
    {
    }

    public void Handle_S_STORE_DICE_LIST(S_STORE_DICE_LIST InPacket)
    {
        diceGoodsMap.Clear();
        for (int i = 0; i < InPacket.diceCount; ++i)
        {
            int id = InPacket.diceList[i].id;
            int count = InPacket.diceList[i].count;
            int price = InPacket.diceList[i].price;
            bool soldOut = InPacket.diceList[i].soldOut;

            FDiceGoods goods = new FDiceGoods(id, count, price, soldOut);
            diceGoodsMap.Add(goods.id, goods);
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

        if (diceGoodsMap.ContainsKey(InPacket.id))
        {
            FDiceGoods diceGoods = diceGoodsMap[InPacket.id];
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
        FDiceGoods goods = FindDiceGoods(InID);
        if (goods == null)
        {
            OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_INVALID_GOODS);
            return;
        }

        if (goods.soldOut)
        {
            OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_SOLDOUT);
            return;
        }

        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if (inventoryController == null || inventoryController.Gold < goods.price)
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
        FStoreBoxData boxData = FStoreDataManager.Instance.FindStoreBoxData(InID);
        if(boxData == null)
        {
            OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_INVALID_GOODS);
            return;
        }

        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if (inventoryController == null)
        {
            switch (boxData.priceType)
            {
                case StorePriceType.Gold: if(inventoryController.Gold < boxData.goldPrice) OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY); return;
                case StorePriceType.Dia: if(inventoryController.Dia < boxData.diaPrice) OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY); return;
                case StorePriceType.Card: if(inventoryController.Card < boxData.cardPrice) OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY); return;
            }
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
        foreach (var pair in diceGoodsMap)
        {
            InFunc(pair.Value);
        }
    }

    public FDiceGoods FindDiceGoods(int InID)
    {
        if(diceGoodsMap.ContainsKey(InID))
            return diceGoodsMap[InID];

        return null;
    }

    FStoreMenu FindStoreUI()
    {
        return FUIManager.Instance.FindUI<FStoreMenu>();
    }


}
