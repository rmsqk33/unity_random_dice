using Packet;
using System.Collections.Generic;
using UnityEngine;

public class FInventoryController : FControllerBase
{
    List<int> acquiredBattleFielDList = new List<int>();

    private int gold;
    private int dia;
    private int card;

    public int Gold { get { return gold; } }
    public int Dia { get { return dia; } }
    public int Card { get { return card; } }
    
    public FInventoryController(FLocalPlayer InOwner) : base(InOwner)
    {
    }

    public void Handle_S_USER_DATA(in S_USER_DATA InPacket)
    {
        gold = InPacket.gold;
        dia = InPacket.dia;
        card = InPacket.card;

        FLobbyUserInfoUI userInfoUI = FindLobbyUserInfoUI();
        if (userInfoUI != null)
        {
            userInfoUI.Initialize();
        }

        FBattleMenu battleMenu = FindBattleMenu();
        if (battleMenu != null)
        {
            battleMenu.Initialize();
        }
    }

    public void Handle_S_CHANGE_GOLD(in S_CHANGE_GOLD InPacket)
    {
        gold = InPacket.gold;

        FLobbyUserInfoUI ui = FindLobbyUserInfoUI();
        if (ui != null)
        {
            ui.Gold = gold;
        }
    }

    public void Handle_S_CHANGE_DIA(in S_CHANGE_DIA InPacket)
    {
        dia = InPacket.dia;

        FLobbyUserInfoUI ui = FindLobbyUserInfoUI();
        if(ui != null)
        {
            ui.Dia = dia;
        }
    }

    public void Handle_S_CHANGE_CARD(in S_CHANGE_CARD InPacket)
    {
        card = InPacket.card;

        FBattleMenu ui = FindBattleMenu();
        if (ui != null)
        {
            ui.Card = card;
        }
    }

    FLobbyUserInfoUI FindLobbyUserInfoUI()
    {
        return FUIManager.Instance.FindUI<FLobbyUserInfoUI>();
    }

    FBattleMenu FindBattleMenu()
    {
        return FUIManager.Instance.FindUI<FBattleMenu>();
    }
    
}
