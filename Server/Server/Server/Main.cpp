#include "stdafx.h"
#include "FConnectListener.h"
#include "FTimeManager.h"
#include "FObjectManager.h"
#include "FDatabaseManager.h"
#include "FDiceDataManager.h"
#include "FStoreDataManager.h"
#include "FServerManager.h"
#include "FUserDataManager.h"
#include "FBattlefieldDataManager.h"
#include "FBattleDataManager.h"

#ifdef _DEBUG
#include "FTest.h"
#endif 

mutex gThreadLock;

int main()
{
	if (FConnectListener::GetInstance()->Initialize() == false)
		return 0;
	
	if (FDatabaseManager::GetInstance()->ConnectDatabase("220.71.22.12", "root", "1234", "randomdice") == false)
		return 0;

	FDataCenter::GetInstance()->LoadData();

	FDiceDataManager::GetInstance()->Initialize();
	FStoreDataManager::GetInstance()->Initialize();
	FUserDataManager::GetInstance()->Initialize();
	FBattlefieldDataManager::GetInstance()->Initialize();
	FBattleDataManager::GetInstance()->Initialize();

#ifdef _DEBUG
	FTest test;
#endif

	FConnectListener::GetInstance()->Listen();

	INT64 periodFrequency;
	QueryPerformanceFrequency((LARGE_INTEGER*)&periodFrequency);
	double timeScale = 1.0 / periodFrequency;

	INT64 lastSec;
	QueryPerformanceCounter((LARGE_INTEGER*)&lastSec);

	while (true)
	{
		INT64 curSec;
		QueryPerformanceCounter((LARGE_INTEGER*)&curSec);

		float deltaTime = (curSec - lastSec) * timeScale;
		
		gThreadLock.lock();
		FTimeManager::GetInstance()->Tick(deltaTime);
		FObjectManager::GetInstance()->Tick(deltaTime);
		gThreadLock.unlock();

		lastSec = curSec;
	}

	return 0;
}