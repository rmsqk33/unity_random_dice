using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FBattleFieldData
{
    public int ID;
    public string Name;
    public string SkinImage;
}

public class FBattleFieldDataManager : FNonObjectSingleton<FBattleFieldDataManager>
{
    Dictionary<int, FBattleFieldData> m_BattleFieldDataMap = new Dictionary<int, FBattleFieldData>();

    public void Initialize()
    {
        List<FDataNode> dataNodeList = FDataCenter.Instance.GetDataNodesWithQuery("BattleFieldList.BattleField");
        foreach (FDataNode node in dataNodeList)
        {
            FBattleFieldData newData = new FBattleFieldData();
            newData.ID = node.GetIntAttr("id");
            newData.Name = node.GetStringAttr("name");
            newData.SkinImage = node.GetStringAttr("skinImage");
            m_BattleFieldDataMap.Add(newData.ID, newData);
        }
    }

    public delegate void ForeachBattleFieldDataFunc(FBattleFieldData InData);
    public void ForeachBattleFieldData(ForeachBattleFieldDataFunc InFunc)
    {
        foreach(FBattleFieldData data in m_BattleFieldDataMap.Values)
        {
            InFunc(data);
        }
    }

    public FBattleFieldData? FindBattleFieldData(int InID)
    {
        if (m_BattleFieldDataMap.ContainsKey(InID))
            return m_BattleFieldDataMap[InID];

        return null;
    }
}
