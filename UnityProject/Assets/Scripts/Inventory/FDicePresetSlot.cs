using RandomDice;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FDicePresetSlot : MonoBehaviour
{
    [SerializeField]
    Image m_DiceIcon;
    [SerializeField]
    Image m_DiceIcon_L;
    [SerializeField]
    TextMeshProUGUI m_Level;

    public void SetSlot(in FDice InSlot)
    {
        FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InSlot.id);
        if (diceData == null)
            return;

        m_DiceIcon_L.gameObject.SetActive(diceData.Value.Grade == DiceGrade.DICE_GRADE_LEGEND);
        m_DiceIcon.gameObject.SetActive(diceData.Value.Grade != DiceGrade.DICE_GRADE_LEGEND);
        if (m_DiceIcon_L.IsActive())
            m_DiceIcon_L.sprite = Resources.Load<Sprite>(diceData.Value.IconPath);
        else
            m_DiceIcon.sprite = Resources.Load<Sprite>(diceData.Value.IconPath);

        m_Level.text = InSlot.level.ToString();
    }
}
