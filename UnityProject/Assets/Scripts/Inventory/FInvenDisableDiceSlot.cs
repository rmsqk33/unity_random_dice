using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FInvenDisableDiceSlot : MonoBehaviour
{
    [SerializeField]
    Image Background;
    [SerializeField]
    Image DiceIcon;
    [SerializeField]
    Image DiceIcon_L;
    [SerializeField]
    TextMeshProUGUI GradeText;

    public void Init(in FDiceData InData)
    {
        DiceIcon_L.enabled = InData.Grade == 4;
        DiceIcon.enabled = InData.Grade != 4;

        if (InData.Grade == 4)
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
}
