using FEnum;
using System.Collections.Generic;

public class FAbnormalityOverlapData
{
    public readonly int overlap;
    public readonly float effectPercentage;
    public readonly string effectPrefab;

    public FAbnormalityOverlapData(FDataNode InNode)
    {
        overlap = InNode.GetIntAttr("overlap");
        effectPercentage = InNode.GetFloatAttr("effectPercentage");
        effectPrefab = InNode.GetStringAttr("effectPrefab");
    }
}

public class FAbnormalityData
{
    public readonly int id;
    public readonly int maxOverlap;
    public readonly float value;
    public readonly float valuePerLevel;
    public readonly float valuePerBattleLevel;
    public readonly float interval;
    public readonly float duration;
    public readonly bool nonOverlap;
    public readonly StatType statType;
    public readonly AbnormalityType type;
    public readonly AbilityType abilityType;

    Dictionary<int, FAbnormalityOverlapData> overlapMap = new Dictionary<int, FAbnormalityOverlapData>();

    public FAbnormalityData(FDataNode InNode)
    {
        id = InNode.GetIntAttr("id");
        maxOverlap = InNode.GetIntAttr("maxOverlap");
        value = InNode.GetFloatAttr("value");
        valuePerLevel = InNode.GetFloatAttr("valuePerLevel");
        valuePerBattleLevel = InNode.GetFloatAttr("valuePerBattleLevel");

        interval = InNode.GetFloatAttr("interval");
        duration = InNode.GetFloatAttr("duration");

        nonOverlap = InNode.GetBoolAttr("nonOverlap");

        statType = (StatType)InNode.GetIntAttr("statType");
        type = (AbnormalityType)InNode.GetIntAttr("type");
        abilityType = (AbilityType)InNode.GetIntAttr("abilityType");
        
        InNode.ForeachChildNodes("Overlap", (in FDataNode InNode) => {
            FAbnormalityOverlapData overlapData = new FAbnormalityOverlapData(InNode);
            overlapMap.Add(overlapData.overlap, overlapData);
        });
    }

    public FAbnormalityOverlapData FindOverlapData(int InOverlap)
    {
        if (overlapMap.ContainsKey(InOverlap))
            return overlapMap[InOverlap];

        return null;
    }

}

public class FAbnormalityDataManager : FNonObjectSingleton<FAbnormalityDataManager>
{
    Dictionary<int, FAbnormalityData> abnormalityDataMap = new Dictionary<int, FAbnormalityData>();

    public void Initialize()
    {
        List<FDataNode> abnormalityDataList = FDataCenter.Instance.GetDataNodesWithQuery("AbnormalityList.Abnormality");
        foreach(FDataNode node in abnormalityDataList)
        {
            FAbnormalityData abnormalityData = new FAbnormalityData(node);
            abnormalityDataMap.Add(abnormalityData.id, abnormalityData);
        }
    }

    public FAbnormalityData FindAbnormalityData(int InID)
    {
        if (abnormalityDataMap.ContainsKey(InID))
            return abnormalityDataMap[InID];

        return null;
    }

    public FAbnormalityOverlapData FindAbnormalityOverlapData(int InID, int InOverlap)
    {
        FAbnormalityData data = FindAbnormalityData(InID);
        if (data != null)
            return data.FindOverlapData(InOverlap);

        return null;
    }
}
