#pragma once

#include "FSingleton.h"

struct FDiceGoodsData
{
	DiceGrade grade;
	int drawMin;
	int drawMax;
	int price;
	int count;
};

struct FBoxGoodsData
{
	DiceGrade grade;
	int min;
	int max;
	int minPerOne;
	int maxPerOne;
};

struct FBoxStoreData
{
	int id;
	int goldPrice;
	int diaPrice;
	int cardPrice;
	StorePriceType priceType;
	map<DiceGrade, FBoxGoodsData> goodsDataMap;
};

class FStoreDataManager : public FSingleton<FStoreDataManager>
{
private:
	int m_DiceStoreResetTimeSec;
	map<DiceGrade, FDiceGoodsData> m_DiceStoreDataMap;
	map<int, FBoxStoreData> m_BoxStoreDataMap;

public:
	void Initialize();
	void Release() override;

	int GetDiceStoreResetTimeSec() const;
	const FDiceGoodsData* FindDiceGoodsData(DiceGrade InGrade) const;
	const FBoxStoreData* FindBoxStoreData(int InID) const;

	void ForeachDiceGoodsDataMap(const function<void(const FDiceGoodsData&)>& InFunc) const;
	void ForeachBoxGoodsDataMap(int InID, const function<void(const FBoxGoodsData&)>& InFunc) const;
};

