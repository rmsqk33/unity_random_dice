#include "stdafx.h"
#include "FPlayer.h"
#include "FControllerBase.h"
#include "FDiceController.h"
#include "FBattlefieldController.h"
#include "FInventoryController.h"
#include "FStoreController.h"
#include "FStatController.h"

FPlayer::FPlayer(SOCKET InClientSocket, int InID)
	: m_ClientSocket(InClientSocket)
	, m_DBID(InID)
{
}

void FPlayer::Initialize()
{
	AddController<FStatController>();
	AddController<FInventoryController>();
	AddController<FDiceController>();
	AddController<FBattlefieldController>();
	AddController<FStoreController>();

	InitController();
}

void FPlayer::Release()
{
	for (auto& iter : m_Controllers)
	{
		delete iter.second;
	}
	m_Controllers.clear();
}

void FPlayer::Tick(float InDeltaTime)
{
	for (auto& iter : m_Controllers)
	{
		iter.second->Tick(InDeltaTime);
	}
}

SOCKET FPlayer::GetClientSocket() const
{
	return m_ClientSocket;
}

int FPlayer::GetDBID() const
{
	return m_DBID;
}

void FPlayer::InitController()
{
	for (auto& iter : m_Controllers)
	{
		iter.second->Initialize();
	}
}
