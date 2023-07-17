#include "stdafx.h"
#include "FStoreController.h"
#include "FStoreDataManager.h"
#include "FDiceDataManager.h"
#include "FDatabaseManager.h"
#include "FServerManager.h"
#include "FDiceController.h"
#include "FInventoryController.h"

FStoreController::FStoreController(FPlayer* InOwner)
	: FControllerBase(InOwner)
{
}

FStoreController::~FStoreController()
{

}

void FStoreController::Initialize()
{
	m_ResetTimeSec = FStoreDataManager::GetInstance()->GetDiceStoreResetTimeSec();

	string query = FUtil::SPrintf("SELECT storeupdatetime FROM randomdice.user WHERE id=%d", GetDBID());
	if (const MYSQL_ROWS* rows = FDatabaseManager::GetInstance()->Query(query))
		m_UpdateTimeSec = FUtil::AtoI64(rows->data[0]);

	if (IsResetTime())
		UpdateDiceStore();
	else
		LoadDiceStore();

	SendDiceStoreToClient();
}

void FStoreController::Release()
{

}

void FStoreController::Tick(float InDeltaTime)
{
	if (IsResetTime())
	{
		UpdateDiceStore();
		SendDiceStoreToClient();
	}
}

void FStoreController::Handle_C_PURCHASE_DICE(const C_PURCHASE_DICE* InPacket)
{
	S_PURCHASE_DICE packet;
	packet.id = InPacket->id;
	packet.resultType = CheckPurchasableDice(InPacket->id);

	if (packet.resultType == STORE_PURCHASE_RESULT_SUCCESS)
	{
		DiceGoodsInfo& diceGoodsInfo = m_DiceMap.find(InPacket->id)->second;
		diceGoodsInfo.soldOut = true;

		if (FInventoryController* inventory = FindController<FInventoryController>())
		{
			inventory->GoldModification(-diceGoodsInfo.price);
		}

		if (FDiceController* diceController = FindController<FDiceController>())
		{
			diceController->AddDice(diceGoodsInfo.id, diceGoodsInfo.count);
		}

		string query = FUtil::SPrintf("UPDATE randomdice.dicerandomstore SET soldout=%d WHERE userid=%d AND diceid=%d", 1, GetDBID(), diceGoodsInfo.id);
		FDatabaseManager::GetInstance()->Query(query);
	}

	FServerManager::GetInstance()->Send(GetSocket(), &packet);
}

StorePurchaseResult FStoreController::CheckPurchasableDice(int InID) const
{
	auto iter = m_DiceMap.find(InID);
	if (iter == m_DiceMap.end())
		return STORE_PURCHASE_RESULT_INVALID_GOODS;

	const DiceGoodsInfo& diceGoodsInfo = iter->second;
	if (diceGoodsInfo.soldOut)
		return STORE_PURCHASE_RESULT_SOLDOUT;


	if (FInventoryController* inventory = FindController<FInventoryController>())
	{
		if (inventory->GetGold() < diceGoodsInfo.price)
			return STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY;
	}

	return STORE_PURCHASE_RESULT_SUCCESS;
}

void FStoreController::Handle_C_PURCHASE_BOX(const C_PURCHASE_BOX* InPacket)
{
	S_PURCHASE_BOX packet;
	packet.resultType = CheckPurchasableBox(InPacket->id);

	if (packet.resultType == STORE_PURCHASE_RESULT_SUCCESS)
	{
		const FBoxStoreData* boxData = FStoreDataManager::GetInstance()->FindBoxStoreData(InPacket->id);
		if (FInventoryController* inventory = FindController<FInventoryController>())
		{
			switch (boxData->priceType)
			{
			case STORE_PRICE_TYPE_GOLD: inventory->GoldModification(-boxData->goldPrice); break;
			case STORE_PRICE_TYPE_DIA: inventory->DiaModification(-boxData->diaPrice); break;
			case STORE_PRICE_TYPE_CARD: inventory->CardModification(-boxData->cardPrice); break;
			}

		}

		map<int, int> diceMap;
		for (auto& iter : boxData->goodsDataMap)
		{
			const FBoxGoodsData& boxGoodsData = iter.second;
			int totalCount = FUtil::Rand(boxGoodsData.min, boxGoodsData.max);
			while (totalCount)
			{
				int diceCount = FUtil::Rand(min(boxGoodsData.minPerOne, totalCount), min(boxGoodsData.maxPerOne, totalCount));
				int diceCountByGrade = FDiceDataManager::GetInstance()->GetDiceCountByGrade(boxGoodsData.grade);
				int diceIndexByGrade = FUtil::Rand(1, diceCountByGrade) - 1;
				int diceID = FDiceDataManager::GetInstance()->GetDiceIDByGrandAndIndex(boxGoodsData.grade, diceIndexByGrade);

				auto iter = diceMap.find(diceID);
				if (iter == diceMap.end())
					diceMap.insert(pair<int, int>(diceID, diceCount));
				else
					iter->second += diceCount;

				totalCount -= diceCount;
			}
		}

		if (FDiceController* diceController = FindController<FDiceController>())
		{
			vector<pair<int, int>> diceList = FUtil::ConvertMapToList<int, int>(diceMap);
			diceController->AddDice(diceList);
		}
	}

	FServerManager::GetInstance()->Send(GetSocket(), &packet);
}

