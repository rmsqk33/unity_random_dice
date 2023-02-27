using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FGoodsSlot : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_NameText;
    [SerializeField]
    TextMeshProUGUI m_CountText;
    [SerializeField]
    TextMeshProUGUI m_PriceText;
    [SerializeField]
    Image m_Background;
    [SerializeField]
    Image m_DiceImage;

    public string Name { set { m_NameText.text = value; } }
    public int Count { set { m_CountText.text = "x" + value; } }
    public int Price { set { m_PriceText.text = value.ToString(); } }
    public Sprite Background { set { m_Background.sprite = value; } }
    public Sprite DiceImage { set { m_DiceImage.sprite = value; } }
    public bool SoldOut 
    {
        set 
        {
            Button button = GetComponent<Button>();
            if(button != null)
            {
                button.animator.SetTrigger(value == true ? "Disabled" : "Normal");
            }
        }
    }
}
