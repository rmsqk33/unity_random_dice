#pragma once

struct FWaveData
{
	int wave;
	int card;
	int exp;
};

struct FBattleData
{
	int maxWave;
	vector<FWaveData> waveList;
};

class FBattleDataManager : public FSingleton<FBattleDataManager>
{
private:
	map<int, FBattleData> m_BattleDataMap;

public:
	void Initialize();

	int CalcCardByClearWave(int InBattleID, int InWave) const;
	int CalcExpByClearWave(int InBattleID, int InWave) const;

	const FBattleData* FindBattleData(int InBattleID) const;
};

