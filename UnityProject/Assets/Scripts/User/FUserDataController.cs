using Packet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FUserDataController
{
    byte m_Level = 0;
    int m_Gold = 0;
    int m_Dia = 0;
    int m_Exp = 0;
    int m_MaxExp = 0;
    string m_Name = new string("");

    public void Handle_S_USER_DATA(in S_USER_DATA InPacket)
    {
        m_Name = InPacket.name;
        m_Gold = InPacket.gold;
        m_Dia = InPacket.dia;
        m_Level = InPacket.level;
        m_Exp = InPacket.exp;
        m_MaxExp = FDataCenter.Instance.GetIntAttribute("UserClass.Class[@class=" + InPacket.level + "]@exp");
        UpdateUI();
    }

    void UpdateUI()
    {
        FLobbyUserInfoUI userInfoUI = FindUserInfoUI();
        if (userInfoUI)
        {
            userInfoUI.Name = m_Name;
            userInfoUI.Gold = m_Gold;
            userInfoUI.Dia = m_Dia;
            userInfoUI.MaxExp = m_MaxExp;
            userInfoUI.CurrentExp = m_Exp;
            userInfoUI.Level = m_Level;
        }
    }

    FLobbyUserInfoUI FindUserInfoUI()
    {
        GameObject userInfoUI = GameObject.Find("UserInfoUI");
        if (userInfoUI != null)
            return userInfoUI.GetComponent<FLobbyUserInfoUI>();

        return null;
    }
}
