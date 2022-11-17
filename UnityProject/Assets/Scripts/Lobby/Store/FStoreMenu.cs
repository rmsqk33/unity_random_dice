using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FStoreMenu : FLobbyScrollMenuBase
{
    [SerializeField]
    FGoodsGroup m_DiceList;
    [SerializeField]
    FGoodsGroup m_BoxList;
    [SerializeField]
    FGoodsSlot m_GoodsSlotPrefab;

    private void Start()
    {
        InitStore();
    }

    public void InitStore()
    {
        InitBoxList();
        UpdateDiceGoodsList();
    }

    public void UpdateDiceGoodsList()
    {
        m_DiceList.ClearGoods();
        FStoreController.Instance.ForeachDiceGoodsList((in FDiceGoods InGoods) => {
            FDiceData? diceData = FDiceDataManager.Instance.FindDiceData(InGoods.id);
            if (diceData != null)
            {
                FGoodsSlot slot = GameObject.Instantiate<FGoodsSlot>(m_GoodsSlotPrefab);
                slot.Name = diceData.Value.Name;
                slot.Price = InGoods.price;
                slot.Count = InGoods.count;
                slot.Image = Resources.Load<Sprite>(diceData.Value.IconPath);

                int diceID = InGoods.id;
                slot.GetComponent<Button>().onClick.AddListener(() => { OnClickDice(diceID); });
                
                m_DiceList.AddGoods(slot);
            }
        });
    }

    public override void OnDeactive()
    {
        base.OnDeactive();
        FPopupManager.Instance.ClosePopup();
    }

    private void InitBoxList()
    {
        FDataNode dataNode = FDataCenter.Instance.GetDataNodeWithQuery("StoreDataList.BoxStoreData");
        m_BoxList.Title = dataNode.GetStringAttr("name");

        dataNode.ForeachChildNodes("Box", (in FDataNode InNode) => {
            FGoodsSlot slot = GameObject.Instantiate<FGoodsSlot>(m_GoodsSlotPrefab);
            slot.Name = dataNode.GetStringAttr("name");
            slot.Price = InNode.GetIntAttr("price");
            slot.Image = Resources.Load<Sprite>(InNode.GetStringAttr("image"));
            slot.Count = 1;

            int boxID = InNode.GetIntAttr("id");
            slot.GetComponent<Button>().onClick.AddListener(() => { OnClickBox(boxID); });

            m_BoxList.AddGoods(slot);
        });
    }

    private void OnClickDice(int InID)
    {
        FDiceGoods? goods = FStoreController.Instance.FindDiceGoods(InID);
        if (goods != null)
        {
            FPopupManager.Instance.OpenDicePurchasePopup(InID, goods.Value.count, goods.Value.price);
        }
    }

    private void OnClickBox(int InID)
    {

    }
}