StorePurchaseResult FStoreController::CheckPurchasableBox(int InID) const
{
	const FBoxStoreData* boxData = FStoreDataManager::GetInstance()->FindBoxStoreData(InID);
	if (boxData == nullptr)
		return STORE_PURCHASE_RESULT_INVALID_GOODS;

	if (FInventoryController* inventory = FindController<FInventoryController>())
	{
		switch (boxData->priceType)
		{
		case STORE_PRICE_TYPE_GOLD: if (inventory->GetGold() < boxData->goldPrice) return STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY;
		case STORE_PRICE_TYPE_DIA: if (inventory->GetDia() < boxData->diaPrice) return STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY;
		case STORE_PRICE_TYPE_CARD: if (inventory->GetCard() < boxData->cardPrice) return STORE_PURCHASE_RESULT_NOT_ENOUGH_MONEY;
		}
	}

	return STORE_PURCHASE_RESULT_SUCCESS;
}

void FStoreController::UpdateDiceStore()
{
	m_UpdateTimeSec = time(nullptr);

	string deleteQuery = FUtil::SPrintf("DELETE FROM randomdice.dicerandomstore WHERE userid=%d", GetDBID());
	FDatabaseManager::GetInstance()->Query(deleteQuery);

	string updateQuery = FUtil::SPrintf("UPDATE randomdice.user SET storeupdatetime=%lld WHERE id=%d", m_UpdateTimeSec, GetDBID());
	FDatabaseManager::GetInstance()->Query(updateQuery);

	m_DiceMap.clear();

	FStoreDataManager::GetInstance()->ForeachDiceGoodsDataMap([&](const FDiceGoodsData& InData) {
		int diceDrawCount = (rand() % (InData.drawMax - InData.drawMin + 1)) + InData.drawMin;
		if (diceDrawCount <= 0)
			return;

		int diceCountByGrade = FDiceDataManager::GetInstance()->GetDiceCountByGrade(InData.grade);
		int diceIndex = rand() % diceCountByGrade;
		int diceID = FDiceDataManager::GetInstance()->GetDiceIDByGrandAndIndex(InData.grade, diceIndex);
		if (diceID == DICE_ID_NONE)
			return;

		const FDiceData* diceData = FDiceDataManager::GetInstance()->FindDiceData(diceID);
		if (diceData == nullptr)
			return;

		const FDiceGoodsData* goodsData = FStoreDataManager::GetInstance()->FindDiceGoodsData(diceData->grade);
		if (goodsData == nullptr)
			return;

		DiceGoodsInfo diceGoodsInfo;
		diceGoodsInfo.id = diceID;
		diceGoodsInfo.count = goodsData->count;
		diceGoodsInfo.price = goodsData->price;

		m_DiceMap.insert(pair<int, DiceGoodsInfo>(diceID, diceGoodsInfo));
		});

	string insertQuery = FUtil::SPrintf("INSERT INTO randomdice.dicerandomstore (userid, diceid) VALUES");
	for (const auto& iter : m_DiceMap)
	{
		insertQuery += FUtil::SPrintf(" (%d, %d),", GetDBID(), iter.first);
	}
	insertQuery.pop_back();
	FDatabaseManager::GetInstance()->Query(insertQuery);
}

void FStoreController::LoadDiceStore()
{
	string query = FUtil::SPrintf("SELECT diceid, soldout FROM randomdice.dicerandomstore WHERE userid=%d", GetDBID());
	const MYSQL_ROWS* rows = FDatabaseManager::GetInstance()->Query(query);
	while (rows != nullptr)
	{
		int diceID = FUtil::AtoI(rows->data[0]);

		const FDiceData* diceData = FDiceDataManager::GetInstance()->FindDiceData(diceID);
		if (diceData == nullptr)
			continue;

		const FDiceGoodsData* goodsData = FStoreDataManager::GetInstance()->FindDiceGoodsData(diceData->grade);
		if (goodsData == nullptr)
			continue;

		DiceGoodsInfo diceGoodsInfo;
		diceGoodsInfo.id = diceID;
		diceGoodsInfo.count = goodsData->count;
		diceGoodsInfo.price = goodsData->price;
		diceGoodsInfo.soldOut = (bool)FUtil::AtoI(rows->data[1]);

		m_DiceMap.insert(pair<int, DiceGoodsInfo>(diceID, diceGoodsInfo));

		rows = rows->next;
	}
}

void FStoreController::SendDiceStoreToClient()
{
	S_STORE_DICE_LIST pkt;
	pkt.resetTime = m_UpdateTimeSec + m_ResetTimeSec;
	pkt.diceCount = m_DiceMap.size();

	int i = 0;
	for (const auto& iter : m_DiceMap)
	{
		pkt.diceList[i].id = iter.first;
		pkt.diceList[i].price = iter.second.price;
		pkt.diceList[i].count = iter.second.count;
		pkt.diceList[i].soldOut = iter.second.soldOut;
		++i;
	}

	FServerManager::GetInstance()->Send(GetSocket(), &pkt);
}

bool FStoreController::IsResetTime() const
{
	return m_ResetTimeSec <= (int)(time(nullptr) - m_UpdateTimeSec);
}
