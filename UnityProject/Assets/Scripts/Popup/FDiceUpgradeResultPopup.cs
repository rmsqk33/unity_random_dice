using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FDiceUpgradeResultPopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI DiceName;
    [SerializeField]
    TextMeshProUGUI DiceGrade;
    [SerializeField]
    Image DiceIcon;
    [SerializeField]
    Image DiceIconL;
    [SerializeField]
    Image DiceEye;
    [SerializeField]
    TextMeshProUGUI DiceClass;
    [SerializeField]
    TextMeshProUGUI CurrentCritical;
    [SerializeField]
    TextMeshProUGUI IncreaseCritical;

    public void OpenPopup(FDice InDice)
    {
        FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InDice.id);
        if (diceData == null)
            return;

        FDiceGradeData? gradeData = FDiceDataManager.Instance.FindGradeData(diceData.Value.Grade);
        if (gradeData == null)
            return;

        FStatController statController = FLocalPlayer.Instance.FindController<FStatController>();
        if (statController == null)
            return;

        DiceName.text = diceData.Value.Name;
        DiceGrade.text = gradeData.Value.GradeName;

        DiceIcon.gameObject.SetActive(diceData.Value.Grade != RandomDice.DiceGrade.DICE_GRADE_LEGEND);
        DiceIconL.gameObject.SetActive(diceData.Value.Grade == RandomDice.DiceGrade.DICE_GRADE_LEGEND);

        if (diceData.Value.Grade != RandomDice.DiceGrade.DICE_GRADE_LEGEND)
            DiceIcon.sprite = Resources.Load<Sprite>(diceData.Value.IconPath);
        else
            DiceIconL.sprite = Resources.Load<Sprite>(diceData.Value.IconPath);

        DiceEye.color = diceData.Value.Color;
        DiceClass.text = "Å¬·¡½º " + InDice.level;

        CurrentCritical.text = (statController.Critical - gradeData.Value.Critical) + "%";
        IncreaseCritical.text = "+ " + gradeData.Value.Critical + "%";
    }
}
