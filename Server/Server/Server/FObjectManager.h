#pragma once

#include "FSingleton.h"

class FPlayer;

class FObjectManager : public FSingleton<FObjectManager>
{
private:
	map<SOCKET, FPlayer*> m_PlayerMap;

public:
	void Release();

	const FPlayer* AddPlayer(SOCKET InSocket, int InID);
	void RemovePlayer(SOCKET InSocket);

	void Tick(float InDeltaTime);

	FPlayer* FindPlayer(SOCKET InSocket);
};

