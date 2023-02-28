using RandomDice;
using System.Collections.Generic;

public struct FBoxGoodsData
{
    public DiceGrade grade;
    public int min;
    public int max;
}

public struct FStoreBoxData
{
    public int id;
    public int price;
    public int gold;
    public string name;
    public string boxImagePath;
    public List<FBoxGoodsData> diceList;
}

public class FStoreDataManager : FNonObjectSingleton<FStoreDataManager>
{
    public string BoxStoreTitle;
    Dictionary<int, FStoreBoxData> BoxMap = new Dictionary<int, FStoreBoxData>();
    Dictionary<DiceGrade, string> BoxGoodsImageMap = new Dictionary<DiceGrade, string>();

    public void Initialize()
    {
        FDataNode dataNode = FDataCenter.Instance.GetDataNodeWithQuery("StoreDataList.BoxStoreData");
        if(dataNode != null)
        {
            BoxStoreTitle = dataNode.GetStringAttr("name");
            dataNode.ForeachChildNodes("Card", (in FDataNode InNode) => {
                DiceGrade grade = (DiceGrade)InNode.GetIntAttr("grade");
                string imagePath = InNode.GetStringAttr("image");
                BoxGoodsImageMap.Add(grade, imagePath);
            });

            dataNode.ForeachChildNodes("Box", (in FDataNode InBoxNode) => {
                FStoreBoxData boxData = new FStoreBoxData();
                boxData.id = InBoxNode.GetIntAttr("id");
                boxData.price = InBoxNode.GetIntAttr("price");
                boxData.gold = InBoxNode.GetIntAttr("gold");
                boxData.name = InBoxNode.GetStringAttr("name");
                boxData.boxImagePath = InBoxNode.GetStringAttr("image");
                boxData.diceList = new List<FBoxGoodsData>();

                InBoxNode.ForeachChildNodes("Dice", (in FDataNode InDiceNode) => {
                    FBoxGoodsData diceData = new FBoxGoodsData();
                    diceData.grade = (DiceGrade)InDiceNode.GetIntAttr("grade");
                    diceData.min = InDiceNode.GetIntAttr("min");
                    diceData.max = InDiceNode.GetIntAttr("max");

                    boxData.diceList.Add(diceData);
                });

                BoxMap.Add(boxData.id, boxData);
            });
        }
    }

    public string GetBoxGoodsImage(DiceGrade InGrade)
    {
        if (BoxGoodsImageMap.ContainsKey(InGrade))
            return BoxGoodsImageMap[InGrade];

        return "";
    }

    public FStoreBoxData? FindStoreBoxData(int InID)
    {
        if (BoxMap.ContainsKey(InID))
            return BoxMap[InID];

        return null;
    }

    public delegate void ForeachStoreBoxDataFunc(in FStoreBoxData InData);
    public void ForeachStoreBoxData(ForeachStoreBoxDataFunc InFunc)
    {
        foreach(var iter in BoxMap)
        {
            InFunc(iter.Value);
        }
    }
}
