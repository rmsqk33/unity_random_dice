#include "stdafx.h"
#include "FInventoryController.h"
#include "FServerManager.h"
#include "FDatabaseManager.h"

FInventoryController::FInventoryController(FPlayer* InOwner)
	: FControllerBase(InOwner)
{

}

FInventoryController::~FInventoryController()
{

}

void FInventoryController::Initialize()
{
	string query = FUtil::SPrintf("SELECT gold, dia, card, presetindex FROM randomdice.user WHERE id=%d", GetDBID());
	if (const MYSQL_ROWS* rows = FDatabaseManager::GetInstance()->Query(query))
	{
		m_Gold = FUtil::AtoI(rows->data[0]);
		m_Dia = FUtil::AtoI(rows->data[1]);
		m_Card = FUtil::AtoI(rows->data[2]);
		m_PresetIndex = FUtil::AtoI(rows->data[3]);
	}
}

void FInventoryController::GoldModification(int InGold)
{
	m_Gold += InGold;

	S_CHANGE_GOLD packet;
	packet.gold = m_Gold;
	FServerManager::GetInstance()->Send(GetSocket(), &packet);

	string query = FUtil::SPrintf("UPDATE randomdice.user SET gold=%d WHERE id=%d", m_Gold, GetDBID());
	FDatabaseManager::GetInstance()->Query(query);
}

int FInventoryController::GetGold() const
{
	return m_Gold;
}

void FInventoryController::DiaModification(int InDia)
{
	m_Dia += InDia;

	S_CHANGE_DIA packet;
	packet.dia = m_Dia;
	FServerManager::GetInstance()->Send(GetSocket(), &packet);

	string query = FUtil::SPrintf("UPDATE randomdice.user SET dia=%d WHERE id=%d", m_Dia, GetDBID());
	FDatabaseManager::GetInstance()->Query(query);
}

int FInventoryController::GetDia() const
{
	return m_Dia;
}

void FInventoryController::CardModification(int InCard)
{
	m_Card += InCard;

	S_CHANGE_CARD packet;
	packet.card = m_Card;
	FServerManager::GetInstance()->Send(GetSocket(), &packet);

	string query = FUtil::SPrintf("UPDATE randomdice.user SET card=%d WHERE id=%d", m_Card, GetDBID());
	FDatabaseManager::GetInstance()->Query(query);
}

int FInventoryController::GetCard() const
{
	return m_Card;
}

void FInventoryController::SetPresetIndex(int InIndex)
{
	if(m_PresetIndex == InIndex)
		return;

	m_PresetIndex = InIndex;

	string query = FUtil::SPrintf("UPDATE randomdice.user SET presetindex=%d WHERE id=%d", InIndex, GetDBID());
	FDatabaseManager::GetInstance()->Query(query);
}

int FInventoryController::GetPresetIndex() const
{
	return m_PresetIndex;
}
