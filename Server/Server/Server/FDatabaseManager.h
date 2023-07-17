#pragma once

#include "FSingleton.h"
#include "mysql.h"

#pragma comment(lib, "libmysql.lib")

class FDatabaseManager : public FSingleton<FDatabaseManager>
{
private:
	const float DB_CONNECT_PING_SEC = 300.f;

	MYSQL* m_Connection = nullptr;

public:
	void Release() override;
	
	bool ConnectDatabase(const char* InHost, const char* InUser, const char* InPass, const char* InDBName, int InPort = 3306);
	const MYSQL_ROWS* Query(const string& InQuery) const;

private:
	void ConnectPing();
};
