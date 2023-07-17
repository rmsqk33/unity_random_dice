#include "stdafx.h"
#include "FConnectListener.h"
#include "FServerManager.h"

bool FConnectListener::Initialize()
{
	WSADATA wsaData;
	if (WSAStartup(MAKEWORD(2, 2), &wsaData))
	{
		cout << "WSA error";
		return false;
	}

	m_ListenHandle = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (m_ListenHandle == INVALID_SOCKET)
	{
		cout << "socket error";
		closesocket(m_ListenHandle);
		WSACleanup();
		return false;
	}

	SOCKADDR_IN listenAddr = {};
	listenAddr.sin_family = AF_INET;
	listenAddr.sin_port = htons(PORT);
	listenAddr.sin_addr.s_addr = htonl(INADDR_ANY);

	if (bind(m_ListenHandle, (const SOCKADDR*)&listenAddr, static_cast<int>(sizeof(listenAddr))))
	{
		cout << "bind error";
		closesocket(m_ListenHandle);
		WSACleanup();
		return false;
	}

	if (listen(m_ListenHandle, SOMAXCONN))
	{
		cout << "bind error";
		closesocket(m_ListenHandle);
		WSACleanup();
		return false;
	}

    return true;
}

void FConnectListener::Release()
{
	closesocket(m_ListenHandle);
	WSACleanup();
}

void FConnectListener::Listen()
{
	thread([&]() {
		SOCKADDR_IN clientAddr = {};
		int clientAddrSize = sizeof(clientAddr);
		while (1)
		{
			SOCKET clientSocket = accept(m_ListenHandle, (SOCKADDR*)&clientAddr, &clientAddrSize);
			if (clientSocket == INVALID_SOCKET)
				continue;

			FServerManager::GetInstance()->AddClientSocket(clientSocket);
		}
	}).detach();
}
