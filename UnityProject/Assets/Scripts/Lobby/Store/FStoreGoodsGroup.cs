using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FStoreGoodsGroup : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI titleText;
    [SerializeField]
    Transform goodsList;
    [SerializeField]
    TextMeshProUGUI timeText;

    Dictionary<int, FStoreGoodsSlot> goodsMap = new Dictionary<int, FStoreGoodsSlot>();

    public string Title { set { titleText.text = value; } }
    public string Time { set { timeText.text = value; } }
    public Transform GoodsParent { get { return goodsList; } }

    public void AddGoods(int InID, FStoreGoodsSlot InSlot)
    {
        goodsMap.Add(InID, InSlot);
    }

    public void ClearGoods()
    {
        foreach (FStoreGoodsSlot slot in goodsMap.Values)
        {
            GameObject.Destroy(slot.gameObject);
        }
        goodsMap.Clear();
    }

    public void SetDiceSoldOut(int InID)
    {
        if(goodsMap.ContainsKey(InID))
        {
            goodsMap[InID].SoldOut = true;
        }
    }
}
