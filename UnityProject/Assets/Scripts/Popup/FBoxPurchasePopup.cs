using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FBoxPurchasePopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI m_Title;
    [SerializeField]
    TextMeshProUGUI m_Price;
    [SerializeField]
    Image m_BoxImage;
    [SerializeField]
    List<FBoxGoods> m_GoodsList;
    [SerializeField]
    List<Transform> m_LineList;

    private int id { get; set; }

    public delegate void ButtonHandler(int InID);
    ButtonHandler m_PurchaseBtnHandler;

    public ButtonHandler PurchaseBtnHandler { set { m_PurchaseBtnHandler = value; } }

    public void OpenPopup(int InID)
    {
        id = InID;

        FStoreBoxData boxData = FStoreDataManager.Instance.FindStoreBoxData(InID).Value;
        m_Title.text = boxData.name;
        m_Price.text = "x " + boxData.price;
        m_BoxImage.sprite = Resources.Load<Sprite>(boxData.boxImagePath);

        m_GoodsList[0].Count = "x" + boxData.gold;
        for(int i = 1; i < m_GoodsList.Count; ++i)
        {
            FBoxGoods boxGoods = m_GoodsList[i];
            
            bool isActiveGoods = i <= boxData.diceList.Count;
            boxGoods.gameObject.SetActive(isActiveGoods);

            if (isActiveGoods)
            {
                FBoxGoodsData boxGoodsData = boxData.diceList[i - 1];
                boxGoods.GoodsIcon = Resources.Load<Sprite>(FStoreDataManager.Instance.GetBoxGoodsImage(boxGoodsData.grade));
                boxGoods.Count = "x " + (boxGoodsData.min == boxGoodsData.max ? boxGoodsData.min : boxGoodsData.min + " ~ " + boxGoodsData.max);

                FDiceGradeData? diceGradeData = FDiceDataManager.Instance.FindGradeData(boxGoodsData.grade);
                if (diceGradeData != null)
                {
                    boxGoods.GoodsName = diceGradeData.Value.GradeName;
                }
            }

            if(i % 2 == 0)
            {
                int lineIndex = i / 2 - 1;
                m_LineList[lineIndex].gameObject.SetActive(isActiveGoods);
                boxGoods.transform.parent.gameObject.SetActive(isActiveGoods);
            }
        }
    }

    public void OnClickPurchase()
    {
        m_PurchaseBtnHandler(id);
    }
}
