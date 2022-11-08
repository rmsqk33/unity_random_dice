using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public struct FDiceData
{
    public int ID;
    public int Grade;
    public string Name;
    public string Description;
    public string IconPath;
    public string DisableIconPath;
    public Color Color;
}

public struct FDiceLevelData
{
    public int Level;
    public int MaxExp;
    public int GoldCost;
}

public struct FDiceGradeData
{
    public int Grade;
    public string GradeName;
    public string BackgroundPath;
    public int InitialLevel;
    public int Critical;
    public Dictionary<int, FDiceLevelData> LevelDataMap;
}

public class FDiceDataManager : FNonObjectSingleton<FDiceDataManager>
{
    Dictionary<int, FDiceData> m_DiceDataMap = new Dictionary<int, FDiceData>();
    Dictionary<int, FDiceGradeData> m_DiceGradeDataMap = new Dictionary<int, FDiceGradeData>();

    public void Initialize()
    {
        List<FDataNode> diceDataNodes = FDataCenter.Instance.GetDataNodesWithQuery("DiceList.Dice");
        foreach(FDataNode node in diceDataNodes)
        {
            FDiceData data = new FDiceData();
            data.ID = node.GetIntAttr("id");
            data.Grade = node.GetIntAttr("grade");
            data.Name = node.GetStringAttr("name");
            data.Description = node.GetStringAttr("description");
            data.IconPath = node.GetStringAttr("icon");
            data.DisableIconPath = node.GetStringAttr("disableIcon");
            data.Color = node.GetColorAttr("color");

            m_DiceDataMap.Add(data.ID, data);
        }

        List<FDataNode> diceGradeDataNodes = FDataCenter.Instance.GetDataNodesWithQuery("DiceGradeList.DiceGrade");
        foreach (FDataNode node in diceGradeDataNodes)
        {
            FDiceGradeData gradeData = new FDiceGradeData();
            gradeData.Grade = node.GetIntAttr("grade");
            gradeData.GradeName = node.GetStringAttr("name");
            gradeData.BackgroundPath = node.GetStringAttr("invenSlotImage");
            gradeData.InitialLevel = node.GetIntAttr("initialLevel");
            gradeData.Critical = node.GetIntAttr("critical");
            gradeData.LevelDataMap = new Dictionary<int, FDiceLevelData>();

            node.ForeachChildNodes("Level", (in FDataNode InNode) => {
                FDiceLevelData levelData = new FDiceLevelData();
                levelData.Level = InNode.GetIntAttr("level");
                levelData.MaxExp = InNode.GetIntAttr("exp");
                levelData.GoldCost = InNode.GetIntAttr("goldCost");

                gradeData.LevelDataMap.Add(levelData.Level, levelData);
            });

            m_DiceGradeDataMap.Add(gradeData.Grade, gradeData);
        }
    }

    public delegate void ForeachDiceDataFunc(in FDiceData InDiceData);
    public void ForeachDiceData(in ForeachDiceDataFunc InFunc)
    {
        foreach (FDiceData data in m_DiceDataMap.Values)
        {
            InFunc(data);
        }
    }

    public FDiceData? FindDiceData(int InID)
    {
        if (m_DiceDataMap.ContainsKey(InID))
        {
            return m_DiceDataMap[InID];
        }
        return null;
    }

    public FDiceGradeData? FindGradeDataByID(int InID)
    {
        FDiceData? diceData = FindDiceData(InID);
        if (diceData != null)
            return FindGradeData(diceData.Value.Grade);

        return null;
    }

    public FDiceGradeData? FindGradeData(int InGrade)
    {
        if(m_DiceGradeDataMap.ContainsKey(InGrade))
        {
            return m_DiceGradeDataMap[InGrade];
        }
        return null;
    }
}
