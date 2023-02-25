using Packet;
using System.Collections.Generic;
using UnityEngine;

public class FInventoryController : FControllerBase
{
    List<int> AcquiredBattleFielDList = new List<int>();

    public int Gold { get; set; }
    public int Dia { get; set; }

    public FInventoryController(FLocalPlayer InOwner) : base(InOwner)
    {
    }

    public void Handle_S_USER_DATA(in S_USER_DATA InPacket)
    {
        Gold = InPacket.gold;
        Dia = InPacket.dia;
    }
}
