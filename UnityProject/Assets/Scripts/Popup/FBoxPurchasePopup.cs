using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FEnum;

public class FBoxPurchasePopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    TextMeshProUGUI price;
    [SerializeField]
    List<Image> priceIconList;
    [SerializeField]
    Image boxImage;
    [SerializeField]
    List<FBoxGoods> goodsList;
    [SerializeField]
    List<Transform> lineList;

    private int boxID;

    public void OpenPopup(int InID)
    {
        boxID = InID;

        FStoreBoxData boxData = FStoreDataManager.Instance.FindStoreBoxData(InID);
        if(boxData == null)
        {
            Close();
            return;
        }

        title.text = boxData.name;
        boxImage.sprite = Resources.Load<Sprite>(boxData.boxImagePath);

        price.text = "x ";
        switch (boxData.priceType)
        {
            case StorePriceType.Gold: price.text += boxData.goldPrice; break;
            case StorePriceType.Dia: price.text += boxData.diaPrice; break;
            case StorePriceType.Card: price.text += boxData.cardPrice; break;
        }

        for(int i = 0; i < (int)StorePriceType.Max; ++i)
        {
            if (i < priceIconList.Count)
                priceIconList[i].gameObject.SetActive((int)boxData.priceType == i);
        }
        

        goodsList[0].Count = "x" + boxData.gold;

        int goodsCount = 1;
        boxData.ForeachGoodsData((FBoxGoodsData InData) => {
            FBoxGoods boxGoods = goodsList[goodsCount];
            boxGoods.gameObject.SetActive(true);
            boxGoods.Count = "x " + (InData.min == InData.max ? InData.min : InData.min + " ~ " + InData.max);

            FBoxGoodsImageData imageData = FStoreDataManager.Instance.GetBoxGoodsImageData(InData.grade);
            if (imageData != null)
            {
                if (imageData.image != null) boxGoods.GoodsIcon = Resources.Load<Sprite>(imageData.image);
                else if (imageData.prefab != null) boxGoods.AddGoodsIconPrefab(Instantiate(Resources.Load<Transform>(imageData.prefab)));
            }

            FDiceGradeData diceGradeData = FDiceDataManager.Instance.FindGradeData(InData.grade);
            if (diceGradeData != null)
            {
                boxGoods.GoodsName = diceGradeData.gradeName;
            }

            if (goodsCount % 2 == 0)
            {
                int lineIndex = goodsCount / 2 - 1;
                lineList[lineIndex].gameObject.SetActive(true);
                boxGoods.transform.parent.gameObject.SetActive(true);
            }

            ++goodsCount;
        });

        for(int i = goodsCount; i < goodsList.Count; ++i)
        {
            FBoxGoods boxGoods = goodsList[i];
            boxGoods.gameObject.SetActive(false);
            if (i % 2 == 0)
            {
                int lineIndex = i / 2 - 1;
                lineList[lineIndex].gameObject.SetActive(false);
                boxGoods.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickPurchase()
    {
        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if (storeController != null)
        {
            storeController.RequestPurchaseBox(boxID);
        }
    }
}
