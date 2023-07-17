#include "stdafx.h"
#include "FMatchingManager.h"
#include "FServerManager.h"
#include "FPlayer.h"

void FMatchingManager::AddUser(const FPlayer* InPlayer)
{
	if (m_MatchingUserMap.find(InPlayer->GetClientSocket()) != m_MatchingUserMap.end())
		return;

	m_MatchingUserMap.insert(pair<SOCKET, const FPlayer*>(InPlayer->GetClientSocket(), InPlayer));
	if (2 <= m_MatchingUserMap.size())
	{
		MatchingUser(m_MatchingUserMap.begin()->second, (++m_MatchingUserMap.begin())->second);
	}
}

void FMatchingManager::RemoveUser(SOCKET InSocket)
{
	auto iter = m_MatchingUserMap.find(InSocket);
	if (iter == m_MatchingUserMap.end())
		return;

	m_MatchingUserMap.erase(InSocket);
}

void FMatchingManager::MatchingUser(const FPlayer* InPlayer1, const FPlayer* InPlayer2)
{
	S_BATTLE_MATCHING packet1;
	packet1.isHost = true;
	FServerManager::GetInstance()->Send(InPlayer1->GetClientSocket(), &packet1);

	sockaddr_in sockAddr;
	int size = sizeof(sockAddr);
	getsockname(InPlayer1->GetClientSocket(), (struct sockaddr*)&sockAddr, &size);

	S_BATTLE_MATCHING packet2;
	packet2.isHost = false;
	FUtil::CharToWChar(packet2.hostIP, inet_ntoa(sockAddr.sin_addr));
	FServerManager::GetInstance()->Send(InPlayer2->GetClientSocket(), &packet2);

	m_MatchingUserMap.erase(InPlayer1->GetClientSocket());
	m_MatchingUserMap.erase(InPlayer2->GetClientSocket());
}
