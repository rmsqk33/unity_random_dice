using Mono.Cecil;
using Packet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public struct FDice
{ 
    public int id;
    public int level;
    public int exp;
}

public class FUserDataController : FNonObjectSingleton<FUserDataController>
{
    int m_Level = 0;
    int m_Gold = 0;
    int m_Dia = 0;
    int m_Exp = 0;
    int m_MaxExp = 0;
    int m_Critical = 0;
    
    string m_Name = new string("");
    Dictionary<int, FDice> m_AcquiredDiceMap = new Dictionary<int, FDice>();
    Dictionary<int, int> m_AcquiredBattleFieldMap = new Dictionary<int, int>();
    int[,] m_DicePresetIDList;
    int[] m_BattleFieldPresetIDList;
    int m_SelectedPresetIndex = 0;

    public int Level { get { return m_Level; } }
    public int Gold { get { return m_Gold; } }
    public int Dia { get { return m_Dia; } }
    public int Exp { get { return m_Exp; } }
    public int MaxExp { get { return m_MaxExp; } }
    public int Critical { get { return m_Critical; } }
    public int SelectedPresetIndex { get { return m_SelectedPresetIndex; } }
    public string Name { get { return m_Name; } }

#if DEBUG
    [RuntimeInitializeOnLoadMethod]
    static void Test()
    {
        S_USER_DATA testPacket = new S_USER_DATA();
        testPacket.exp = 300;
        testPacket.name = "ÈÄ½Ãµò";
        testPacket.level = 3;
        testPacket.dia = 30000;
        testPacket.gold = 30000;
        testPacket.selectedPresetIndex = 0;
        for(int i = 0; i < 5; ++i)
        {
            for(int j = 0; j < 5; ++j)
            {
                testPacket.dicePreset[i, j] = (i + j) % 5 + 1;
            }
        }
        
        for (int i = 0; i < 10; ++i)
        {
            testPacket.diceDataList[i].id = i + 1;
            testPacket.diceDataList[i].count = 200;
            testPacket.diceDataList[i].level = 6;
        }

        for(int i = 0; i < 3; ++i)
        {
            testPacket.battleFieldDataList[i].id = i + 1;
            testPacket.battleFieldDataList[i].level = 1;
        }

        for(int i = 0; i < testPacket.battleFieldPreset.Length; ++i)
        {
            testPacket.battleFieldPreset[i] = 1;
        }

        Instance.Handle_S_USER_DATA(testPacket);
    }
#endif

    public void Handle_S_USER_DATA(in S_USER_DATA InPacket)
    {
        InitLobbyUserInfoUI(InPacket);
        InitInventory(InPacket);
        InitPreset(InPacket);
    }

    public FDice? FindAcquiredDice(int InID)
    {
        return m_AcquiredDiceMap.ContainsKey(InID) ? m_AcquiredDiceMap[InID] : null;
    }

    public bool IsAcquiredBattleField(int InID)
    {
        return m_AcquiredBattleFieldMap.ContainsKey(InID);
    }

    public delegate void ForeachDicePresetHandle(int InID);
    public void ForeachDicePreset(int InIndex, in ForeachDicePresetHandle InFunc)
    {
        if (m_DicePresetIDList == null)
            return;

        for(int i = 0; i < m_DicePresetIDList.GetLength(0); ++i)
        {
            InFunc(m_DicePresetIDList[InIndex, i]);
        }
    }

    public int GetBattleFieldPresetID(int InIndex)
    {
        if (m_BattleFieldPresetIDList == null)
            return 0;

        return m_BattleFieldPresetIDList[InIndex];
    }

    public void SetPreset(int InIndex)
    {
        if (m_SelectedPresetIndex == InIndex)
            return;

        m_SelectedPresetIndex = InIndex;

        FDicePreset dicePresetUI = FindDicePreset();
        if (dicePresetUI != null)
        {
            dicePresetUI.SetPreset(InIndex);
        }

        FBattleFieldPreset battleFieldPresetUI = FindBattleFieldPreset();
        if (battleFieldPresetUI != null)
        {
            battleFieldPresetUI.SetPreset(InIndex);
        }
    }

    public void SetDicePreset(int InID, int InIndex)
    {
        FDicePreset dicePresetUI = FindDicePreset();
        int prevIndex = GetDicePresetIndex(InID, m_SelectedPresetIndex);
        if (prevIndex != -1)
        {
            int prevDiceID = m_DicePresetIDList[m_SelectedPresetIndex, InIndex];
            m_DicePresetIDList[m_SelectedPresetIndex, prevIndex] = prevDiceID;
            dicePresetUI.SetDicePreset(prevDiceID, prevIndex);
        }

        m_DicePresetIDList[m_SelectedPresetIndex, InIndex] = InID;
        dicePresetUI.SetDicePreset(InID, InIndex);
    }

