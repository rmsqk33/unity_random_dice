#pragma once

#include "FControllerBase.h"

class FStatController : public FControllerBase
{
private:
	string m_Name;
	int m_Level;
	int m_Exp;
	int m_MaxExp;

public:
	FStatController(FPlayer* InOwner);
	~FStatController();

	void Initialize() override;

	void Handle_C_CHANGE_NAME(const C_CHANGE_NAME* InPacket);

	void SetLevel(int InLevel);
	int GetLevel() const;

	void ExpModification(int InExp);
	int GetExp() const;

	void SetName(const char* InName);
	const char* GetName() const;

	static ControllerType GetType()
	{
		return CONTROLLER_TYPE_STAT;
	}

private:
	void SetLevelInner(int InLevel);
};

