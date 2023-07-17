#include "stdafx.h"
#include "FStatController.h"
#include "FDatabaseManager.h"
#include "FServerManager.h"

#include <regex>

FStatController::FStatController(FPlayer* InOwner)
	: FControllerBase(InOwner)
{
}

FStatController::~FStatController()
{
}

void FStatController::Initialize()
{
	string query = FUtil::SPrintf("SELECT level, exp, nickname FROM randomdice.user WHERE id=%d", GetDBID());
	if (const MYSQL_ROWS* rows = FDatabaseManager::GetInstance()->Query(query))
	{
		SetLevelInner(FUtil::AtoI(rows->data[0]));
		m_Exp = FUtil::AtoI(rows->data[1]);
		m_Name = rows->data[2];
	}
}

void FStatController::Handle_C_CHANGE_NAME(const C_CHANGE_NAME* InPacket)
{
	S_CHANGE_NAME packet;

	wstring nameWStr = InPacket->name;
	
	if (nameWStr.length() == 0)
	{
		packet.resultType = CHANGE_NAME_RESULT_BLANK;
		FServerManager::GetInstance()->Send(GetSocket(), &packet);
		return;
	}
	
	bool hasSpecialChar = false;
	for (wchar_t ch : nameWStr)
	{
		if ('0' <= ch && ch <= '9'
			|| 'a' <= ch && ch <= 'z'
			|| 'A' <= ch && ch <= 'Z'
			|| L'°¡' <= ch && ch <= L'ÆR')
			continue;

		hasSpecialChar = true;
		break;
	}

	if (hasSpecialChar)
	{
		packet.resultType = CHANGE_NAME_RESULT_SPECIAL_CHARACTER;
		FServerManager::GetInstance()->Send(GetSocket(), &packet);
		return;
	}

	char nameStr[1024];
	FUtil::WCharToChar(nameStr, InPacket->name);

	string checkDuplicationQuery = FUtil::SPrintf("SELECT nickname, COUNT(nickname) FROM randomdice.user GROUP BY \'%s\' HAVING COUNT(nickname) > 0", nameStr);
	const MYSQL_ROWS* rows = FDatabaseManager::GetInstance()->Query(checkDuplicationQuery);
	if (rows != nullptr && 0 < rows->length)
	{
		packet.resultType = CHANGE_NAME_RESULT_ALEADY;
		FServerManager::GetInstance()->Send(GetSocket(), &packet);
		return;
	}

	string updateQuery = FUtil::SPrintf("UPDATE randomdice.user SET nickname=\'%s\' WHERE id=%d", nameStr, GetDBID());
	FDatabaseManager::GetInstance()->Query(updateQuery);

	
	SetName(nameStr);

	packet.resultType = CHANGE_NAME_RESULT_SUCCESS;
	wcscpy_s(packet.name, nameWStr.c_str());
	FServerManager::GetInstance()->Send(GetSocket(), &packet);
}

void FStatController::SetLevel(int InLevel)
{
	SetLevelInner(InLevel);

	S_CHANGE_LEVEL packet;
	packet.level = m_Level;
	FServerManager::GetInstance()->Send(GetSocket(), &packet);

	string query = FUtil::SPrintf("UPDATE randomdice.user SET level=%d WHERE id=%d", m_Level, GetDBID());
	FDatabaseManager::GetInstance()->Query(query);
}

int FStatController::GetLevel() const
{
	return m_Level;
}

void FStatController::ExpModification(int InExp)
{
	m_Exp += InExp;
	if (m_MaxExp <= m_Exp)
	{
		SetLevel(m_Level + 1);
		m_Exp -= m_MaxExp;
	}

	S_CHANGE_EXP packet;
	packet.exp = m_Exp;
	FServerManager::GetInstance()->Send(GetSocket(), &packet);

	string query = FUtil::SPrintf("UPDATE randomdice.user SET exp=%d WHERE id=%d", m_Exp, GetDBID());
	FDatabaseManager::GetInstance()->Query(query);
}

int FStatController::GetExp() const
{
	return m_Exp;
}

void FStatController::SetName(const char* InName)
{
	m_Name = InName;
}

const char* FStatController::GetName() const
{
	return m_Name.c_str();
}

void FStatController::SetLevelInner(int InLevel)
{
	m_Level = InLevel;
	m_MaxExp = FDataCenter::GetInstance()->GetIntAttribute(FUtil::SPrintf("UserClass.Class[@class=%d]@exp", InLevel));
}
