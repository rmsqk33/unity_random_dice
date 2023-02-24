using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FStoreMenu : FLobbyScrollMenuBase
{
    [SerializeField]
    FGoodsGroup m_DiceList;
    [SerializeField]
    FGoodsGroup m_BoxList;
    [SerializeField]
    FGoodsSlot m_GoodsSlotPrefab;
    [SerializeField]
    GameObject m_Content;

    DateTime m_ElapsedTime;

    private void Start()
    {
        InitStore();
        m_ElapsedTime = DateTime.Now;
    }

    private void Update()
    {
        if(1 <= (DateTime.Now - m_ElapsedTime).Seconds)
        {
            UpdateResetTime();
            m_ElapsedTime = DateTime.Now;
        }
    }

    public void InitStore()
    {
        UpdateDiceGoodsList();
        InitBoxList();
    }

    public void UpdateDiceGoodsList()
    {
        m_DiceList.ClearGoods();
        FStoreController.Instance.ForeachDiceGoodsList((in FDiceGoods InGoods) => {
            FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InGoods.id);
            if (diceData != null)
            {
                FGoodsSlot slot = GameObject.Instantiate<FGoodsSlot>(m_GoodsSlotPrefab);
                slot.Name = diceData.Value.Name;
                slot.Price = InGoods.price;
                slot.Count = InGoods.count;
                slot.Image = Resources.Load<Sprite>(diceData.Value.IconPath);

                int diceID = InGoods.id;
                slot.GetComponent<Button>().onClick.AddListener(() => { OnClickDice(diceID); });
                
                m_DiceList.AddGoods(slot);
            }
        });

        UpdateResetTime();

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)m_DiceList.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)m_BoxList.transform);
    }

    public override void OnDeactive()
    {
        base.OnDeactive();
        FPopupManager.Instance.ClosePopup();
    }

    private void InitBoxList()
    {
        FDataNode dataNode = FDataCenter.Instance.GetDataNodeWithQuery("StoreDataList.BoxStoreData");
        m_BoxList.Title = dataNode.GetStringAttr("name");

        dataNode.ForeachChildNodes("Box", (in FDataNode InNode) => {
            FGoodsSlot slot = GameObject.Instantiate<FGoodsSlot>(m_GoodsSlotPrefab);
            slot.Name = InNode.GetStringAttr("name");
            slot.Price = InNode.GetIntAttr("price");
            slot.Image = Resources.Load<Sprite>(InNode.GetStringAttr("image"));
            slot.Count = 1;

            int boxID = InNode.GetIntAttr("id");
            slot.GetComponent<Button>().onClick.AddListener(() => { OnClickBox(boxID); });

            m_BoxList.AddGoods(slot);
        });
    }

    private void UpdateResetTime()
    {
        DateTime resetTime = new DateTime(1970, 1, 1, 9, 0, 0, 0);
        resetTime = resetTime.AddSeconds(FStoreController.Instance.ResetTime);

        TimeSpan diff = resetTime - DateTime.Now;

        string timeText = "갱신까지 남은 시간 : ";
        if (0 < diff.Days)
            timeText += diff.Days + "일 ";

        if (0 < diff.Hours)
            timeText += diff.Hours + "시 ";

        if (0 < diff.Minutes)
            timeText += diff.Minutes + "분 ";

        timeText += diff.Seconds + "초";

        m_DiceList.Time = timeText;
    }

    private void OnClickDice(int InID)
    {
        FDiceGoods? goods = FStoreController.Instance.FindDiceGoods(InID);
        if (goods != null)
        {
            FPopupManager.Instance.OpenDicePurchasePopup(InID, goods.Value.count, goods.Value.price);
        }
    }

    private void OnClickBox(int InID)
    {

    }
}
