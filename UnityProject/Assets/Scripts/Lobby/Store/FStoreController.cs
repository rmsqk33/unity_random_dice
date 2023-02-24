using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Packet;
using System;

public struct FDiceGoods
{
    public int id;
    public int count;
    public int price;
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
