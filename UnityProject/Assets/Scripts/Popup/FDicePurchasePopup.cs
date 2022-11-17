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

    public void OpenPopup(int InID, int InCount, int InPrice)
    {
        ID = InID;
        FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InID);
        if(diceData != null)
        {
            Name = diceData.Value.Name;
            DiceImage = Resources.Load<Sprite>(diceData.Value.IconPath);
            Count = InCount;
            Price = InPrice;

            FDiceGradeData? gradeData = FDiceDataManager.Instance.FindGradeData(diceData.Value.Grade);
            if (gradeData != null)
                Grade = gradeData.Value.GradeName;
        }
    }

    public void OnClickPurchase()
    {

    }

    public void OnClose()
    {
        FPopupManager.Instance.ClosePopup();
    }
}
