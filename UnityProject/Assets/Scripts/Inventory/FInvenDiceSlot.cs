using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FInvenDiceSlot : MonoBehaviour
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

    int m_CurrentExp = 0;
    int m_MaxExp = 0;

    public void Init(in FDiceData InDiceData, in FDice InDice)
    {
        ID = InDice.id;
        Level = InDice.level;
        DiceEye.color = InDiceData.Color;
     
        DiceIcon_L.enabled = InDiceData.Grade == 4;
        DiceIcon.enabled = InDiceData.Grade != 4;

        if (InDiceData.Grade == 4)
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

    public int Level { set { LevelText.text = value.ToString(); } }
    public int ID { get; set; }

    public int CurrentExp
    {
        set
        {
            m_CurrentExp = value;
            UpdateExp();
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
}
