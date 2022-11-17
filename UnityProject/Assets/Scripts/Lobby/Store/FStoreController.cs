using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Packet;

public struct FDiceGoods
{
    public int id;
    public int count;
    public int price;
}

public class FStoreController : FNonObjectSingleton<FStoreController>
{
    List<FDiceGoods> m_DiceGoodsList = new List<FDiceGoods>();

#if DEBUG
    [RuntimeInitializeOnLoadMethod]
    private static void Test()
    {
        S_STORE_DICE_LIST packet = new S_STORE_DICE_LIST();
        packet.diceCount = 5;
        for(int i = 0; i < packet.diceCount; ++i)
        {
            packet.diceList[i].count = 50;
            packet.diceList[i].id = i + 1;
            packet.diceList[i].price = 100 * (i + 1);
        }

        FStoreController.Instance.Handler_S_STORE_DICE_LIST(packet);
    }
#endif

    public void Handler_S_STORE_DICE_LIST(S_STORE_DICE_LIST InPacket)
    {
        for(int i = 0; i < InPacket.diceCount; ++i)
        {
            FDiceGoods goods = new FDiceGoods();
            goods.id = InPacket.diceList[i].id;
            goods.count = InPacket.diceList[i].count;
            goods.price = InPacket.diceList[i].price;

            m_DiceGoodsList.Add(goods);
        }

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
