#pragma once

#include "FControllerBase.h"

class FStoreController : public FControllerBase
{
private:
	struct DiceGoodsInfo
	{
		int id = 0;
		int count = 0;
		int price = 0;
		bool soldOut = false;
	};

	map<int, DiceGoodsInfo> m_DiceMap;
	time_t m_UpdateTimeSec;
	int m_ResetTimeSec;

public:
	FStoreController(FPlayer* InOwner);
	~FStoreController();

	void Initialize() override;
	void Release() override;
	void Tick(float InDeltaTime) override;

	void Handle_C_PURCHASE_DICE(const C_PURCHASE_DICE* InPacket);
	void Handle_C_PURCHASE_BOX(const C_PURCHASE_BOX* InPacket);

	static ControllerType GetType()
	{
		return CONTROLLER_TYPE_STORE;
	}

private:
	void LoadDiceStore();
	void UpdateDiceStore();
	void SendDiceStoreToClient();

	bool IsResetTime() const;

	StorePurchaseResult CheckPurchasableDice(int InID) const;
	StorePurchaseResult CheckPurchasableBox(int InID) const;
};
