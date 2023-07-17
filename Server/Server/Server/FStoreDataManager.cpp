#include "stdafx.h"
#include "FStoreDataManager.h"

void FStoreDataManager::Initialize()
{
	if (const FDataNode* diceStoreDataNode = FDataCenter::GetInstance()->FindNode("StoreDataList.DiceStoreData"))
	{
		m_DiceStoreResetTimeSec = diceStoreDataNode->GetIntAttr("ResetTime");
		diceStoreDataNode->ForeachChildNodes("Dice", [&](const FDataNode* InNode) {
			FDiceGoodsData goodsData;
			goodsData.grade = (DiceGrade)InNode->GetIntAttr("grade");
			goodsData.price = InNode->GetIntAttr("price");
			goodsData.count = InNode->GetIntAttr("count");
			goodsData.drawMin = InNode->GetIntAttr("drawMin");
			goodsData.drawMax = InNode->GetIntAttr("drawMax");

			m_DiceStoreDataMap.insert(pair<DiceGrade, FDiceGoodsData>(goodsData.grade, goodsData));
			});
	}
	
	vector<const FDataNode*> dataNodes = FDataCenter::GetInstance()->FindNodes("StoreDataList.BoxList.Box");
	for(const FDataNode* dataNode : dataNodes)
	{
		FBoxStoreData boxStoreData;
		boxStoreData.id = dataNode->GetIntAttr("id");
		boxStoreData.goldPrice = dataNode->GetIntAttr("goldPrice");
		boxStoreData.diaPrice = dataNode->GetIntAttr("diaPrice");
		boxStoreData.cardPrice = dataNode->GetIntAttr("cardPrice");

		if (boxStoreData.goldPrice != 0) boxStoreData.priceType = STORE_PRICE_TYPE_GOLD;
		else if (boxStoreData.diaPrice != 0) boxStoreData.priceType = STORE_PRICE_TYPE_DIA;
		else if (boxStoreData.cardPrice != 0) boxStoreData.priceType = STORE_PRICE_TYPE_CARD;

		dataNode->ForeachChildNodes("Dice", [&](const FDataNode* InNode) {
			FBoxGoodsData goodsData;
			goodsData.grade = (DiceGrade)InNode->GetIntAttr("grade");
			goodsData.min = InNode->GetIntAttr("min");
			goodsData.max = InNode->GetIntAttr("max");
			goodsData.minPerOne = InNode->GetIntAttr("minPerOne");
			goodsData.maxPerOne = InNode->GetIntAttr("maxPerOne");
			
			boxStoreData.goodsDataMap.insert(pair<DiceGrade, FBoxGoodsData>(goodsData.grade, goodsData));
			});

		m_BoxStoreDataMap.insert(pair<int, FBoxStoreData>(boxStoreData.id, boxStoreData));
	}
}

void FStoreDataManager::Release()
{
}

int FStoreDataManager::GetDiceStoreResetTimeSec() const
{
	return m_DiceStoreResetTimeSec;
}

const FDiceGoodsData* FStoreDataManager::FindDiceGoodsData(DiceGrade InGrade) const
{
	auto iter = m_DiceStoreDataMap.find(InGrade);
	if (iter != m_DiceStoreDataMap.end())
		return &iter->second;

	return nullptr;
}

const FBoxStoreData* FStoreDataManager::FindBoxStoreData(int InID) const
{
	auto iter = m_BoxStoreDataMap.find(InID);
	if (iter != m_BoxStoreDataMap.end())
		return &iter->second;

	return nullptr;
}

void FStoreDataManager::ForeachDiceGoodsDataMap(const function<void(const FDiceGoodsData&)>& InFunc) const
{
	for (auto& iter : m_DiceStoreDataMap)
	{
		InFunc(iter.second);
	}
}

void FStoreDataManager::ForeachBoxGoodsDataMap(int InID, const function<void(const FBoxGoodsData&)>& InFunc) const
{
	auto iter = m_BoxStoreDataMap.find(InID);
	if(iter != m_BoxStoreDataMap.end())
	{
		for (auto& i : iter->second.goodsDataMap)
		{
			InFunc(i.second);
		}
	}
}
