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

// �� Ŭ�� ��Ŷ ó�� ������
void FServerManager::Listen(SOCKET InSocket)
{
	char buffer[MAX_PACKET_SIZE]; // ���޹��� ������ ��ü, ���� ��Ŷ�� ����� �� ����
	int receivedPacketSize = 0; // ���޹��� ��Ŷ ��ü ũ��
	int packetSize = 0; // ���� �տ� �ִ� ��Ŷ ũ��

	while (true)
	{
		receivedPacketSize += recv(InSocket, (char*)buffer + receivedPacketSize, MAX_PACKET_SIZE - receivedPacketSize, 0);

		// Ŭ�� ������ ���� ��� ���� ���� ó�� �� ������ ����
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

		// ���޹��� ��Ŷ�� �������� �� �ֱ� ������ �ݺ������� ó��
		while (0 < receivedPacketSize)
		{
			// ���� ���� ��Ŷ ũ�� �б�
			if (packetSize == 0)
				memcpy(&packetSize, buffer, sizeof(packetSize));

			// ��Ŷ ��ü�� ���� �ʾ��� ��� �ٽ� recv ó���ϵ��� �ݺ��� Ż��
			if (receivedPacketSize < packetSize)
				break;

			PacketType packetType = PACKET_TYPE_NONE;
			memcpy(&packetType, buffer + sizeof(packetSize), sizeof(PacketType));

			// ��Ŷ�� ������ ���� �� �׿� �´� �ڵ鷯 ȣ��
			auto iter = m_PacketHandlerMap.find(packetType);
			if (iter != m_PacketHandlerMap.end())
				iter->second(InSocket, buffer + sizeof(packetSize) + sizeof(PacketType));

			// ó�� �Ϸ�� ��Ŷ�� ���ۿ��� ����
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
