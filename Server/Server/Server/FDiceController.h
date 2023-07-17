#pragma once

#include "FControllerBase.h"

struct FDice
{
	int id;
	int level;
	int count;
};

class FDiceController : public FControllerBase
{
private:
	map<int, FDice> m_AcquiredDiceMap;
	int m_DicePreset[PRESET_MAX][DICE_PRESET_SLOT_MAX];

public:
	FDiceController(FPlayer* InOwner);

	void Initialize() override;
	void Release() override;

	void AddDice(int InID, int InCount);
	void AddDice(const vector<pair<int, int>>& InDiceList);
	void UpgradeDice(int InID);

	void SetDicePreset(int InPresetIndex, int InSlotIndex, int InDiceID);

	void ForeachAcquiredDice(const function<void(const FDice&)>& InFunc) const;
	void ForeachDicePreset(const function<void(int, int, int)>& InFunc) const;

	static ControllerType GetType()
	{
		return CONTROLLER_TYPE_DICE;
	}

private:
	DiceUpgradeResult CheckDiceUpgradable(int InID) const;

	void InitAcquiredDice();
	void InitDicePreset();
};

