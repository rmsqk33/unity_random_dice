using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RandomDice;

public class FAcquiredDiceSlot : MonoBehaviour
{
    [SerializeField]
    Image Background;
    [SerializeField]
    Image DiceIcon;
    [SerializeField]
    Image DiceIcon_L;
    [SerializeField]
    Image DiceEye;
    [SerializeField]
    TextMeshProUGUI LevelText;
    [SerializeField]
    Image ExpGauge;
    [SerializeField]
    TextMeshProUGUI ExpText;
    [SerializeField]
    Image LevelUpIcon;

    int m_CurrentExp = 1;
    int m_MaxExp = 1;

    public int Level { set { LevelText.text = value.ToString(); } }
    public int ID { get; set; }

    public delegate void ClickHandler(int InID);
    ClickHandler m_ClickHandler;
    public ClickHandler OnClickHandler { set { m_ClickHandler = value; } }

    public int CurrentExp
    {
        set
        {
            m_CurrentExp = value;
            UpdateExp();
        }
    }

    public void Init(in FDiceData InDiceData, in FDice InDice)
    {
        ID = InDice.id;
        Level = InDice.level;
        DiceEye.color = InDiceData.Color;
     
        DiceIcon_L.gameObject.SetActive(InDiceData.Grade == DiceGrade.DICE_GRADE_LEGEND);
        DiceIcon.gameObject.SetActive(InDiceData.Grade != DiceGrade.DICE_GRADE_LEGEND);

        if (DiceIcon_L.IsActive())
            DiceIcon_L.sprite = Resources.Load<Sprite>(InDiceData.IconPath);
        else
            DiceIcon.sprite = Resources.Load<Sprite>(InDiceData.IconPath);

        FDiceGradeData? gradeData = FDiceDataManager.Instance.FindGradeData(InDiceData.Grade);
        if (gradeData != null)
        {
            Background.sprite = Resources.Load<Sprite>(gradeData.Value.BackgroundPath);

            FDiceLevelData levelData;
            if (gradeData.Value.LevelDataMap.TryGetValue(InDice.level, out levelData))
            {
                SetExp(InDice.exp, levelData.MaxExp);
            }
        }
    }

    public void SetExp(int InExp, int InMax)
    {
        m_CurrentExp = Mathf.Min(InExp, InMax);
        m_MaxExp = InMax;
        UpdateExp();
    }

    void UpdateExp()
    {
        Vector3 scale = ExpGauge.transform.localScale;
        scale.x = (float)m_CurrentExp / (float)m_MaxExp;
        ExpGauge.transform.localScale = scale;

        ExpText.text = m_CurrentExp.ToString() + "/" + m_MaxExp.ToString();

        LevelUpIcon.enabled = m_MaxExp == m_CurrentExp;
    }

    public void OnClickSlot()
    {
        m_ClickHandler(ID);
    }
}
