#pragma once

#include "FControllerBase.h"

class FLevelController : public FControllerBase
{
private:
	int m_Level;
	int m_Exp;
	int m_MaxExp;

public:
	FLevelController(FPlayer* InOwner);
	~FLevelController();

	void Initialize() override;

	void SetLevel(int InLevel);
	int GetLevel() const;

	void SetExp(int InExp);
	int GetExp() const;
	int GetMaxExp() const;
};