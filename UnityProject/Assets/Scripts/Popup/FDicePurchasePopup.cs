using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FDicePurchasePopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI m_DiceName;
    [SerializeField]
    TextMeshProUGUI m_DiceGrade;
    [SerializeField]
    Image m_DiceImage;
    [SerializeField]
    TextMeshProUGUI m_Count;
    [SerializeField]
    TextMeshProUGUI m_Price;

    public int ID { get; set; }
    public string Name { set { m_DiceName.text = value; } }
    public string Grade { set { m_DiceGrade.text = value; } }
    public Sprite DiceImage { set { m_DiceImage.sprite = value; } }
    public int Count { set { m_Count.text = "x" + value; } }
    public int Price { set { m_Price.text = value.ToString(); } }

    public delegate void ButtonHandler(int InID);
    ButtonHandler m_PurchaseBtnHandler;
    
    public ButtonHandler PurchaseBtnHandler { set { m_PurchaseBtnHandler = value; } }

    public void OpenPopup(int InID)
    {
        ID = InID;

        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if(storeController == null)
        {
            Close();
            return;
        }

        FDiceGoods? diceGoods = storeController.FindDiceGoods(InID);
        if(diceGoods == null)
        {
            Close();
            return;
        }

        FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InID);
        if(diceData == null)
        {
            Close();
            return;
        }

        Name = diceData.Value.Name;
        DiceImage = Resources.Load<Sprite>(diceData.Value.IconPath);
        Count = diceGoods.Value.count;
        Price = diceGoods.Value.price;

        FDiceGradeData? gradeData = FDiceDataManager.Instance.FindGradeData(diceData.Value.Grade);
        if (gradeData != null)
            Grade = gradeData.Value.GradeName;
    }

    public void OnClickPurchase()
    {
        m_PurchaseBtnHandler(ID);
    }

    public void OnClose()
    {
        FPopupManager.Instance.ClosePopup();
    }
}
