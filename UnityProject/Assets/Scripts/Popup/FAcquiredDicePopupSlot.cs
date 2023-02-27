using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FAcquiredDicePopupSlot : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_NameText;
    [SerializeField]
    TextMeshProUGUI m_GradeText;
    [SerializeField]
    TextMeshProUGUI m_CountText;
    [SerializeField]
    Image m_DiceIcon;
    [SerializeField]
    Image m_DiceIconL;
    [SerializeField]
    Image m_Eye;

    public void SetSlot(int InDiceID, int InCount)
    {
        FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InDiceID);
        if (diceData == null)
            return;

        FDiceGradeData? diceGradeData = FDiceDataManager.Instance.FindGradeData(diceData.Value.Grade);
        if (diceGradeData == null)
            return;

        m_NameText.text = diceData.Value.Name;
        m_GradeText.text = diceGradeData.Value.GradeName;
        m_CountText.text = "x" + InCount.ToString();

        m_DiceIcon.gameObject.SetActive(diceData.Value.Grade != RandomDice.DiceGrade.DICE_GRADE_LEGEND);
        m_DiceIconL.gameObject.SetActive(diceData.Value.Grade == RandomDice.DiceGrade.DICE_GRADE_LEGEND);

        if (diceData.Value.Grade != RandomDice.DiceGrade.DICE_GRADE_LEGEND)
            m_DiceIcon.sprite = Resources.Load<Sprite>(diceData.Value.IconPath);
        else
            m_DiceIconL.sprite = Resources.Load<Sprite>(diceData.Value.IconPath);

        m_Eye.color = diceData.Value.Color;
    }
}
