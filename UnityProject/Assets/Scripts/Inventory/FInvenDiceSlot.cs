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
    Image DiceEye;
    [SerializeField]
    TextMeshProUGUI LevelText;

    [SerializeField]
    Image ExpGauge;
    [SerializeField]
    TextMeshProUGUI ExpText;
    [SerializeField]
    Image LevelUpIcon;
    
    public int Level
    { 
        set
        {
            LevelText.text = value.ToString();
        }
    }
    
    public int CurrentExp
    {
        set
        {
            UpdateExp();
        }
    }

    public int MaxExp
    {
        get;
    }

    void UpdateExp()
    {
    }
}
