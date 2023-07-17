using TMPro;
using UnityEngine;

public class FAcquiredDicePopupSlot : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI gradeText;
    [SerializeField]
    TextMeshProUGUI countText;
    [SerializeField]
    FDiceImage diceImage;

    public void SetSlot(int InDiceID, int InCount)
    {
        FDiceData diceData = FDiceDataManager.Instance.FindDiceData(InDiceID);
        if (diceData == null)
            return;

        FDiceGradeData diceGradeData = FDiceDataManager.Instance.FindGradeData(diceData.grade);
        if (diceGradeData == null)
            return;

        nameText.text = diceData.name;
        gradeText.text = diceGradeData.gradeName;
        countText.text = "x" + InCount.ToString();
        diceImage.SetImage(diceData);
    }
}
