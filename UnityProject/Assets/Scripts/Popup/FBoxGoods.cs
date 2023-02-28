using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FBoxGoods : MonoBehaviour
{
    [SerializeField]
    Image m_GoodsIcon;
    [SerializeField]
    TextMeshProUGUI m_GoodsName;
    [SerializeField]
    TextMeshProUGUI m_Count;

    public Sprite GoodsIcon { set { m_GoodsIcon.sprite = value; } }
    public string GoodsName { set { m_GoodsName.text = value; } }
    public string Count { set { m_Count.text = value; } }
}
