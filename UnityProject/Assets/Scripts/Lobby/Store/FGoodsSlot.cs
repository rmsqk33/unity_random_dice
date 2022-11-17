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
    Image m_Image;

    public string Name { set { m_NameText.text = value; } }
    public int Count { set { m_CountText.text = value <= 1 ? "" : "x" + value; } }
    public int Price { set { m_PriceText.text = value.ToString(); } }
    public Sprite Image { set { m_Image.sprite = value; } }
}
