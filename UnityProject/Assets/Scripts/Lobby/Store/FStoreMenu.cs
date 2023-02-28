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

        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if (storeController != null)
        {
            storeController.ForeachDiceGoodsList((in FDiceGoods InGoods) =>
            {
                FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InGoods.id);
                if (diceData != null)
                {
                    FGoodsSlot slot = GameObject.Instantiate<FGoodsSlot>(m_GoodsSlotPrefab, m_DiceList.GoodsParent);
                    slot.Name = diceData.Value.Name;
                    slot.Gold = InGoods.price;
                    slot.Count = InGoods.count;
                    slot.SoldOut = InGoods.soldOut;
                    slot.GoodsImage = Resources.Load<Sprite>(diceData.Value.IconPath);

                    FDiceGradeData? gradeData = FDiceDataManager.Instance.FindGradeData(diceData.Value.Grade);
                    if(gradeData != null)
                    {
                        slot.Background = Resources.Load<Sprite>(gradeData.Value.BackgroundPath);
                    }

                    int diceID = InGoods.id;
                    slot.GetComponent<Button>().onClick.AddListener(() => { OnClickDice(diceID); });

                    m_DiceList.AddGoods(diceID, slot);
                }
            });
        }

        UpdateResetTime();
    }

    public override void OnDeactive()
    {
        base.OnDeactive();
        FPopupManager.Instance.ClosePopup();
    }

    public void SetDiceSoldOut(int InID)
    {
        m_DiceList.SetDiceSoldOut(InID);
    }

    private void InitBoxList()
    {
        m_BoxList.Title = FStoreDataManager.Instance.BoxStoreTitle;
        FStoreDataManager.Instance.ForeachStoreBoxData((in FStoreBoxData InData) =>
        {
            FGoodsSlot slot = GameObject.Instantiate<FGoodsSlot>(m_GoodsSlotPrefab, m_BoxList.GoodsParent);
            slot.Name = InData.name;
            slot.Dia = InData.price;
            slot.GoodsImage = Resources.Load<Sprite>(InData.boxImagePath);
            slot.Count = 1;

            int boxID = InData.id;
            slot.GetComponent<Button>().onClick.AddListener(() => { OnClickBox(boxID); });

            m_BoxList.AddGoods(boxID, slot);
        });
    }

    private void UpdateResetTime()
    {
        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if (storeController != null)
        {
            DateTime resetTime = new DateTime(1970, 1, 1, 9, 0, 0, 0);
            resetTime = resetTime.AddSeconds(storeController.ResetTime);

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
    }

    private void OnClickDice(int InID)
    {
        FPopupManager.Instance.OpenDicePurchasePopup(InID, OnClickPurchaseDice);
    }

    private void OnClickBox(int InID)
    {
        FPopupManager.Instance.OpenBoxPurchasePopup(InID, OnClickPurchaseBox);
    }

    private void OnClickPurchaseDice(int InID)
    {
        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if(storeController != null)
        {
            storeController.RequestPurchaseDice(InID);
        }
    }

    private void OnClickPurchaseBox(int InID)
    {
        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if (storeController != null)
        {
            storeController.RequestPurchaseBox(InID);
        }
    }
}
