using RandomDice;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public struct FDiceData
{
    public int ID;
    public DiceGrade Grade;
    public string Name;
    public string Description;
    public string IconPath;
    public string notAcquiredIconPath;
    public Color Color;
}

public struct FDiceLevelData
{
    public int Level;
    public int DiceCountCost;
    public int GoldCost;
}

public struct FDiceGradeData
{
    public DiceGrade Grade;
    public string GradeName;
    public string BackgroundPath;
    public int InitialLevel;
    public int MaxLevel;
    public int Critical;
    public Dictionary<int, FDiceLevelData> LevelDataMap;
}

public class FDiceDataManager : FNonObjectSingleton<FDiceDataManager>
{
    Dictionary<int, FDiceData> m_DiceDataMap = new Dictionary<int, FDiceData>();
    Dictionary<DiceGrade, FDiceGradeData> m_DiceGradeDataMap = new Dictionary<DiceGrade, FDiceGradeData>();

    public void Initialize()
    {
        List<FDataNode> diceDataNodes = FDataCenter.Instance.GetDataNodesWithQuery("DiceList.Dice");
        foreach(FDataNode node in diceDataNodes)
        {
            FDiceData data = new FDiceData();
            data.ID = node.GetIntAttr("id");
            data.Grade = (DiceGrade)node.GetIntAttr("grade");
            data.Name = node.GetStringAttr("name");
            data.Description = node.GetStringAttr("description");
            data.IconPath = node.GetStringAttr("icon");
            data.notAcquiredIconPath = node.GetStringAttr("notAcquiredIcon");
            data.Color = node.GetColorAttr("color");

            m_DiceDataMap.Add(data.ID, data);
        }

        List<FDataNode> diceGradeDataNodes = FDataCenter.Instance.GetDataNodesWithQuery("DiceGradeList.DiceGrade");
        foreach (FDataNode node in diceGradeDataNodes)
        {
            FDiceGradeData gradeData = new FDiceGradeData();
            gradeData.Grade = (DiceGrade)node.GetIntAttr("grade");
            gradeData.GradeName = node.GetStringAttr("name");
            gradeData.BackgroundPath = node.GetStringAttr("invenSlotImage");
            gradeData.InitialLevel = node.GetIntAttr("initialLevel");
            gradeData.Critical = node.GetIntAttr("critical");
            gradeData.LevelDataMap = new Dictionary<int, FDiceLevelData>();

            node.ForeachChildNodes("Level", (in FDataNode InNode) => {
                FDiceLevelData levelData = new FDiceLevelData();
                levelData.Level = InNode.GetIntAttr("level");
                levelData.DiceCountCost = InNode.GetIntAttr("diceCountCost");
                levelData.GoldCost = InNode.GetIntAttr("goldCost");

                gradeData.LevelDataMap.Add(levelData.Level, levelData);
            });

            gradeData.MaxLevel = gradeData.LevelDataMap.Keys.Max();

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

    public FDiceGradeData? FindGradeData(DiceGrade InGrade)
    {
        if(m_DiceGradeDataMap.ContainsKey(InGrade))
        {
            return m_DiceGradeDataMap[InGrade];
        }
        return null;
    }

    public FDiceLevelData? FindDiceLevelData(int InID, int InLevel)
    {
        FDiceGradeData? diceGradeData = FindGradeDataByID(InID);
        if (diceGradeData == null)
            return null;

        if (diceGradeData.Value.LevelDataMap.ContainsKey(InLevel) == false)
            return null;

        return diceGradeData.Value.LevelDataMap[InLevel];
    }
}
