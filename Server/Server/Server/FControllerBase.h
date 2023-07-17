#pragma once

#include "FPlayer.h"

class FControllerBase
{
private:
	FPlayer* m_Owner;

public:
	FControllerBase(FPlayer* InOwner);
	virtual ~FControllerBase() {}

	virtual void Initialize() {}
	virtual void Release() {}
	virtual void Tick(float InDeltaTime) {}

	FPlayer* GetOwner() const;
	int GetDBID() const;
	SOCKET GetSocket() const;

	template<typename T>
	T* FindController() const
	{
		return GetOwner()->FindController<T>();
	}
};

