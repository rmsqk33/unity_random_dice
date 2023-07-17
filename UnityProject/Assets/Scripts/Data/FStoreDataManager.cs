using FEnum;
using System.Collections.Generic;

public class FStoreData
{
    public readonly int id;
    public readonly string name;
    List<int> boxIDList = new List<int>();

    public FStoreData(FDataNode InNode)
    {
        id = InNode.GetIntAttr("id");
        name = InNode.GetStringAttr("name");
        InNode.ForeachChildNodes("Box", (in FDataNode InNode) => {
            boxIDList.Add(InNode.GetIntAttr("id"));
        });
    }

    public delegate void ForeachBoxDelegate(in FStoreBoxData InData);
    public void ForeachBoxData(ForeachBoxDelegate InFunc)
    {
        foreach(int id in boxIDList)
        {
            FStoreBoxData boxData = FStoreDataManager.Instance.FindStoreBoxData(id);
            if(boxData != null)
            {
                InFunc(boxData);
            }
        }
    }
}

public class FStoreBoxData
{
    public readonly int id;
    public readonly int goldPrice;
    public readonly int diaPrice;
    public readonly int cardPrice;
    public readonly StorePriceType priceType;

    public readonly int gold;
    public readonly string name;
    public readonly string boxImagePath;
    private List<FBoxGoodsData> goodsList = new List<FBoxGoodsData>();

    public FStoreBoxData(FDataNode InNode)
    {
        id = InNode.GetIntAttr("id");
        goldPrice = InNode.GetIntAttr("goldPrice");
        diaPrice = InNode.GetIntAttr("diaPrice");
        cardPrice = InNode.GetIntAttr("cardPrice");

        if (goldPrice != 0) priceType = StorePriceType.Gold;
        else if (diaPrice != 0) priceType = StorePriceType.Dia;
        else if (cardPrice != 0) priceType = StorePriceType.Card;

        gold = InNode.GetIntAttr("gold");
        name = InNode.GetStringAttr("name");
        boxImagePath = InNode.GetStringAttr("image");
        
        InNode.ForeachChildNodes("Dice", (in FDataNode InDiceNode) => {
            DiceGrade grade = (DiceGrade)InDiceNode.GetIntAttr("grade");
            int min = InDiceNode.GetIntAttr("min");
            int max = InDiceNode.GetIntAttr("max");

            FBoxGoodsData diceData = new FBoxGoodsData(grade, min, max);
            goodsList.Add(diceData);
        });
    }

    public delegate void ForeachGoodsDataDelegate(FBoxGoodsData InData);
    public void ForeachGoodsData(ForeachGoodsDataDelegate InFunc)
    {
        foreach (FBoxGoodsData data in goodsList)
        {
            InFunc(data);
        }
    }
}

public class FBoxGoodsData
{
    public readonly DiceGrade grade;
    public readonly int min;
    public readonly int max;

    public FBoxGoodsData(DiceGrade grade, int min, int max)
    {
        this.grade = grade;
        this.min = min;
        this.max = max;
    }
}

public class FBoxGoodsImageData
{
    public readonly DiceGrade grade;
    public readonly string image;
    public readonly string prefab;

    public FBoxGoodsImageData(FDataNode InNode)
    {
        grade = (DiceGrade)InNode.GetIntAttr("grade");
        image = InNode.GetStringAttr("image");
        prefab = InNode.GetStringAttr("prefab");
    }
}

public class FStoreDataManager : FNonObjectSingleton<FStoreDataManager>
{
    Dictionary<int, FStoreData> storeDataMap = new Dictionary<int, FStoreData>();
    Dictionary<int, FStoreBoxData> boxDataMap = new Dictionary<int, FStoreBoxData>();
    Dictionary<DiceGrade, FBoxGoodsImageData> boxGoodsImageMap = new Dictionary<DiceGrade, FBoxGoodsImageData>();

    int battleCardBoxID;
    
    public int BattleCardBoxID { get { return battleCardBoxID; } }

    public void Initialize()
    {
        FDataNode storeDataNode = FDataCenter.Instance.GetDataNodeWithQuery("StoreDataList");
        if (storeDataNode != null)
        {
            battleCardBoxID = storeDataNode.GetIntAttr("battleCardBoxID");

            List<FDataNode> storeNodeList = storeDataNode.GetDataNodesWithQuery("StoreData");
            foreach(FDataNode node in storeNodeList)
            {
                FStoreData storeData = new FStoreData(node);
                storeDataMap.Add(storeData.id, storeData);
            }

            List<FDataNode> boxNodeList = storeDataNode.GetDataNodesWithQuery("BoxList.Box");
            foreach (FDataNode node in boxNodeList)
            {
                FStoreBoxData boxData = new FStoreBoxData(node);
                boxDataMap.Add(boxData.id, boxData);
            }

            List<FDataNode> cardNodeList = storeDataNode.GetDataNodesWithQuery("CardList.Card");
            foreach (FDataNode node in cardNodeList)
            {
                FBoxGoodsImageData imageData = new FBoxGoodsImageData(node);
                boxGoodsImageMap.Add(imageData.grade, imageData);
            }
        }
    }

    public FBoxGoodsImageData GetBoxGoodsImageData(DiceGrade InGrade)
    {
        if (boxGoodsImageMap.ContainsKey(InGrade))
            return boxGoodsImageMap[InGrade];

        return null;
    }

    public FStoreBoxData FindStoreBoxData(int InID)
    {
        if (boxDataMap.ContainsKey(InID))
            return boxDataMap[InID];

        return null;
    }

    public delegate void ForeachStoreDataFunc(in FStoreData InData);
    public void ForeachStoreData(ForeachStoreDataFunc InFunc)
    {
        foreach(var iter in storeDataMap)
        {
            InFunc(iter.Value);
        }
    }
}
