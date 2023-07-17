using FEnum;
using System.Collections.Generic;
using UnityEngine;

public class FSkillData
{
    public readonly int id;
    public readonly SkillType skillType;
    public readonly SkillTargetType targetType;
    public readonly int projectileID;
    public readonly int effectID;
    public readonly int loopCount;

    public readonly int abnormalityID;
    public readonly int checkAbnormalityID;
    public readonly float interval;
    public readonly float duration;
    public readonly float pathMinRate;
    public readonly float pathMaxRate;

    public readonly int summonEnemyID;
    public readonly int summonCount;

    public FSkillData(FDataNode InNode)
    {
        id = InNode.GetIntAttr("id");
        skillType = (SkillType)InNode.GetIntAttr("skillType");
        targetType = (SkillTargetType)InNode.GetIntAttr("targetType");
        projectileID = InNode.GetIntAttr("projectileID");
        effectID = InNode.GetIntAttr("effectID");
        loopCount = Mathf.Max(1, InNode.GetIntAttr("loopCount"));
        
        abnormalityID = InNode.GetIntAttr("abnormalityID");
        checkAbnormalityID = InNode.GetIntAttr("checkAbnormalityID");
        
        interval = InNode.GetFloatAttr("interval");
        duration = InNode.GetFloatAttr("duration");
        pathMinRate = InNode.GetFloatAttr("pathMinRate");
        pathMaxRate = InNode.GetFloatAttr("pathMaxRate");
        summonEnemyID = InNode.GetIntAttr("summonEnemyID");
        summonCount = InNode.GetIntAttr("summonCount");
    }
}

public class FSkillDataManager : FNonObjectSingleton<FSkillDataManager>
{
    Dictionary<int, FSkillData> skillDataMap = new Dictionary<int, FSkillData>();

    public void Initialize()
    {
        List<FDataNode> skillDataNodeList =  FDataCenter.Instance.GetDataNodesWithQuery("SkillDataList.SkillData");
        foreach(FDataNode node in skillDataNodeList)
        {
            FSkillData skillData = new FSkillData(node);
            skillDataMap.Add(skillData.id, skillData);
        }
    }

    public FSkillData FindSkillData(int InID)
    {
        if (skillDataMap.ContainsKey(InID))
            return skillDataMap[InID];

        return null;
    }
}
