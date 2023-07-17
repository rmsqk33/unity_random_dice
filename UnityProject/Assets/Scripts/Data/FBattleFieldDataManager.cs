using System.Collections.Generic;

public class FBattleFieldData
{
    public readonly int id;
    public readonly int price;
    public readonly string name;
    public readonly string skinImagePath;
    public readonly string battlefieldPrefab;
    
    public FBattleFieldData(FDataNode InNode)
    {
        id = InNode.GetIntAttr("id");
        price = InNode.GetIntAttr("price");
        name = InNode.GetStringAttr("name");
        skinImagePath = InNode.GetStringAttr("skinImage");
        battlefieldPrefab = InNode.GetStringAttr("battlefieldPrefab");
    }
}

public class FBattleFieldDataManager : FNonObjectSingleton<FBattleFieldDataManager>
{
    Dictionary<int, FBattleFieldData> battleFieldDataMap = new Dictionary<int, FBattleFieldData>();

    public void Initialize()
    {
        List<FDataNode> dataNodeList = FDataCenter.Instance.GetDataNodesWithQuery("BattleFieldList.BattleField");
        foreach (FDataNode node in dataNodeList)
        {
            FBattleFieldData newData = new FBattleFieldData(node);
            battleFieldDataMap.Add(newData.id, newData);
        }
    }

    public delegate void ForeachBattleFieldDataFunc(FBattleFieldData InData);
    public void ForeachBattleFieldData(ForeachBattleFieldDataFunc InFunc)
    {
        foreach(FBattleFieldData data in battleFieldDataMap.Values)
        {
            InFunc(data);
        }
    }

    public FBattleFieldData FindBattleFieldData(int InID)
    {
        if (battleFieldDataMap.ContainsKey(InID))
            return battleFieldDataMap[InID];

        return null;
    }
}
