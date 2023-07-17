#pragma once

class FPlayer;

class FMatchingManager : public FSingleton<FMatchingManager>
{
private:
	map<SOCKET, const FPlayer*> m_MatchingUserMap;

public:
	void AddUser(const FPlayer* InPlayer);
	void RemoveUser(SOCKET InSocket);
	
private:
	void MatchingUser(const FPlayer* InPlayer1, const FPlayer* InPlayer2);
};

