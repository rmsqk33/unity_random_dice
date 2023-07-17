#pragma once

#include "FSingleton.h"

class FConnectListener : public FSingleton<FConnectListener>
{
private:
	SOCKET m_ListenHandle;
	
public:
	bool Initialize();
	void Release();

	void Listen();
};

