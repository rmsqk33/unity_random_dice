#include "stdafx.h"
#include "FServerManager.h"
#include "FPacketHandler.h"

void FServerManager::Release()
{
	for (SOCKET& socket : m_ClientSocketList)
	{
		closesocket(socket);
	}
	m_ClientSocketList.clear();
}

void FServerManager::AddClientSocket(SOCKET InSocket)
{
	m_ClientSocketList.push_back(InSocket);

	thread([&, InSocket]() {
		Listen(InSocket);
		}).detach();
}

void FServerManager::RemoveClientSocket(SOCKET InSocket)
{
	auto iter = find(m_ClientSocketList.begin(), m_ClientSocketList.end(), InSocket);
	if (iter != m_ClientSocketList.end())
	{
		closesocket(*iter);
		m_ClientSocketList.erase(iter);

		FMatchingManager::GetInstance()->RemoveUser(InSocket);
	}
}

// 각 클라별 패킷 처리 스레드
void FServerManager::Listen(SOCKET InSocket)
{
	char buffer[MAX_PACKET_SIZE]; // 전달받은 데이터 전체, 여러 패킷이 저장될 수 있음
	int receivedPacketSize = 0; // 전달받은 패킷 전체 크기
	int packetSize = 0; // 제일 앞에 있는 패킷 크기

	while (true)
	{
		receivedPacketSize += recv(InSocket, (char*)buffer + receivedPacketSize, MAX_PACKET_SIZE - receivedPacketSize, 0);

		// 클라 접속이 끊긴 경우 접속 해제 처리 후 스레드 종료
		if (receivedPacketSize == 0)
		{
			gThreadLock.lock();
			FObjectManager::GetInstance()->RemovePlayer(InSocket);
			RemoveClientSocket(InSocket);
			gThreadLock.unlock();
			return;
		}

		if (MAX_PACKET_SIZE < receivedPacketSize)
			receivedPacketSize = 0;

		// 전달받은 패킷이 여러개일 수 있기 때문에 반복문으로 처리
		while (0 < receivedPacketSize)
		{
			// 제일 앞쪽 패킷 크기 읽기
			if (packetSize == 0)
				memcpy(&packetSize, buffer, sizeof(packetSize));

			// 패킷 전체가 오지 않았을 경우 다시 recv 처리하도록 반복문 탈출
			if (receivedPacketSize < packetSize)
				break;

			PacketType packetType = PACKET_TYPE_NONE;
			memcpy(&packetType, buffer + sizeof(packetSize), sizeof(PacketType));

			// 패킷이 온전히 왔을 때 그에 맞는 핸들러 호출
			auto iter = m_PacketHandlerMap.find(packetType);
			if (iter != m_PacketHandlerMap.end())
				iter->second(InSocket, buffer + sizeof(packetSize) + sizeof(PacketType));

			// 처리 완료된 패킷을 버퍼에서 제거
			receivedPacketSize -= packetSize;
			if (0 < receivedPacketSize)
				memcpy(buffer, buffer + packetSize, receivedPacketSize);

			packetSize = 0;
		}
	}
}

void FServerManager::Send(SOCKET InSocket, const PacketBase* InPacket)
{
	char buffer[MAX_PACKET_SIZE];
	ZeroMemory(buffer, MAX_PACKET_SIZE);

	int packetSize = InPacket->Serialize(buffer);
	send(InSocket, buffer, packetSize, 0);
}

void FServerManager::AddPacketHandler(PacketType InType, const PacketHandler& InFunc)
{
	m_PacketHandlerMap.insert(pair<PacketType, PacketHandler>(InType, InFunc));
}
