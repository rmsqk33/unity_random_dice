#include "stdafx.h"
#include "FDatabaseManager.h"
#include "FTimeManager.h"

void FDatabaseManager::Release()
{
    if (m_Connection != nullptr)
    {
        mysql_close(m_Connection);
        delete m_Connection;
    }
}

bool FDatabaseManager::ConnectDatabase(const char* InHost, const char* InUser, const char* InPass, const char* InDBName, int InPort)
{
    if (m_Connection != nullptr)
        return false;

    m_Connection = new MYSQL();
    if (mysql_init(m_Connection) == nullptr)
        return false;

    if (mysql_real_connect(m_Connection, InHost, InUser, InPass, InDBName, InPort, (char*)NULL, 0) == nullptr)
        return false;

    mysql_set_character_set(m_Connection, "euckr");

    FTimeManager::GetInstance()->AddScheduleLoop(DB_CONNECT_PING_SEC, []() { FDatabaseManager::GetInstance()->ConnectPing(); });

    return true;
}

void FDatabaseManager::ConnectPing()
{
    if (m_Connection == nullptr)
        return;

    Query("SELECT * FROM randomdice.temp");
}

const MYSQL_ROWS* FDatabaseManager::Query(const string& InQuery) const
{
    if (m_Connection == nullptr)
        return nullptr;

    static mutex m;
    
    m.lock();

    MYSQL_ROWS* retVal = nullptr;
    if (mysql_query(m_Connection, InQuery.c_str()) != 0)
    {
        cout << "sql query error : " << mysql_error(m_Connection) << endl;
    }
    else if (MYSQL_RES* res = mysql_store_result(m_Connection))
    {
        retVal = res->data_cursor;
    }

    m.unlock();

    return retVal;
}
