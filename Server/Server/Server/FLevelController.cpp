#include "stdafx.h"
#include "FLevelController.h"
#include "FMessageManager.h"
#include "FUtil.h"
#include "FDatabaseManager.h"

FLevelController::FLevelController(FPlayer* InOwner)
	: FControllerBase(InOwner)
{

}

FLevelController::~FLevelController()
{

}

void FLevelController::Initialize()
{
	string query = FUtil::SPrintf("SELECT * presetindex FROM randomdice.user WHERE id=%s", GetDBID());
	if (const MYSQL_ROWS* rows = FDatabaseManager::GetInstance()->Query(query.c_str()))
	{
		
	}
}

void FLevelController::SetLevel(int InLevel)
{
	m_Level = InLevel;

	if(const FDataNode* dataNode = FDataCenter::GetInstance()->FindNode("UserClass.Class"))

	S_CHANGE_LEVEL pkt;
	pkt.level = InLevel;
	pkt.exp = 0;
	pkt.maxExp = 

	FMessageManager::GetInstance()->Send(GetClientSocket(), &pkt);
}

int FLevelController::GetLevel() const
{
	return m_Level;
}

void FLevelController::SetExp(int InExp)
{
	m_Exp = InExp;
}

int FLevelController::GetExp() const
{
	return m_Exp;
}

int FLevelController::GetMaxExp() const
{
	return m_MaxExp;
}
