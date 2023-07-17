#include "stdafx.h"
#include "FUserDataManager.h"
#include "FDataCenter.h"
#include "FUtil.h"

void FUserDataManager::Initialize()
{
	if(const FDataNode* initUserDataNode = FDataCenter::GetInstance()->FindNode("InitUserData"))
	{
		m_InitGold = initUserDataNode->GetIntAttr("gold");
		m_InitDia = initUserDataNode->GetIntAttr("dia");
		m_InitCard = initUserDataNode->GetIntAttr("card");
		
		vector<string> diceIdStrList = FUtil::SplitString(initUserDataNode->GetStringAttr("diceIdList"), ",");
		for(const string& idStr : diceIdStrList)
		{
			m_InitDiceList.push_back(FUtil::AtoI(idStr.c_str()));
		}

		vector<string> battlefieldIdStrList = FUtil::SplitString(initUserDataNode->GetStringAttr("battlefieldIdList"), ",");
		for(const string& idStr : battlefieldIdStrList)
		{
			m_InitBattlefieldList.push_back(FUtil::AtoI(idStr.c_str()));
		}

		if(const FDataNode* presetListNode = initUserDataNode->FindNode("PresetList"))
		{
			presetListNode->ForeachChildNodes("Preset", [&](const FDataNode* InNode){
				vector<string> diceIdStrList = FUtil::SplitString(InNode->GetStringAttr("diceIdList"), ",");
				vector<int> dicePresetList;
				for(const string& idStr : diceIdStrList)
				{
					dicePresetList.push_back(FUtil::AtoI(idStr.c_str()));
				}
				m_DicePresetList.push_back(dicePresetList);
				m_BattlefieldPresetList.push_back(InNode->GetIntAttr("battlefieldId"));
			});
		}
	}
}
    
int FUserDataManager::GetInitGold() const
{
	return m_InitGold;
}

int FUserDataManager::GetInitDia() const
{
	return m_InitDia;
}

int FUserDataManager::GetInitCard() const
{
	return m_InitCard;
}

void FUserDataManager::ForeachInitDiceList(const function<void(int)>& InFunc) const
{
	for(int id : m_InitDiceList)
	{
		InFunc(id);
	}
}

void FUserDataManager::ForeachDicePreset(const function<void(int, int, int)>& InFunc) const
{
	for(int i = 0; i < m_DicePresetList.size(); ++i)
	{
		for(int j = 0; j < m_DicePresetList[i].size(); ++j)
		{
			InFunc(i, j, m_DicePresetList[i][j]);
		}
	}
}

void FUserDataManager::ForeachInitBattlefieldList(const function<void(int)>& InFunc) const
{
	for(int id : m_InitBattlefieldList)
	{
		InFunc(id);
	}
}

void FUserDataManager::ForeachBattlefieldPreset(const function<void(int, int)>& InFunc) const
{
	for(int i = 0; i < m_BattlefieldPresetList.size(); ++i)
	{
		InFunc(i, m_BattlefieldPresetList[i]);
	}
}
