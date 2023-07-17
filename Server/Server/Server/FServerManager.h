#pragma once

#include "FSingleton.h"

struct PacketBase;

class FServerManager : public FSingleton<FServerManager>
{
private:
	typedef void(*PacketHandler)(SOCKET InClient, const char*);

	vector<SOCKET> m_ClientSocketList;
	map<PacketType, PacketHandler> m_PacketHandlerMap;

public:
	void Release() override;

	void AddClientSocket(SOCKET InSocket);
	void RemoveClientSocket(SOCKET InSocket);

	void Listen(SOCKET InSocket);
	void Send(SOCKET InSocket, const PacketBase* InPacket);
	void AddPacketHandler(PacketType InType, const PacketHandler& InFunc);
};

