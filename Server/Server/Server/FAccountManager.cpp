#include "stdafx.h"
#include "FAccountManager.h"
#include "FDatabaseManager.h"
#include "FServerManager.h"
#include "FObjectManager.h"
#include "FUserDataManager.h"
#include "FUtil.h"
#include "FDiceController.h"
#include "FBattlefieldController.h"
#include "FInventoryController.h"
#include "FPlayer.h"
#include "FStatController.h"
#include "FDiceDataManager.h"

void FAccountManager::RequestGuestLogin(SOCKET InClient, const string& InLoginID)
{
    bool loginResult = true;

    string query = FUtil::SPrintf(
        "SELECT id FROM randomdice.user where loginid=\'%s\'", InLoginID.c_str());

    const MYSQL_ROWS* userIDRow = FDatabaseManager::GetInstance()->Query(query);
    if (userIDRow == nullptr || FUtil::AtoI(userIDRow->data[0]) == 0)
        loginResult = false;

    S_GUEST_LOGIN pkt;
    pkt.result = loginResult;
    FServerManager::GetInstance()->Send(InClient, &pkt);

    if (loginResult == false)
        return;
	
    AddPlayer(InClient, FUtil::AtoI(userIDRow->data[0]));
}

void FAccountManager::CreateGuestAccount(SOCKET InClient)
{
    string loginID;
    if (CreateUserDB(loginID) == false)
        return;

    string query = FUtil::SPrintf("SELECT id FROM randomdice.user WHERE loginid=\'%s\'", loginID.c_str());
    const  MYSQL_ROWS* userIDRow = FDatabaseManager::GetInstance()->Query(query);
    if (userIDRow == nullptr)
        return;

    int userID = FUtil::AtoI(userIDRow->data[0]);
    CreateAcquiredDiceDB(userID);
    CreateDicePresetDB(userID);
    CreateAcquiredBattlefieldDB(userID);
    CreateBattlefieldPresetDB(userID);

    S_CREATE_GUEST_ACCOUNT pkt;
    FUtil::CharToWChar(pkt.id, loginID.c_str());

    FServerManager::GetInstance()->Send(InClient, &pkt);
}

void FAccountManager::AddPlayer(SOCKET InSocket, int InDBID)
{
    const FPlayer* player = FObjectManager::GetInstance()->AddPlayer(InSocket, InDBID);

    S_USER_DATA pkt;

    if (const FStatController* controller = player->FindController<FStatController>())
    {
        pkt.level = controller->GetLevel();
        pkt.exp = controller->GetExp();

        FUtil::CharToWChar(pkt.name, controller->GetName());
    }

    if (const FDiceController* controller = player->FindController<FDiceController>())
    {
        int i = 0;
        controller->ForeachAcquiredDice([&](const FDice& InDice) {
            pkt.diceDataList[i].id = InDice.id;
            pkt.diceDataList[i].count = InDice.count;
            pkt.diceDataList[i].level = InDice.level;
            ++i;
            });

        controller->ForeachDicePreset([&](int InPresetIndex, int InSlotIndex, int InDiceID) {
            pkt.dicePreset[InPresetIndex][InSlotIndex] = InDiceID;
            });
    }

    if (const FBattlefieldController* controller = player->FindController<FBattlefieldController>())
    {
        int i = 0;
        controller->ForeachAcquiredBattlefield([&](int InID) {
            pkt.battleFieldIDList[i++] = InID;
            });

        controller->ForeachBattlefieldPreset([&](int InPresetIndex, int InBattlefieldID) {
            pkt.battleFieldPreset[InPresetIndex] = InBattlefieldID;
            });
    }

    if (const FInventoryController* controller = player->FindController<FInventoryController>())
    {
        pkt.selectedPresetIndex = controller->GetPresetIndex();
        pkt.gold = controller->GetGold();
        pkt.dia = controller->GetDia();
        pkt.card = controller->GetCard();
    }

    FServerManager::GetInstance()->Send(InSocket, &pkt);
}

bool FAccountManager::CreateUserDB(string& OutLoginID)
{
    const MYSQL_ROWS* userCountRow = FDatabaseManager::GetInstance()->Query("SELECT COUNT(*) FROM randomdice.user");
    if (userCountRow == nullptr)
        return false;

    int gold = FUserDataManager::GetInstance()->GetInitGold();
    int dia = FUserDataManager::GetInstance()->GetInitDia();
    int card = FUserDataManager::GetInstance()->GetInitCard();
    string loginID = FUtil::ItoA(FUtil::AtoI(userCountRow->data[0]) + 1);

    string query = FUtil::SPrintf(
        "INSERT INTO randomdice.user(loginid, gold, dia, card)\
         VALUES(\'%s\', %d, %d, %d)", loginID.c_str(), gold, dia, card);

    FDatabaseManager::GetInstance()->Query(query);
    OutLoginID = loginID;

    return true;
}

void FAccountManager::CreateAcquiredDiceDB(int InUserID)
{
    string query = "INSERT INTO randomdice.acquireddice(userid, diceid, level) VALUES";
    FUserDataManager::GetInstance()->ForeachInitDiceList([&](int InDiceID){
        const FDiceGradeData* diceGradeData = FDiceDataManager::GetInstance()->FindDiceGradeDataByID(InDiceID);
        if (diceGradeData != nullptr)
        {
            query += FUtil::SPrintf(" (%d, %d, %d),", InUserID, InDiceID, diceGradeData->initLevel);
        }
    });
    query.pop_back();
    FDatabaseManager::GetInstance()->Query(query);
}

void FAccountManager::CreateDicePresetDB(int InUserID)
{
    string query = "INSERT INTO randomdice.dicepreset(userid, presetindex, slotindex, diceid) VALUES";
    FUserDataManager::GetInstance()->ForeachDicePreset([&](int InPresetIndex, int InSlotIndex, int InDiceID) {
        query += FUtil::SPrintf(" (%d, %d, %d, %d),", InUserID, InPresetIndex, InSlotIndex, InDiceID);
    });
    query.pop_back();
    FDatabaseManager::GetInstance()->Query(query);
}

void FAccountManager::CreateAcquiredBattlefieldDB(int InUserID)
{
    string query = "INSERT INTO randomdice.acquiredbattlefield(userid, battlefieldid) VALUES";
    FUserDataManager::GetInstance()->ForeachInitBattlefieldList([&](int InBattlefieldID){
        query += FUtil::SPrintf(" (%d, %d),", InUserID, InBattlefieldID);
    });
    query.pop_back();
    FDatabaseManager::GetInstance()->Query(query);
}

void FAccountManager::CreateBattlefieldPresetDB(int InUserID)
{
    string query = "INSERT INTO randomdice.battlefieldpreset(userid, presetindex, battlefieldid) VALUES";
    FUserDataManager::GetInstance()->ForeachBattlefieldPreset([&](int InPresetIndex, int InBattlefieldID){
		query += FUtil::SPrintf(" (%d, %d, %d),", InUserID, InPresetIndex, InBattlefieldID);
	});
    query.pop_back();
    FDatabaseManager::GetInstance()->Query(query);
}