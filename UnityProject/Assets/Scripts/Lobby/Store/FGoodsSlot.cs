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
    Image m_GoldIcon;
    [SerializeField]
    Image m_DiaIcon;
    [SerializeField]
    Image m_Background;
    [SerializeField]
    Image m_GoodsImage;

    public string Name { set { m_NameText.text = value; } }
    public int Count { set { m_CountText.text = "x" + value; } }
    public Sprite Background { set { m_Background.sprite = value; } }
    public Sprite GoodsImage { set { m_GoodsImage.sprite = value; } }

    public int Gold 
    { 
        set 
        {
            m_GoldIcon.gameObject.SetActive(true);
            m_DiaIcon.gameObject.SetActive(false);
            m_PriceText.text = value.ToString(); 
        }
    }

    public int Dia 
    { 
        set 
        {
            m_GoldIcon.gameObject.SetActive(false);
            m_DiaIcon.gameObject.SetActive(true);
            m_PriceText.text = value.ToString(); 
        }
    }

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
