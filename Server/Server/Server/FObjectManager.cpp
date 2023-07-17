#include "stdafx.h"
#include "FObjectManager.h"
#include "FPlayer.h"

void FObjectManager::Release()
{
	for (auto& iter : m_PlayerMap)
	{
		iter.second->Release();
		delete iter.second;
	}
	m_PlayerMap.clear();
}

const FPlayer* FObjectManager::AddPlayer(SOCKET InSocket, int InID)
{
	auto iter = m_PlayerMap.find(InSocket);
	if (iter != m_PlayerMap.end())
		return iter->second;

	FPlayer* player = new FPlayer(InSocket, InID);
	player->Initialize();

	m_PlayerMap.insert(pair<SOCKET, FPlayer*>(InSocket, player));

	return player;
}

void FObjectManager::RemovePlayer(SOCKET InSocket)
{
	auto iter = m_PlayerMap.find(InSocket);
	if (iter == m_PlayerMap.end())
		return;

	iter->second->Release();
	delete iter->second;

	m_PlayerMap.erase(InSocket);
}

void FObjectManager::Tick(float InDeltaTime)
{
	for (auto& iter : m_PlayerMap)
	{
		iter.second->Tick(InDeltaTime);
	}
}

FPlayer* FObjectManager::FindPlayer(SOCKET InSocket)
{
	auto iter = m_PlayerMap.find(InSocket);
	if (iter != m_PlayerMap.end())
		return iter->second;

	return nullptr;
}
