using Packet;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FInventoryController : FControllerBase
{
    List<int> AcquiredBattleFielDList = new List<int>();

    public int Gold { get; private set; }
    public int Dia { get; private set; }

    public FInventoryController(FLocalPlayer InOwner) : base(InOwner)
    {
    }

    public void Handle_S_USER_DATA(in S_USER_DATA InPacket)
    {
        Gold = InPacket.gold;
        Dia = InPacket.dia;
    }

    public void Handle_S_CHANGE_GOLD(in S_CHANGE_GOLD InPacket)
    {
        Gold = InPacket.gold;

        FLobbyUserInfoUI ui = FindLobbyUserInfoUI();
        if (ui != null)
        {
            ui.Gold = Gold;
        }
    }

    public void Handle_S_CHANGE_DIA(in S_CHANGE_DIA InPacket)
    {
        Dia = InPacket.dia;

        FLobbyUserInfoUI ui = FindLobbyUserInfoUI();
        if(ui != null)
        {
            ui.Dia = Dia;
        }
    }

    private FLobbyUserInfoUI FindLobbyUserInfoUI()
    {
        return GameObject.FindObjectOfType<FLobbyUserInfoUI>();
    }
}
