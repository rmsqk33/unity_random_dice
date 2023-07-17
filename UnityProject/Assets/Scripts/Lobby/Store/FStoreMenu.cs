using System;
using UnityEngine;
using UnityEngine.UI;

public class FStoreMenu : FLobbyScrollMenuBase
{
    [SerializeField]
    FStoreGoodsGroup diceList;
    [SerializeField]
    FStoreGoodsGroup boxList;
    [SerializeField]
    FStoreGoodsSlot goodsSlotPrefab;
    [SerializeField]
    GameObject content;

    DateTime elapsedTime;

    private void Start()
    {
        InitStore();
        elapsedTime = DateTime.Now;
    }

    private void Update()
    {
        if(1 <= (DateTime.Now - elapsedTime).Seconds)
        {
            UpdateResetTime();
            elapsedTime = DateTime.Now;
        }
    }

    public void InitStore()
    {
        UpdateDiceGoodsList();
        InitBoxList();
    }

    public void UpdateDiceGoodsList()
    {
        diceList.ClearGoods();

        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if (storeController != null)
        {
            storeController.ForeachDiceGoodsList((in FDiceGoods InGoods) =>
            {
                FDiceData diceData = FDiceDataManager.Instance.FindDiceData(InGoods.id);
                if (diceData != null)
                {
                    FStoreGoodsSlot slot = GameObject.Instantiate<FStoreGoodsSlot>(goodsSlotPrefab, diceList.GoodsParent);
                    slot.Name = diceData.name;
                    slot.Gold = InGoods.price;
                    slot.Count = InGoods.count;
                    slot.SoldOut = InGoods.soldOut;
                    slot.GoodsImage = Resources.Load<Sprite>(diceData.iconPath);

                    FDiceGradeData gradeData = FDiceDataManager.Instance.FindGradeData(diceData.grade);
                    if(gradeData != null)
                    {
                        slot.Background = Resources.Load<Sprite>(gradeData.backgroundPath);
                    }

                    int diceID = InGoods.id;
                    slot.GetComponent<Button>().onClick.AddListener(() => { OnClickDice(diceID); });

                    diceList.AddGoods(diceID, slot);
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
        diceList.SetDiceSoldOut(InID);
    }

    private void InitBoxList()
    {
        FStoreDataManager.Instance.ForeachStoreData((in FStoreData InData) =>
        {
            boxList.Title = InData.name;
            InData.ForeachBoxData((in FStoreBoxData InBoxData) => {
                FStoreGoodsSlot slot = GameObject.Instantiate<FStoreGoodsSlot>(goodsSlotPrefab, boxList.GoodsParent);
                slot.Name = InBoxData.name;
                slot.Dia = InBoxData.diaPrice;
                slot.GoodsImage = Resources.Load<Sprite>(InBoxData.boxImagePath);
                slot.Count = 1;

                int boxID = InBoxData.id;
                slot.GetComponent<Button>().onClick.AddListener(() => { OnClickBox(boxID); });
           
                boxList.AddGoods(boxID, slot);
            });
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

            diceList.Time = timeText;
        }
    }

    private void OnClickDice(int InID)
    {
        FPopupManager.Instance.OpenDicePurchasePopup(InID);
    }

    private void OnClickBox(int InID)
    {
        FPopupManager.Instance.OpenBoxPurchasePopup(InID);
    }
}
