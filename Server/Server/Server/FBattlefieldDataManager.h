#pragma once

#include "FSingleton.h"

struct FBattlefieldData
{
	int id;
	int price;
};

class FBattlefieldDataManager : public FSingleton<FBattlefieldDataManager>
{
private:
	map<int, FBattlefieldData> m_BattlefieldDataMap;

public:
	void Initialize();

	const FBattlefieldData* FindBattlefieldData(int InID) const;
};

