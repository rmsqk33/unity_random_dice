#include "stdafx.h"
#include "FDiceDataManager.h"

void FDiceDataManager::Initialize()
{
	for (int i = DICE_GRADE_NORMAL; i < DICE_GRADE_MAX; ++i)
	{
		m_DiceIDByGradeMap.insert(pair<DiceGrade, vector<int>>((DiceGrade)i, vector<int>()));
	}

	vector<const FDataNode*> diceNodes = FDataCenter::GetInstance()->FindNodes("DiceList.Dice");
	for (const FDataNode* diceNode : diceNodes)
	{
		FDiceData diceData;
		diceData.id = diceNode->GetIntAttr("id");
		diceData.grade = (DiceGrade)diceNode->GetIntAttr("grade");

		m_DiceMap.insert(pair<int, FDiceData>(diceData.id, diceData));

		auto iter = m_DiceIDByGradeMap.find(diceData.grade);
		if (iter != m_DiceIDByGradeMap.end())
			iter->second.push_back(diceData.id);
	}

	vector<const FDataNode*> gradeNodes = FDataCenter::GetInstance()->FindNodes("DiceGradeList.DiceGrade");
	for (const FDataNode* gradeNode : gradeNodes)
	{
		FDiceGradeData data;
		data.grade = (DiceGrade)gradeNode->GetIntAttr("grade");
		data.initLevel = gradeNode->GetIntAttr("initialLevel");
		data.critical = gradeNode->GetIntAttr("critical");

		m_DiceGradeMap.insert(pair<DiceGrade, FDiceGradeData>(data.grade, data));

		gradeNode->ForeachChildNodes("Level", [&](const FDataNode* InNode) {
			FDiceGradeLevelData gradeLevelData;
			gradeLevelData.level = InNode->GetIntAttr("level");
			gradeLevelData.diceCountCost = InNode->GetIntAttr("diceCountCost");
			gradeLevelData.goldCost = InNode->GetIntAttr("goldCost");

			int id = ConvertDiceGradeLevelToID(data.grade, gradeLevelData.level);
			m_DiceGradeLevelMap.insert(pair<int, FDiceGradeLevelData>(id, gradeLevelData));
			});
	}
}

void FDiceDataManager::Release()
{
}

const FDiceData* FDiceDataManager::FindDiceData(int InID) const
{
	const auto& iter = m_DiceMap.find(InID);
	if (iter != m_DiceMap.end())
		return &iter->second;

	return nullptr;
}

const FDiceGradeData* FDiceDataManager::FindDiceGradeData(DiceGrade InGrade) const
{
	const auto& iter = m_DiceGradeMap.find(InGrade);
	if (iter != m_DiceGradeMap.end())
		return &iter->second;

	return nullptr;
}

const FDiceGradeData* FDiceDataManager::FindDiceGradeDataByID(int InID) const
{	
	if (const FDiceData* diceData = FindDiceData(InID))
	{
		const auto& iter = m_DiceGradeMap.find(diceData->grade);
		if (iter != m_DiceGradeMap.end())
			return &iter->second;
	}

	return nullptr;
}

const FDiceGradeLevelData* FDiceDataManager::FindDiceGradeLevelData(DiceGrade InGrade, int InLevel) const
{
	auto iter = m_DiceGradeLevelMap.find(ConvertDiceGradeLevelToID(InGrade, InLevel));
	if (iter != m_DiceGradeLevelMap.end())
		return &iter->second;

	return nullptr;
}

const FDiceGradeLevelData* FDiceDataManager::FindDiceGradeLevelDataByID(int InID, int InLevel) const
{
	if (const FDiceGradeData* gradeData = FindDiceGradeDataByID(InID))
	{
		return FindDiceGradeLevelData(gradeData->grade, InLevel);
	}

	return nullptr;
}

int FDiceDataManager::GetDiceCountByGrade(DiceGrade InGrade) const
{
	auto iter = m_DiceIDByGradeMap.find(InGrade);
	if (iter != m_DiceIDByGradeMap.end())
		return iter->second.size();

	return 0;
}

int FDiceDataManager::GetDiceIDByGrandAndIndex(DiceGrade InGrade, int InIndex) const
{
	auto iter = m_DiceIDByGradeMap.find(InGrade);
	if (iter == m_DiceIDByGradeMap.end())
		return 0;

	if (InIndex < 0 || iter->second.size() <= InIndex)
		return 0;

	return iter->second[InIndex];
}

int FDiceDataManager::ConvertDiceGradeLevelToID(DiceGrade InGrade, int InLevel) const
{
	return (InGrade << 8) + InLevel;
}