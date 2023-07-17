using FEnum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FDiceData
{
    public readonly int id;
    public readonly DiceGrade grade;
    public readonly string name;
    public readonly string description;
    public readonly string iconPath;
    public readonly string notAcquiredIconPath;
    public readonly Color color;
    public readonly List<int> skillIDList = new List<int>();

    public FDiceData(FDataNode InNode)
    {
        id = InNode.GetIntAttr("id");
        grade = (DiceGrade)InNode.GetIntAttr("grade");
        name = InNode.GetStringAttr("name");
        description = InNode.GetStringAttr("description");
        iconPath = InNode.GetStringAttr("icon");
        notAcquiredIconPath = InNode.GetStringAttr("notAcquiredIcon");
        color = InNode.GetColorAttr("color");

        FDataNode skillListNode = InNode.FindChildNode("SkillList");
        if(skillListNode != null)
        {
            skillListNode.ForeachChildNodes("Skill", (in FDataNode InNode) => {
                skillIDList.Add(InNode.GetIntAttr("id"));
            });
        }
    }

    public delegate void ForeachSkillIDDelegate(int InID);
    public void ForeachSkillID(ForeachSkillIDDelegate InFunc)
    {
        foreach(int id in skillIDList)
        {
            InFunc(id);
        }
    }
}

public class FDiceLevelData
{
    public readonly int level;
    public readonly int diceCountCost;
    public readonly int goldCost;

    public FDiceLevelData(FDataNode InNode)
    {
        level = InNode.GetIntAttr("level");
        diceCountCost = InNode.GetIntAttr("diceCountCost");
        goldCost = InNode.GetIntAttr("goldCost");
    }
}

public class FDiceGradeData
{
    public readonly DiceGrade grade;
    public readonly string gradeName;
    public readonly string backgroundPath;
    public readonly int initialLevel;
    public readonly int maxLevel;
    public readonly int critical;

    private Dictionary<int, FDiceLevelData> levelDataMap = new Dictionary<int, FDiceLevelData>();

    public FDiceGradeData(FDataNode InNode)
    {
        grade = (DiceGrade)InNode.GetIntAttr("grade");
        gradeName = InNode.GetStringAttr("name");
        backgroundPath = InNode.GetStringAttr("invenSlotImage");
        initialLevel = InNode.GetIntAttr("initialLevel");
        critical = InNode.GetIntAttr("critical");

        InNode.ForeachChildNodes("Level", (in FDataNode InNode) => {
            FDiceLevelData levelData = new FDiceLevelData(InNode);
            levelDataMap.Add(levelData.level, levelData);
        });

        maxLevel = levelDataMap.Keys.Max();
    }

    public FDiceLevelData FindDiceLevelData(int InLevel)
    {
        if (levelDataMap.ContainsKey(InLevel))
            return levelDataMap[InLevel];

        return null;
    }
}

public class FDiceDataManager : FNonObjectSingleton<FDiceDataManager>
{
    Dictionary<int, FDiceData> diceDataMap = new Dictionary<int, FDiceData>();
    Dictionary<DiceGrade, FDiceGradeData> diceGradeDataMap = new Dictionary<DiceGrade, FDiceGradeData>();

    public void Initialize()
    {
        List<FDataNode> diceDataNodes = FDataCenter.Instance.GetDataNodesWithQuery("DiceList.Dice");
        foreach(FDataNode node in diceDataNodes)
        {
            FDiceData data = new FDiceData(node);
            diceDataMap.Add(data.id, data);
        }

        List<FDataNode> diceGradeDataNodes = FDataCenter.Instance.GetDataNodesWithQuery("DiceGradeList.DiceGrade");
        foreach (FDataNode node in diceGradeDataNodes)
        {
            FDiceGradeData gradeData = new FDiceGradeData(node);
            diceGradeDataMap.Add(gradeData.grade, gradeData);
        }
    }

    public delegate void ForeachDiceDataFunc(in FDiceData InDiceData);
    public void ForeachDiceData(in ForeachDiceDataFunc InFunc)
    {
        foreach (FDiceData data in diceDataMap.Values)
        {
            InFunc(data);
        }
    }

    public FDiceData FindDiceData(int InID)
    {
        if (diceDataMap.ContainsKey(InID))
            return diceDataMap[InID];

        return null;
    }

    public FDiceGradeData FindGradeDataByID(int InID)
    {
        FDiceData diceData = FindDiceData(InID);
        if (diceData != null)
            return FindGradeData(diceData.grade);

        return null;
    }

    public FDiceGradeData FindGradeData(DiceGrade InGrade)
    {
        if(diceGradeDataMap.ContainsKey(InGrade))
            return diceGradeDataMap[InGrade];

        return null;
    }

    public FDiceLevelData FindDiceLevelData(int InID, int InLevel)
    {
        FDiceGradeData diceGradeData = FindGradeDataByID(InID);
        if (diceGradeData == null)
            return null;

        return diceGradeData.FindDiceLevelData(InLevel);
    }
}
