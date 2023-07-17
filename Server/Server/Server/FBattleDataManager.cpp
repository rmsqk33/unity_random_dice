#include "stdafx.h"
#include "FBattleDataManager.h"

void FBattleDataManager::Initialize()
{
    vector<const FDataNode*> nodeList = FDataCenter::GetInstance()->FindNodes("BattleData");
    for (const FDataNode* node : nodeList)
    {
        FBattleData battleData;
        node->ForeachChildNodes("Wave", [&](const FDataNode* InNode) {
            FWaveData waveData;
            waveData.wave = InNode->GetIntAttr("wave");
            waveData.card = InNode->GetIntAttr("card");
            waveData.exp = InNode->GetIntAttr("exp");
            battleData.waveList.push_back(waveData);
            });

        int battleID = node->GetIntAttr("id");
        battleData.maxWave = battleData.waveList.size();

        m_BattleDataMap.insert(pair<int, FBattleData>(battleID, battleData));
    }
}

int FBattleDataManager::CalcCardByClearWave(int InBattleID, int InWave) const
{
    int card = 0;
    if (const FBattleData* battleData = FindBattleData(InBattleID))
    {
        int wave = min(battleData->maxWave, InWave);
        for (int i = 0; i < wave; ++i)
        {
            card += battleData->waveList[i].card * (i / battleData->maxWave + 1);
        }
    }
    
    return card;
}

int FBattleDataManager::CalcExpByClearWave(int InBattleID, int InWave) const
{
    int exp = 0;
    if (const FBattleData* battleData = FindBattleData(InBattleID))
    {
        int wave = min(battleData->maxWave, InWave);
        for (int i = 0; i < wave; ++i)
        {
            exp += battleData->waveList[i].exp * (i / battleData->maxWave + 1);
        }
    }

    return exp;
}

const FBattleData* FBattleDataManager::FindBattleData(int InBattleID) const
{
    auto iter = m_BattleDataMap.find(InBattleID);
    if (iter != m_BattleDataMap.end())
        return &iter->second;

    return nullptr;
}
