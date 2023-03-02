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

    int m_CurrentCount = 1;
    int m_MaxCount = 1;

    public int Level { set { LevelText.text = value.ToString(); } }
    public int ID { get; set; }

    public delegate void ClickHandler(int InID);
    ClickHandler m_ClickHandler;
    public ClickHandler OnClickHandler { set { m_ClickHandler = value; } }

    public int CurrentCount
    {
        set
        {
            m_CurrentCount = value;
            UpdateCount();
        }
    }

    public int MaxCount
    {
        set
        {
            m_MaxCount = value;
            UpdateCount();
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
                m_CurrentCount = InDice.count;
                m_MaxCount = levelData.DiceCountCost;
                UpdateCount();
            }
        }
    }

    void UpdateCount()
    {
        Vector3 scale = ExpGauge.transform.localScale;
        scale.x = Mathf.Min((float)m_CurrentCount / (float)m_MaxCount, 1);
        ExpGauge.transform.localScale = scale;

        ExpText.text = m_CurrentCount.ToString() + "/" + m_MaxCount.ToString();

        LevelUpIcon.gameObject.SetActive(m_MaxCount <= m_CurrentCount);
    }

    public void OnClickSlot()
    {
        m_ClickHandler(ID);
    }
}
