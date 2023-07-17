using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FEnum;

public class FAcquiredDiceSlot : MonoBehaviour
{
    [SerializeField]
    Image background;
    [SerializeField]
    FDiceImage diceImage;
    [SerializeField]
    TextMeshProUGUI level;
    [SerializeField]
    Image expGauge;
    [SerializeField]
    TextMeshProUGUI exp;
    [SerializeField]
    Image levelUpIcon;

    int currentCount = 1;
    int maxCount = 1;

    public int Level { set { level.text = value.ToString(); } }
    public int ID { get; set; }
    public int CurrentCount
    {
        set
        {
            currentCount = value;
            UpdateCount();
        }
    }

    public int MaxCount
    {
        set
        {
            maxCount = value;
            UpdateCount();
        }
    }
    
    public void Init(in FDiceData InDiceData, in FDice InDice)
    {
        ID = InDice.id;
        Level = InDice.level;
        diceImage.SetImage(InDiceData);

        FDiceGradeData gradeData = FDiceDataManager.Instance.FindGradeData(InDiceData.grade);
        if (gradeData != null)
        {
            background.sprite = Resources.Load<Sprite>(gradeData.backgroundPath);

            FDiceLevelData levelData = gradeData.FindDiceLevelData(InDice.level);
            if (levelData != null)
            {
                currentCount = InDice.count;
                maxCount = levelData.diceCountCost;
                UpdateCount();
            }
        }
    }

    void UpdateCount()
    {
        Vector3 scale = expGauge.transform.localScale;
        scale.x = Mathf.Min((float)currentCount / (float)maxCount, 1);
        expGauge.transform.localScale = scale;

        exp.text = currentCount.ToString() + "/" + maxCount.ToString();

        levelUpIcon.gameObject.SetActive(maxCount <= currentCount);
    }

    public void OnClickSlot()
    {
        FPopupManager.Instance.OpenAcquiredDiceInfoPopup(ID);
    }
}
