using FEnum;
using System.Collections.Generic;
using UnityEngine;

public class FAbilityData
{
    public readonly AbilityType type;
    public readonly string title;
    public readonly string icon;

    public FAbilityData(FDataNode InNode)
    {
        type = (AbilityType)InNode.GetIntAttr("type");
        title = InNode.GetStringAttr("title");
        icon = InNode.GetStringAttr("icon");
    }
}

public class FAbilityDataManager : FNonObjectSingleton<FAbilityDataManager>
{
    Dictionary<AbilityType, FAbilityData> abilityDataMap = new Dictionary<AbilityType, FAbilityData>();
    Dictionary<SkillTargetType, string> targetTypeStringMap = new Dictionary<SkillTargetType, string>();

    public void Initialize()
    {
        List<FDataNode> abilityNodeList =  FDataCenter.Instance.GetDataNodesWithQuery("AbilityDataList.Ability");
        foreach(FDataNode node in abilityNodeList)
        {
            FAbilityData data = new FAbilityData(node);
            abilityDataMap.Add(data.type, data);
        }

        List<FDataNode> targetTypeStringNodeList = FDataCenter.Instance.GetDataNodesWithQuery("AbilityDataList.TargetTypeString");
        foreach(FDataNode node in targetTypeStringNodeList)
        {
            SkillTargetType type = (SkillTargetType)node.GetIntAttr("type");
            string str = node.GetStringAttr("name");
            targetTypeStringMap.Add(type, str);
        }
    }

    public string GetAbilityTitle(AbilityType InType)
    {
        if (abilityDataMap.ContainsKey(InType))
            return abilityDataMap[InType].title;

        return null;
    }

    public string GetAbilityIcon(AbilityType InType)
    {
        if (abilityDataMap.ContainsKey(InType))
            return abilityDataMap[InType].icon;

        return null;
    }

    public string GetTargetTypeString(SkillTargetType InType)
    {
        if (targetTypeStringMap.ContainsKey(InType))
            return targetTypeStringMap[InType];

        return null;
    }
}
