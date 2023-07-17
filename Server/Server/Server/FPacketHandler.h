#pragma once

#include "FServerManager.h"
#include "FDatabaseManager.h"
#include "FAccountManager.h"
#include "FObjectManager.h"
#include "FPlayer.h"
#include "FUtil.h"
#include "FInventoryController.h"
#include "FDiceController.h"
#include "FBattlefieldController.h"
#include "FStoreController.h"
#include "FStatController.h"
#include "FBattleDataManager.h"
#include "FMatchingManager.h"

#define REGIST_PACKET_HANDLER(ENUM, PKT)\
	void Handler_##PKT(SOCKET InClient, const char* InBuffer);\
	class PKT##_TempClass\
	{\
	public:\
	PKT##_TempClass()\
		{\
			FServerManager::GetInstance()->AddPacketHandler(PacketType::ENUM, Handler_##PKT);\
		}\
	};\
	PKT##_TempClass PKT##Var;\
	void Handler_##PKT(SOCKET InClient, const char* InBuffer)

REGIST_PACKET_HANDLER(PACKET_TYPE_C_GUEST_LOGIN, C_GUEST_LOGIN)
{
	C_GUEST_LOGIN pkt(InBuffer);

	string loginID;
	FUtil::WCharToChar(loginID, pkt.id);
	FAccountManager::GetInstance()->RequestGuestLogin(InClient, loginID);
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_CREATE_GUEST_ACCOUNT, C_ADD_GUEST_ACCOUNT)
{
	FAccountManager::GetInstance()->CreateGuestAccount(InClient);
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_PURCHASE_DICE, C_PURCHASE_DICE)
{
	C_PURCHASE_DICE pkt(InBuffer);

	if (FPlayer* player = FObjectManager::GetInstance()->FindPlayer(InClient))
	{
		if (FStoreController* controller = player->FindController<FStoreController>())
		{
			controller->Handle_C_PURCHASE_DICE(&pkt);
		}
	}
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_PURCHASE_BOX, C_PURCHASE_BOX)
{
	C_PURCHASE_BOX pkt(InBuffer);

	if (FPlayer* player = FObjectManager::GetInstance()->FindPlayer(InClient))
	{
		if (FStoreController* controller = player->FindController<FStoreController>())
		{
			controller->Handle_C_PURCHASE_BOX(&pkt);
		}
	}
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_PURCHASE_BATTLEFIELD, C_PURCHASE_BATTLEFIELD)
{
	C_PURCHASE_BATTLEFIELD pkt(InBuffer);

	if (FPlayer* player = FObjectManager::GetInstance()->FindPlayer(InClient))
	{
		if (FBattlefieldController* controller = player->FindController<FBattlefieldController>())
		{
			controller->Handle_C_PURCHASE_BATTLEFIELD(&pkt);
		}
	}
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_UPGRADE_DICE, C_UPGRADE_DICE)
{
	C_UPGRADE_DICE pkt(InBuffer);

	if (FPlayer* player = FObjectManager::GetInstance()->FindPlayer(InClient))
	{
		if (FDiceController* controller = player->FindController<FDiceController>())
		{
			controller->UpgradeDice(pkt.id);
		}
	}
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_CHANGE_PRESET, C_CHANGE_PRESET)
{
	C_CHANGE_PRESET pkt(InBuffer);

	if (FPlayer* player = FObjectManager::GetInstance()->FindPlayer(InClient))
	{
		if (FInventoryController* controller = player->FindController<FInventoryController>())
		{
			controller->SetPresetIndex(pkt.presetIndex);
		}
	}
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_CHANGE_PRESET_DICE, C_CHANGE_PRESET_DICE)
{
	C_CHANGE_PRESET_DICE pkt(InBuffer);

	if (FPlayer* player = FObjectManager::GetInstance()->FindPlayer(InClient))
	{
		if (FDiceController* controller = player->FindController<FDiceController>())
		{
			controller->SetDicePreset(pkt.presetIndex, pkt.slotIndex, pkt.diceId);
		}
	}
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_CHANGE_PRESET_BATTLEFIELD, C_CHANGE_PRESET_BATTLEFIELD)
{
	C_CHANGE_PRESET_BATTLEFIELD pkt(InBuffer);

	if (FPlayer* player = FObjectManager::GetInstance()->FindPlayer(InClient))
	{
		if (FBattlefieldController* controller = player->FindController<FBattlefieldController>())
		{
			controller->SetBattlefieldPreset(pkt.presetIndex, pkt.battlefieldId);
		}
	}
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_CHANGE_NAME, C_CHANGE_NAME)
{
	C_CHANGE_NAME pkt(InBuffer);

	if (FPlayer* player = FObjectManager::GetInstance()->FindPlayer(InClient))
	{
		if (FStatController* controller = player->FindController<FStatController>())
		{
			controller->Handle_C_CHANGE_NAME(&pkt);
		}
	}
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_BATTLE_RESULT, C_BATTLE_RESULT)
{
	C_BATTLE_RESULT pkt(InBuffer);
	if (pkt.clearWave == 0)
		return;

	if (FPlayer* player = FObjectManager::GetInstance()->FindPlayer(InClient))
	{
		if (FInventoryController* inventory = player->FindController<FInventoryController>())
		{
			int card = FBattleDataManager::GetInstance()->CalcCardByClearWave(pkt.battleId, pkt.clearWave);
			inventory->CardModification(card);
		}

		if (FStatController* stat = player->FindController<FStatController>())
		{
			int exp = FBattleDataManager::GetInstance()->CalcExpByClearWave(pkt.battleId, pkt.clearWave);
			stat->ExpModification(exp);
		}
	}
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_BATTLE_MATCHING, C_BATTLE_MATCHING)
{
	C_BATTLE_MATCHING pkt(InBuffer);

	if (const FPlayer* player = FObjectManager::GetInstance()->FindPlayer(InClient))
	{
		FMatchingManager::GetInstance()->AddUser(player);
	}
}

REGIST_PACKET_HANDLER(PACKET_TYPE_C_BATTLE_MATCHING_CANCEL, C_BATTLE_MATCHING_CANCEL)
{
	C_BATTLE_MATCHING_CANCEL pkt(InBuffer);

	if (const FPlayer* player = FObjectManager::GetInstance()->FindPlayer(InClient))
	{
		FMatchingManager::GetInstance()->RemoveUser(player->GetClientSocket());
	}
}