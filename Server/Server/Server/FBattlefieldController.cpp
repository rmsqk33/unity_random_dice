#include "stdafx.h"
#include "FBattlefieldController.h"
#include "FDatabaseManager.h"
#include "FInventoryController.h"
#include "FBattlefieldDataManager.h"
#include "FServerManager.h"

FBattlefieldController::FBattlefieldController(FPlayer* InOwner)
	: FControllerBase(InOwner)
{
}

FBattlefieldController::~FBattlefieldController()
{
}

void FBattlefieldController::Initialize()
{
	InitAcquiredBattlefield();
	InitBattlefieldPreset();
}

void FBattlefieldController::Handle_C_PURCHASE_BATTLEFIELD(const C_PURCHASE_BATTLEFIELD* InPacket)
{
	int battlefieldID = InPacket->id;

	S_PURCHASE_BATTLEFIELD packet;
	packet.resultType = BATTLEFIELD_PURCHASE_RESULT_SUCCESS;

	FInventoryController* inventoryController = FindController<FInventoryController>();
	const FBattlefieldData* battlefieldData = FBattlefieldDataManager::GetInstance()->FindBattlefieldData(battlefieldID);
	if (battlefieldData == nullptr)
		packet.resultType = BATTLEFIELD_PURCHASE_RESULT_INVALID;
	else if(IsAcquiredBattlefield(battlefieldID))
		packet.resultType = BATTLEFIELD_PURCHASE_RESULT_ALEADY_ACQUIRED;
	else if(inventoryController == nullptr || inventoryController->GetDia() < battlefieldData->price)
		packet.resultType = BATTLEFIELD_PURCHASE_RESULT_NOT_ENOUGH_MONEY;

	if (packet.resultType == BATTLEFIELD_PURCHASE_RESULT_SUCCESS)
	{
		packet.id = battlefieldID;

		inventoryController->DiaModification(-battlefieldData->price);
		m_AcquiredBattlefieldList.push_back(battlefieldID);

		string query = FUtil::SPrintf("INSERT INTO randomdice.acquiredbattlefield(userid, battlefieldid) VALUES(%d, %d)", GetDBID(), battlefieldID);
		FDatabaseManager::GetInstance()->Query(query);
	}

	FServerManager::GetInstance()->Send(GetSocket(), &packet);
}

void FBattlefieldController::InitAcquiredBattlefield()
{
	string query = FUtil::SPrintf("SELECT battlefieldid FROM randomdice.acquiredbattlefield WHERE userid=%d", GetDBID());
	const MYSQL_ROWS* rows = FDatabaseManager::GetInstance()->Query(query);
	while (rows != nullptr)
	{
		int id = FUtil::AtoI(rows->data[0]);
		m_AcquiredBattlefieldList.push_back(id);

		rows = rows->next;
	}
}

void FBattlefieldController::InitBattlefieldPreset()
{
	string query = FUtil::SPrintf("SELECT presetindex, battlefieldid FROM randomdice.battlefieldpreset WHERE userid=%d", GetDBID());
	const MYSQL_ROWS* rows = FDatabaseManager::GetInstance()->Query(query);
	while (rows != nullptr)
	{
		int index = FUtil::AtoI(rows->data[0]);
		m_BattlefieldPreset[index] = FUtil::AtoI(rows->data[1]);

		rows = rows->next;
	}
}

void FBattlefieldController::SetBattlefieldPreset(int InPresetIndex, int InBattlefieldID)
{
	if (InPresetIndex < 0 || PRESET_MAX <= InPresetIndex)
		return;

	if (IsAcquiredBattlefield(InBattlefieldID) == false)
		return;

	m_BattlefieldPreset[InPresetIndex] = InBattlefieldID;

	string query = FUtil::SPrintf("UPDATE randomdice.battlefieldpreset SET battlefieldid=%d WHERE userid=%d AND presetindex=%d", InBattlefieldID, GetDBID(), InPresetIndex);
	FDatabaseManager::GetInstance()->Query(query);
}

void FBattlefieldController::ForeachAcquiredBattlefield(const function<void(int)>& InFunc) const
{
	for (int id : m_AcquiredBattlefieldList)
	{
		InFunc(id);
	}
}

void FBattlefieldController::ForeachBattlefieldPreset(const function<void(int, int)>& InFunc) const
{
	for (int i = 0; i < PRESET_MAX; ++i)
	{
		InFunc(i, m_BattlefieldPreset[i]);
	}
}

bool FBattlefieldController::IsAcquiredBattlefield(int InID) const
{
	return std::find(m_AcquiredBattlefieldList.begin(), m_AcquiredBattlefieldList.end(), InID) != m_AcquiredBattlefieldList.end();
}