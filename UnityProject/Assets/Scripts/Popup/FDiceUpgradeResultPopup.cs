using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FDiceUpgradeResultPopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI diceName;
    [SerializeField]
    TextMeshProUGUI diceGrade;
    [SerializeField]
    FDiceImage diceImage;
    [SerializeField]
    Image diceEye;
    [SerializeField]
    TextMeshProUGUI diceClass;
    [SerializeField]
    TextMeshProUGUI currentCritical;
    [SerializeField]
    TextMeshProUGUI increaseCritical;

    public void OpenPopup(FDice InDice)
    {
        FDiceData diceData = FDiceDataManager.Instance.FindDiceData(InDice.id);
        if (diceData == null)
            return;

        FDiceGradeData gradeData = FDiceDataManager.Instance.FindGradeData(diceData.grade);
        if (gradeData == null)
            return;

        FLocalPlayerStatController statController = FLocalPlayer.Instance.FindController<FLocalPlayerStatController>();
        if (statController == null)
            return;

        diceName.text = diceData.name;
        diceGrade.text = gradeData.gradeName;
        diceImage.SetImage(diceData);
        diceClass.text = "Å¬·¡½º " + InDice.level;

        currentCritical.text = (statController.Critical - gradeData.critical) + "%";
        increaseCritical.text = "+ " + gradeData.critical + "%";
    }
}
