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

    Dictionary<int, FGoodsSlot> m_GoodsIDMap = new Dictionary<int, FGoodsSlot>();

    public string Title { set { m_TitleText.text = value; } }
    public string Time { set { m_TimeText.text = value; } }
    public Transform GoodsParent { get { return m_GoodsList; } }

    public void AddGoods(int InID, FGoodsSlot InSlot)
    {
        m_GoodsIDMap.Add(InID, InSlot);
    }

    public void ClearGoods()
    {
        foreach (FGoodsSlot slot in m_GoodsIDMap.Values)
        {
            GameObject.Destroy(slot.gameObject);
        }
        m_GoodsIDMap.Clear();
    }

    public void SetDiceSoldOut(int InID)
    {
        if(m_GoodsIDMap.ContainsKey(InID))
        {
            m_GoodsIDMap[InID].SoldOut = true;
        }
    }
}
