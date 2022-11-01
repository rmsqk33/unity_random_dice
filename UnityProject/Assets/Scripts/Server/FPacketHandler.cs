using UnityEngine;

public class FPacketHandler
{
    [RuntimeInitializeOnLoadMethod]
    static void RegistPacketHandler()
    {
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_REQUEST_LOGIN, Handle_S_REQUEST_LOGIN);
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_ADD_GUEST_ACCOUNT, Handle_S_ADD_GUEST_ACCOUNT);
    }

    static void Handle_S_REQUEST_LOGIN(in byte[] InBuffer)
    {
    }

    static void Handle_S_ADD_GUEST_ACCOUNT(in byte[] InBuffer)
    {
    }
}
