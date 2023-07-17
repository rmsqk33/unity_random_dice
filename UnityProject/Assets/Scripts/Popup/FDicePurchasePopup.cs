using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FDicePurchasePopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI diceName;
    [SerializeField]
    TextMeshProUGUI diceGrade;
    [SerializeField]
    Image diceImage;
    [SerializeField]
    TextMeshProUGUI count;
    [SerializeField]
    TextMeshProUGUI price;

    int diceID;

    public void OpenPopup(int InID)
    {
        diceID = InID;

        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if(storeController == null)
        {
            Close();
            return;
        }

        FDiceGoods diceGoods = storeController.FindDiceGoods(InID);
        if(diceGoods == null)
        {
            Close();
            return;
        }

        FDiceData diceData = FDiceDataManager.Instance.FindDiceData(InID);
        if(diceData == null)
        {
            Close();
            return;
        }

        FDiceGradeData gradeData = FDiceDataManager.Instance.FindGradeData(diceData.grade);
        if (gradeData == null)
        {
            Close();
            return;
        }

        diceName.text = diceData.name;
        diceImage.sprite = Resources.Load<Sprite>(diceData.iconPath);
        diceGrade.text = gradeData.gradeName;
        count.text = "x" + diceGoods.count;
        price.text = diceGoods.price.ToString();
    }

    public void OnClickPurchase()
    {
        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if (storeController != null)
        {
            storeController.RequestPurchaseDice(diceID);
        }
    }

    public void OnClose()
    {
        FPopupManager.Instance.ClosePopup();
    }
}
