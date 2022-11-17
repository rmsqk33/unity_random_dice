using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class FGoodsGroup : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_TitleText;
    [SerializeField]
    Transform m_GoodsList;
    
    public string Title { set { m_TitleText.text = value; } }
    public void AddGoods(FGoodsSlot InSlot)
    {
        InSlot.transform.SetParent(m_GoodsList);
    }

    public void ClearGoods()
    {
        for(int i = 0; i < m_GoodsList.transform.childCount; ++i)
        {
            GameObject.Destroy(m_GoodsList.transform.GetChild(i).gameObject);
        }
    }
}