    public void SetBattleFieldPreset(int InID)
    {
        FBattleFieldPreset battleFieldPresetUI = FindBattleFieldPreset();
        m_BattleFieldPresetIDList[m_SelectedPresetIndex] = InID;
        battleFieldPresetUI.SetBattleFieldPreset(InID);
    }

    int GetDicePresetIndex(int InID, int InIndex)
    {
        for(int i = 0; i < m_DicePresetIDList.GetLength(0); ++i)
        {
            if (m_DicePresetIDList[InIndex, i] == InID)
                return i;
        }
        return -1;
    }

    void InitLobbyUserInfoUI(in S_USER_DATA InPacket)
    {
        m_Name = InPacket.name;
        m_Gold = InPacket.gold;
        m_Dia = InPacket.dia;
        m_Level = InPacket.level;
        m_Exp = InPacket.exp;
        m_MaxExp = FDataCenter.Instance.GetIntAttribute("UserClass.Class[@class=" + InPacket.level + "]@exp");

        FLobbyUserInfoUI userInfoUI = FindLobbyUserInfoUI();
        if (userInfoUI != null)
        {
            userInfoUI.InitUserInfo();
        }
    }

    void InitInventory(in S_USER_DATA InPacket)
    {
        foreach(S_USER_DATA.DICE_DATA diceData in InPacket.diceDataList)
        {
            if (diceData.id != 0)
            {
                AddAcquiredDice(diceData);
            }
        }

        foreach (S_USER_DATA.BATTLEFIELD_DATA battleFieldData in InPacket.battleFieldDataList)
        {
            if (battleFieldData.id != 0)
            {
                AddAcquiredBattleField(battleFieldData);
            }
        }

        FDiceInventory diceInventory = FindDiceInventory();
        if (diceInventory != null)
            diceInventory.On_S_USER_DATA();

        FBattleFieldInventory battleFieldInventory = FindBattleFieldInventory();
        if (battleFieldInventory != null)
            battleFieldInventory.On_S_USER_DATA();
    }

    void InitPreset(in S_USER_DATA InPacket)
    {
        m_DicePresetIDList = InPacket.dicePreset.Clone() as int[,];
        m_BattleFieldPresetIDList = InPacket.battleFieldPreset.Clone() as int[];
        m_SelectedPresetIndex = InPacket.selectedPresetIndex;

        SetPreset(m_SelectedPresetIndex);
    }

    void AddAcquiredDice(in S_USER_DATA.DICE_DATA InData)
    {
        FDice dice = new FDice();
        dice.id = InData.id;
        dice.exp = InData.count;
        dice.level = InData.level;

        m_AcquiredDiceMap.Add(InData.id, dice);

        AddCritical(InData.id, InData.level);
    }

    void AddAcquiredBattleField(in S_USER_DATA.BATTLEFIELD_DATA InData)
    {
        m_AcquiredBattleFieldMap.Add(InData.id, InData.level);
    }

    FDiceInventory FindDiceInventory()
    {
        GameObject gameObject = GameObject.Find("DiceInventory");
        if (gameObject != null)
            return gameObject.GetComponent<FDiceInventory>();

        return null;
    }

    FBattleFieldInventory FindBattleFieldInventory()
    {
        GameObject gameObject = GameObject.Find("BattleFieldInventory");
        if (gameObject != null)
            return gameObject.GetComponent<FBattleFieldInventory>();

        return null;
    }

    FLobbyUserInfoUI FindLobbyUserInfoUI()
    {
        GameObject gameObject = GameObject.Find("UserInfoUI");
        if (gameObject != null)
            return gameObject.GetComponent<FLobbyUserInfoUI>();

        return null;
    }

    FDicePreset FindDicePreset()
    {
        GameObject gameObject = GameObject.Find("DicePreset");
        if (gameObject != null)
            return gameObject.GetComponent<FDicePreset>();

        return null;
    }

    FBattleFieldPreset FindBattleFieldPreset()
    {
        GameObject gameObject = GameObject.Find("BattleFieldPreset");
        if (gameObject != null)
            return gameObject.GetComponent<FBattleFieldPreset>();

        return null;
    }

    void AddCritical(int InDiceID, int InLevel)
    {
        FDiceGradeData? data = FDiceDataManager.Instance.FindGradeDataByID(InDiceID);
        if(data != null)
        {
            m_Critical += data.Value.Critical * InLevel;
        }
    }
}
