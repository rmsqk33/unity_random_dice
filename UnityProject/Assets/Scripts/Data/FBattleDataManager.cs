using FEnum;
using System;
using System.Collections.Generic;

public class FWaveData
{ 
	public readonly int wave;
	public readonly int card;
	List<int> enemySummonList = new List<int>();

	public int SummonCount { get { return enemySummonList.Count; } }

	public FWaveData(FDataNode InWaveNode)
	{
		this.wave = InWaveNode.GetIntAttr("wave");
		this.card = InWaveNode.GetIntAttr("card");

        InWaveNode.ForeachChildNodes("Enemy", (in FDataNode InNode) => {
			enemySummonList.Add(InNode.GetIntAttr("id"));
		});
    }

	public int GetEnemyID(int InSummonIndex)
	{
		if(InSummonIndex < enemySummonList.Count)
			return	enemySummonList[InSummonIndex];

		return 0;
	}
}

public class FBattleData
{
    public readonly int id;
    public readonly int life;
    public readonly float summonInterval;
	public readonly int maxWave;

	Dictionary<int, FWaveData> waveDataMap = new Dictionary<int, FWaveData>();

	public FBattleData(FDataNode InNode)
	{
		id = InNode.GetIntAttr("id");
		life = InNode.GetIntAttr("life");
		summonInterval = InNode.GetFloatAttr("summonInterval");

		int max = 0;
		InNode.ForeachChildNodes("Wave", (in FDataNode InNode) => {
			FWaveData waveData = new FWaveData(InNode);
			waveDataMap.Add(waveData.wave, waveData);
			max = Math.Max(waveData.wave, max);
        });

		maxWave = max;
    }

    public FWaveData FindWaveData(int InWave)
	{
		int wave = Math.Clamp(InWave, 1, waveDataMap.Count);
		if (waveDataMap.ContainsKey(wave))
			return waveDataMap[wave];

		return null;
    }
}

public class FBattleDiceLevelData
{
	public readonly int level;
	public readonly int cost;

    public FBattleDiceLevelData(FDataNode InNode)
	{
        level = InNode.GetIntAttr("level");
        cost = InNode.GetIntAttr("cost");
    }
}

public class FBattleDataManager : FNonObjectSingleton<FBattleDataManager>
{
	Dictionary<int, FBattleData> battleDataMap = new Dictionary<int, FBattleData>();
	Dictionary<int, FBattleDiceLevelData> diceLevelMap = new Dictionary<int, FBattleDiceLevelData>();

    public int InitSP { get; private set; }
	public int InitDiceSummonCost { get; private set; }
	public int DiceSummonCostIncrease { get; private set; }
    public int MaxLevel { get; private set; }
    public int MaxEyeCount { get; private set; }
    public int CoopBattleID { get; private set; }
    public float BattleStartInterval { get; private set; }
    public float WaveEndInterval { get; private set; }
    
    public void Initialize()
    {
		FDataNode battleCommonDataNode = FDataCenter.Instance.GetDataNodeWithQuery("BattleCommonData");
        if (battleCommonDataNode != null)
		{
			InitSP = battleCommonDataNode.GetIntAttr("initSP");
            InitDiceSummonCost = battleCommonDataNode.GetIntAttr("initDiceSummonCost");
            DiceSummonCostIncrease = battleCommonDataNode.GetIntAttr("diceSummonCostIncrease");
            MaxEyeCount = battleCommonDataNode.GetIntAttr("maxEyeCount");
            CoopBattleID = battleCommonDataNode.GetIntAttr("coopBattleID");
            BattleStartInterval = battleCommonDataNode.GetFloatAttr("battleStartInterval");
            WaveEndInterval = battleCommonDataNode.GetFloatAttr("waveEndInterval");
            
            List<FDataNode> diceUpgradeNodes = battleCommonDataNode.GetDataNodesWithQuery("DiceUpgrade.Dice");
			foreach(FDataNode dataNode in diceUpgradeNodes)
			{
                FBattleDiceLevelData levelData = new FBattleDiceLevelData(dataNode);
				diceLevelMap.Add(levelData.level, levelData);
            }

			MaxLevel = diceLevelMap.Count;
        }

		List<FDataNode> battleDataNodeList = FDataCenter.Instance.GetDataNodesWithQuery("BattleData");
		foreach(FDataNode node in battleDataNodeList)
		{
			FBattleData data = new FBattleData(node);
            battleDataMap.Add(data.id, data);
        }
    }

	public FBattleData FindBattleData(int InID)
	{
		if (battleDataMap.ContainsKey(InID))
			return battleDataMap[InID];

		return null;
	}

	public FBattleDiceLevelData FindDiceLevelData(int InLevel)
	{
		if(diceLevelMap.ContainsKey(InLevel))
			return diceLevelMap[InLevel];

		return null;
	}
}
