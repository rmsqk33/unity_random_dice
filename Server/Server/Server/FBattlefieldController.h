#pragma once

#include "FControllerBase.h"

struct FBattlefield
{
	int id;
	int level;
};

class FBattlefieldController : public FControllerBase
{
private:
	vector<int> m_AcquiredBattlefieldList;
	int m_BattlefieldPreset[PRESET_MAX];

public:
	FBattlefieldController(FPlayer* InOwner);
	~FBattlefieldController();

	void Initialize() override;

	void Handle_C_PURCHASE_BATTLEFIELD(const C_PURCHASE_BATTLEFIELD* InPacket);

	void SetBattlefieldPreset(int InPresetIndex, int InBattlefieldID);

	void ForeachAcquiredBattlefield(const function<void(int)>& InFunc) const;
	void ForeachBattlefieldPreset(const function<void(int, int)>& InFunc) const;

	static ControllerType GetType()
	{
		return CONTROLLER_TYPE_BATTLEFIELD;
	}

private:
	void InitAcquiredBattlefield();
	void InitBattlefieldPreset();

	bool IsAcquiredBattlefield(int InID) const;
};

