#include "stdafx.h"
#include "FBattlefieldDataManager.h"

void FBattlefieldDataManager::Initialize()
{
    vector<const FDataNode*> battlefieldNodeList = FDataCenter::GetInstance()->FindNodes("BattleFieldList.BattleField");
    for (const FDataNode* battlefieldNode : battlefieldNodeList)
    {
        FBattlefieldData battlefieldData;
        battlefieldData.id = battlefieldNode->GetIntAttr("id");
        battlefieldData.price = battlefieldNode->GetIntAttr("price");

        m_BattlefieldDataMap.insert(pair<int, FBattlefieldData>(battlefieldData.id, battlefieldData));
    }
}

const FBattlefieldData* FBattlefieldDataManager::FindBattlefieldData(int InID) const
{
    auto iter = m_BattlefieldDataMap.find(InID);
    if (iter != m_BattlefieldDataMap.end())
        return &iter->second;

    return nullptr;
}
