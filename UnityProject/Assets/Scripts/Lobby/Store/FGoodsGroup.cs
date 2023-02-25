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
    [SerializeField]
    TextMeshProUGUI m_TimeText;

    Dictionary<int, FGoodsSlot> m_GoodsIDMap;

    public string Title { set { m_TitleText.text = value; } }
    public string Time { set { m_TimeText.text = value; } }
    
    public void AddGoods(int InID, FGoodsSlot InSlot)
    {
        m_GoodsIDMap.Add(InID, InSlot);
        InSlot.transform.SetParent(m_GoodsList);
    }

    public void ClearGoods()
    {
        foreach(Transform child in m_GoodsList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void SetDiceSoldOut(int InID)
    {
        if(m_GoodsIDMap.ContainsKey(InID))
        {
            m_GoodsIDMap[InID].SoldOut = true;
        }
    }
}
