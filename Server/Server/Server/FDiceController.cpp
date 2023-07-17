#include "stdafx.h"
#include "FDiceController.h"
#include "FDiceDataManager.h"
#include "FInventoryController.h"
#include "FServerManager.h"
#include "FDatabaseManager.h"

FDiceController::FDiceController(FPlayer* InOwner)
	: FControllerBase(InOwner)
{
}

void FDiceController::Initialize()
{
	InitAcquiredDice();
	InitDicePreset();
}

void FDiceController::InitAcquiredDice()
{
	string query = FUtil::SPrintf("SELECT diceid, level, count FROM randomdice.acquireddice WHERE userid=%d", GetDBID());
	const MYSQL_ROWS* acquiredDiceRows = FDatabaseManager::GetInstance()->Query(query);

	while (acquiredDiceRows != nullptr)
	{
		FDice dice;
		dice.id = FUtil::AtoI(acquiredDiceRows->data[0]);
		dice.level = FUtil::AtoI(acquiredDiceRows->data[1]);
		dice.count = FUtil::AtoI(acquiredDiceRows->data[2]);

		m_AcquiredDiceMap.insert(pair<int, FDice>(dice.id, dice));

		acquiredDiceRows = acquiredDiceRows->next;
	}
}

void FDiceController::InitDicePreset()
{
	string query = FUtil::SPrintf("SELECT presetindex, slotindex, diceid FROM randomdice.dicepreset WHERE userid=%d", GetDBID());
	const MYSQL_ROWS* dicePresetRows = FDatabaseManager::GetInstance()->Query(query);

	while (dicePresetRows != nullptr)
	{
		int presetIndex = FUtil::AtoI(dicePresetRows->data[0]);
		int slotIndex = FUtil::AtoI(dicePresetRows->data[1]);
		int diceID = FUtil::AtoI(dicePresetRows->data[2]);

		m_DicePreset[presetIndex][slotIndex] = diceID;

		dicePresetRows = dicePresetRows->next;
	}
}

void FDiceController::Release()
{
}

void FDiceController::AddDice(int InID, int InCount)
{
	vector<pair<int, int>> list;
	list.push_back(pair<int, int>(InID, InCount));

	AddDice(list);
}

void FDiceController::AddDice(const vector<pair<int, int>>& InDiceList)
{
	S_ADD_DICE packet;
	packet.diceCount = InDiceList.size();

	for (int i = 0; i < InDiceList.size(); ++i)
	{
		int diceID = InDiceList[i].first;
		int diceCount = InDiceList[i].second;

		auto iter = m_AcquiredDiceMap.find(diceID);
		if (iter == m_AcquiredDiceMap.end())
		{
			FDice dice;
			dice.id = diceID;
			dice.count = diceCount;

			if (const FDiceGradeData* diceGradeData = FDiceDataManager::GetInstance()->FindDiceGradeDataByID(diceID))
			{
				dice.level = diceGradeData->initLevel;
			}

			m_AcquiredDiceMap.insert(pair<int, FDice>(diceID, dice));
		}
		else
		{
			iter->second.count += diceCount;
		}

		packet.diceList[i].id = diceID;
		packet.diceList[i].count = diceCount;
	}

	string query = FUtil::SPrintf("INSERT INTO randomdice.acquireddice(userid, diceid, level, count) VALUES");
	for (auto iter : InDiceList)
	{
		auto acquiredDiceIter = m_AcquiredDiceMap.find(iter.first);
		if (acquiredDiceIter != m_AcquiredDiceMap.end())
		{
			const FDice& dice = acquiredDiceIter->second;
			query += FUtil::SPrintf(" (%d, %d, %d, %d),", GetDBID(), dice.id, dice.level, dice.count);
		}
	}
	query.pop_back();
	query += " ON DUPLICATE KEY UPDATE count=VALUES(count)";
	FDatabaseManager::GetInstance()->Query(query);

	FServerManager::GetInstance()->Send(GetSocket(), &packet);
}

