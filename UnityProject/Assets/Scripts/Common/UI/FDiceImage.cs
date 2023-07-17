using FEnum;
using UnityEngine;
using UnityEngine.UI;

public class FDiceImage : MonoBehaviour
{
    [SerializeField]
    Image diceImage;
    [SerializeField]
    Image diceImageL;
    [SerializeField]
    Image diceEye;

    public void SetImage(int InDiceID, bool InShowEye = true)
    {
        FDiceData diceData = FDiceDataManager.Instance.FindDiceData(InDiceID);
        if (diceData == null)
            return;

        SetImage(diceData, InShowEye);
    }

    public void SetImage(in FDiceData InData, bool InShowEye = true)
    {
        SetImage(InData.grade, InData.iconPath, InData.color, InShowEye);
    }

    public void SetNotAcquiredDice(in FDiceData InData, bool InShowEye = true)
    {
        SetImage(InData.grade, InData.notAcquiredIconPath, InData.color, InShowEye);
    }

    private void SetImage(DiceGrade InGrade, string InPath, Color InColor, bool InShowEye)
    {
        diceImage.gameObject.SetActive(InGrade != DiceGrade.DICE_GRADE_LEGEND);
        diceImageL.gameObject.SetActive(InGrade == DiceGrade.DICE_GRADE_LEGEND);

        if (InGrade != FEnum.DiceGrade.DICE_GRADE_LEGEND)
            diceImage.sprite = Resources.Load<Sprite>(InPath);
        else
            diceImageL.sprite = Resources.Load<Sprite>(InPath);

        diceEye.gameObject.SetActive(InShowEye);
        diceEye.color = InColor;
    }
}
