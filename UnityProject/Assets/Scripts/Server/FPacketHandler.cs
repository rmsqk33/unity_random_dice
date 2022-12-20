using UnityEngine;

public class FPacketHandler
{
    [RuntimeInitializeOnLoadMethod]
    static void RegistPacketHandler()
    {
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_C_GUEST_LOGIN, Handle_C_GUEST_LOGIN);
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_CREATE_GUEST_ACCOUNT, Handle_S_CREATE_GUEST_ACCOUNT);
    }

    static void Handle_C_GUEST_LOGIN(in byte[] InBuffer)
    {
    }

    static void Handle_S_CREATE_GUEST_ACCOUNT(in byte[] InBuffer)
    {
    }
}
