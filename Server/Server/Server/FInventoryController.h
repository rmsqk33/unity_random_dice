#pragma once

#include "FControllerBase.h"

class FInventoryController : public FControllerBase
{
private:
	int m_Gold;
	int m_Dia;
	int m_Card;
	int m_PresetIndex;

public:
	FInventoryController(FPlayer* InOwner);
	~FInventoryController();

	void Initialize() override;

	void GoldModification(int InGold);
	int GetGold() const;

	void DiaModification(int InDia);
	int GetDia() const;

	void CardModification(int InCard);
	int GetCard() const;

	void SetPresetIndex(int InIndex);
	int GetPresetIndex() const;

	static ControllerType GetType()
	{
		return CONTROLLER_TYPE_INVENTORY;
	}
};