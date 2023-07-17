#pragma once

#include "FSingleton.h"

struct FDiceData
{
	int id;
	DiceGrade grade;
};

struct FDiceGradeData
{
	DiceGrade grade;
	int initLevel;
	int maxLevel;
	int critical;
};

struct FDiceGradeLevelData
{
	int level;
	int diceCountCost;
	int goldCost;
};

class FDiceDataManager : public FSingleton<FDiceDataManager>
{
private:
	map<int, FDiceData> m_DiceMap;
	map<DiceGrade, vector<int>> m_DiceIDByGradeMap;
	map<DiceGrade, FDiceGradeData> m_DiceGradeMap;
	map<int, FDiceGradeLevelData> m_DiceGradeLevelMap;

public:
	void Initialize();
	void Release() override;

	const FDiceData* FindDiceData(int InID) const;
	const FDiceGradeData* FindDiceGradeData(DiceGrade InGrade) const;
	const FDiceGradeData* FindDiceGradeDataByID(int InID) const;
	const FDiceGradeLevelData* FindDiceGradeLevelData(DiceGrade InGrade, int InLevel) const;
	const FDiceGradeLevelData* FindDiceGradeLevelDataByID(int InID, int InLevel) const;

	int GetDiceCountByGrade(DiceGrade InGrade) const;
	int GetDiceIDByGrandAndIndex(DiceGrade InGrade, int InIndex) const;

private:
	int ConvertDiceGradeLevelToID(DiceGrade InGrade, int InLevel) const;
};

