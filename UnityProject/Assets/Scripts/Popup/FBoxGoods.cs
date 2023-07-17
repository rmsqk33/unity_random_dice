using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FBoxGoods : MonoBehaviour
{
    [SerializeField]
    Image goodsIcon;
    [SerializeField]
    TextMeshProUGUI goodsName;
    [SerializeField]
    TextMeshProUGUI count;

    public Sprite GoodsIcon { set { goodsIcon.sprite = value; } }
    public string GoodsName { set { goodsName.text = value; } }
    public string Count { set { count.text = value; } }

    public void AddGoodsIconPrefab(Transform InIcon)
    {
        InIcon.SetParent(transform, false);

        goodsIcon.gameObject.SetActive(false);
    }
}
