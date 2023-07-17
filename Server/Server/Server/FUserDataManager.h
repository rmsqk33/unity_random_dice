#pragma once

#include "FSingleton.h"

class FUserDataManager : public FSingleton<FUserDataManager>
{
private:
    int m_InitGold;
    int m_InitDia;
    int m_InitCard;
    vector<int> m_InitDiceList;
    vector<int> m_InitBattlefieldList;
    vector<vector<int>> m_DicePresetList;
    vector<int> m_BattlefieldPresetList;

public:
    void Initialize();

    int GetInitGold() const;
    int GetInitDia() const;
    int GetInitCard() const;

    void ForeachInitDiceList(const function<void(int)>& InFunc) const;
    void ForeachDicePreset(const function<void(int, int, int)>& InFunc) const;

    void ForeachInitBattlefieldList(const function<void(int)>& InFunc) const;
    void ForeachBattlefieldPreset(const function<void(int, int)>& InFunc) const;
};