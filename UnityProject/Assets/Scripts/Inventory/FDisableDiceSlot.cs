using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RandomDice;

public class FDisableDiceSlot : MonoBehaviour
{
    [SerializeField]
    Image Background;
    [SerializeField]
    Image DiceIcon;
    [SerializeField]
    Image DiceIcon_L;
    [SerializeField]
    TextMeshProUGUI GradeText;

    public int ID { get; set; }

    public delegate void ClickHandler(int InID);
    ClickHandler m_ClickHandler;
    public ClickHandler OnClickHandler { set { m_ClickHandler = value; } }

    public void Init(in FDiceData InData)
    {
        ID = InData.ID;
        DiceIcon_L.gameObject.SetActive(InData.Grade == DiceGrade.DICE_GRADE_LEGEND);
        DiceIcon.gameObject.SetActive(InData.Grade != DiceGrade.DICE_GRADE_LEGEND);

        if (DiceIcon_L.IsActive())
            DiceIcon_L.sprite = Resources.Load<Sprite>(InData.DisableIconPath);
        else
            DiceIcon.sprite = Resources.Load<Sprite>(InData.DisableIconPath);

        FDiceGradeData? gradeData = FDiceDataManager.Instance.FindGradeData(InData.Grade);
        if (gradeData != null)
        {
            Background.sprite = Resources.Load<Sprite>(gradeData.Value.BackgroundPath);
            GradeText.text = gradeData.Value.GradeName;
        }
    }

    public void OnClickSlot()
    {
        m_ClickHandler(ID);
    }
}
