#pragma once

#include "FSingleton.h"

class FAccountManager : public FSingleton<FAccountManager>
{
private:


public:
	void RequestGuestLogin(SOCKET InClient, const string& InLoginID);
	void CreateGuestAccount(SOCKET InClient);

private:
	void AddPlayer(SOCKET InSocket, int InDBID);

	bool CreateUserDB(string& OutLoginID);
	void CreateAcquiredDiceDB(int InUserID);
	void CreateDicePresetDB(int InUserID);
	void CreateAcquiredBattlefieldDB(int InUserID);
	void CreateBattlefieldPresetDB(int InUserID);
};