void FDiceController::UpgradeDice(int InID)
{
	S_UPGRADE_DICE packet;
	packet.resultType = CheckDiceUpgradable(InID);

	if (packet.resultType == DICE_UPGRADE_RESULT_SUCCESS)
	{
		FDice& dice = m_AcquiredDiceMap.find(InID)->second;
		if (const FDiceGradeLevelData* diceGradeLevelData = FDiceDataManager::GetInstance()->FindDiceGradeLevelDataByID(InID, dice.level))
		{
			++dice.level;
			dice.count -= diceGradeLevelData->diceCountCost;

			string query = FUtil::SPrintf("UPDATE randomdice.acquireddice SET level=%d, count=%d WHERE userid=%d AND diceid=%d", dice.level, dice.count, GetDBID(), InID);
			FDatabaseManager::GetInstance()->Query(query);

			packet.id = InID;
			packet.level = dice.level;
			packet.count = dice.count;

			if (FInventoryController* inventory = FindController<FInventoryController>())
			{
				inventory->GoldModification(-diceGradeLevelData->goldCost);
			}
		}
	}

	FServerManager::GetInstance()->Send(GetSocket(), &packet);
}

void FDiceController::SetDicePreset(int InPresetIndex, int InSlotIndex, int InDiceID)
{
	if (InPresetIndex < 0 || PRESET_MAX <= InPresetIndex)
		return;

	if (InSlotIndex < 0 || DICE_PRESET_SLOT_MAX <= InSlotIndex)
		return;

	if (m_AcquiredDiceMap.find(InDiceID) == m_AcquiredDiceMap.end())
		return;

	m_DicePreset[InPresetIndex][InSlotIndex] = InDiceID;

	string query = FUtil::SPrintf("UPDATE randomdice.dicepreset SET diceid=%d WHERE userid=%d AND presetindex=%d AND slotindex=%d", InDiceID, GetDBID(), InPresetIndex, InSlotIndex);
	FDatabaseManager::GetInstance()->Query(query);
}

DiceUpgradeResult FDiceController::CheckDiceUpgradable(int InID) const
{
	auto iter = m_AcquiredDiceMap.find(InID);
	if (iter == m_AcquiredDiceMap.end())
		return DICE_UPGRADE_RESULT_INVALID_DICE;
	
	const FDice& dice = iter->second;
	const FDiceGradeData* diceGradeData = FDiceDataManager::GetInstance()->FindDiceGradeDataByID(InID);
	if (diceGradeData == nullptr)
		return DICE_UPGRADE_RESULT_INVALID_DICE;

	const FDiceGradeLevelData* diceGradeLevelData = FDiceDataManager::GetInstance()->FindDiceGradeLevelDataByID(InID, dice.level);
	if (diceGradeLevelData == nullptr)
		return DICE_UPGRADE_RESULT_INVALID_DICE;

	if (dice.level == diceGradeData->maxLevel)
		return DICE_UPGRADE_RESULT_MAX_LEVEL;
	
	if (dice.count < diceGradeLevelData->diceCountCost)
		return DICE_UPGRADE_RESULT_NOT_ENOUGH_DICE;

	if (const FInventoryController* inventory = FindController<FInventoryController>())
	{
		if (inventory->GetGold() < diceGradeLevelData->goldCost)
			return DICE_UPGRADE_RESULT_NOT_ENOUGH_MONEY;
	}

	return DICE_UPGRADE_RESULT_SUCCESS;
}

void FDiceController::ForeachAcquiredDice(const function<void(const FDice&)>& InFunc) const
{
	for (auto& iter : m_AcquiredDiceMap)
	{
		InFunc(iter.second);
	}
}

void FDiceController::ForeachDicePreset(const function<void(int, int, int)>& InFunc) const
{
	for (int i = 0; i < DICE_PRESET_SLOT_MAX; ++i)
	{
		for (int j = 0; j < PRESET_MAX; ++j)
		{
			InFunc(i, j, m_DicePreset[i][j]);
		}
	}
}