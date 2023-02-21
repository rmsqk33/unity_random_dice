using Mono.Cecil;
using Packet;
using System;
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
    int[,] m_DicePresetIDList = new int[5,5];
    int[] m_BattleFieldPresetIDList = new int[5];
    int m_SelectedPresetIndex = 0;

    public int Level { get { return m_Level; } }
    public int Gold { get { return m_Gold; } }
    public int Dia { get { return m_Dia; } }
    public int Exp { get { return m_Exp; } }
    public int MaxExp { get { return m_MaxExp; } }
    public int Critical { get { return m_Critical; } }
    public int SelectedPresetIndex { get { return m_SelectedPresetIndex; } }
    public string Name { get { return m_Name; } }

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

        C_CHANGE_PRESET packet = new C_CHANGE_PRESET();
        packet.presetIndex = InIndex;
        FServerManager.Instance.SendMessage(packet);
    }

    public void SetDicePreset(int InID, int InIndex)
    {
        C_CHANGE_PRESET_DICE packet = new C_CHANGE_PRESET_DICE();

        FDicePreset dicePresetUI = FindDicePreset();
        int prevIndex = GetDicePresetIndex(InID, m_SelectedPresetIndex);
        if (prevIndex != -1)
        {
            int prevDiceID = m_DicePresetIDList[m_SelectedPresetIndex, InIndex];
            m_DicePresetIDList[m_SelectedPresetIndex, prevIndex] = prevDiceID;
            dicePresetUI.SetDicePreset(prevDiceID, prevIndex);

            packet.diceId = prevDiceID;
            packet.slotIndex = prevIndex;
            packet.presetIndex = m_SelectedPresetIndex;
            FServerManager.Instance.SendMessage(packet);
        }

        m_DicePresetIDList[m_SelectedPresetIndex, InIndex] = InID;
        dicePresetUI.SetDicePreset(InID, InIndex);

        packet.diceId = InID;
        packet.slotIndex = InIndex;
        packet.presetIndex = m_SelectedPresetIndex;
        FServerManager.Instance.SendMessage(packet);
    }

    public void SetBattleFieldPreset(int InID)
    {
        FBattleFieldPreset battleFieldPresetUI = FindBattleFieldPreset();
        m_BattleFieldPresetIDList[m_SelectedPresetIndex] = InID;
        battleFieldPresetUI.SetBattleFieldPreset(InID);

        C_CHANGE_PRESET_BATTLEFIELD packet = new C_CHANGE_PRESET_BATTLEFIELD();
        packet.battlefieldId = InID;
        packet.presetIndex = m_SelectedPresetIndex;
        FServerManager.Instance.SendMessage(packet);
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
        Array.Copy(InPacket.dicePreset, m_DicePresetIDList, InPacket.dicePreset.Length);
        Array.Copy(InPacket.battleFieldPreset, m_BattleFieldPresetIDList, InPacket.battleFieldPreset.Length);
        m_SelectedPresetIndex = InPacket.selectedPresetIndex;
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
        return  GameObject.FindObjectOfType<FBattleFieldInventory>();
    }

    FLobbyUserInfoUI FindLobbyUserInfoUI()
    {
        return  GameObject.FindObjectOfType<FLobbyUserInfoUI>();
    }

    FDicePreset FindDicePreset()
    {
        return  GameObject.FindObjectOfType<FDicePreset>();
    }

    FBattleFieldPreset FindBattleFieldPreset()
    {
        return  GameObject.FindObjectOfType<FBattleFieldPreset>();
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
