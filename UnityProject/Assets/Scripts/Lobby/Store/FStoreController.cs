using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Packet;
using System;
using RandomDice;

public struct FDiceGoods
{
    public int id;
    public int count;
    public int price;
    public bool soldOut;
}

public class FStoreController : FNonObjectSingleton<FStoreController>
{
    List<FDiceGoods> m_DiceGoodsList = new List<FDiceGoods>();
    Int64 m_ResetTime = 0;

    public Int64 ResetTime { get { return m_ResetTime; } }

    public void Handle_S_STORE_DICE_LIST(S_STORE_DICE_LIST InPacket)
    {
        m_DiceGoodsList.Clear();
        for (int i = 0; i < InPacket.diceCount; ++i)
        {
            FDiceGoods goods = new FDiceGoods();
            goods.id = InPacket.diceList[i].id;
            goods.count = InPacket.diceList[i].count;
            goods.price = InPacket.diceList[i].price;

            m_DiceGoodsList.Add(goods);
        }

        m_ResetTime = InPacket.resetTime;

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

        FStoreMenu storeMenu = FindStoreUI();
        if (storeMenu != null)
            storeMenu.SetDiceSoldOut(InPacket.id);
    }

    public void RequestPurchaseDice(int InID)
    {
        FDiceGoods? goods = FindDiceGoods(InID);
        if (goods == null)
        {
            OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_INVALID_GOODS);
            return;
        }

        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if(inventoryController == null || goods.Value.price < inventoryController.Gold)
        {
            OpenPurchaseResultPopup(StorePurchaseResult.STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY);
            return;
        }

        C_PURCHASE_DICE packet = new C_PURCHASE_DICE();
        packet.id = InID;

        FServerManager.Instance.SendMessage(packet);
    }

    private void OpenPurchaseResultPopup(StorePurchaseResult InResult)
    {
        if(InResult == StorePurchaseResult.STORE_PURCHASE_RESULT_INVALID_GOODS)
            FPopupManager.Instance.OpenMsgPopup("구매 오류", "존재하지 않는 주사위입니다.");
        else if(InResult == StorePurchaseResult.STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY)
            FPopupManager.Instance.OpenMsgPopup("구매 오류", "잔액이 모자라 구매할 수 없습니다.");
    }

    public delegate void ForeachDiceGoodsListFunc(in FDiceGoods InGoods);
    public void ForeachDiceGoodsList(in ForeachDiceGoodsListFunc InFunc)
    {
        foreach(FDiceGoods goods in m_DiceGoodsList)
        {
            InFunc(goods);
        }
    }

    public FDiceGoods? FindDiceGoods(int InID)
    {
        foreach(FDiceGoods goods in m_DiceGoodsList)
        {
            if (goods.id == InID)
                return goods;
        }
        return null;
    }

    private FStoreMenu FindStoreUI()
    {
        return GameObject.FindObjectOfType<FStoreMenu>();
    }

        
}
