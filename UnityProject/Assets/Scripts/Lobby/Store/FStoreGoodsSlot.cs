using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FStoreGoodsSlot : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI countText;
    [SerializeField]
    TextMeshProUGUI priceText;
    [SerializeField]
    Image goldIcon;
    [SerializeField]
    Image diaIcon;
    [SerializeField]
    Image background;
    [SerializeField]
    Image goodsImage;

    public string Name { set { nameText.text = value; } }
    public int Count { set { countText.text = "x" + value; } }
    public Sprite Background { set { background.sprite = value; } }
    public Sprite GoodsImage { set { goodsImage.sprite = value; } }

    public int Gold 
    { 
        set 
        {
            goldIcon.gameObject.SetActive(true);
            diaIcon.gameObject.SetActive(false);
            priceText.text = value.ToString(); 
        }
    }

    public int Dia 
    { 
        set 
        {
            goldIcon.gameObject.SetActive(false);
            diaIcon.gameObject.SetActive(true);
            priceText.text = value.ToString(); 
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
