using Packet;
using UnityEngine;

public class FPacketHandler
{
    [RuntimeInitializeOnLoadMethod]
    static void RegistPacketHandler()
    {
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_GUEST_LOGIN, Handle_S_GUEST_LOGIN);
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_CREATE_GUEST_ACCOUNT, Handle_S_CREATE_GUEST_ACCOUNT);
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_USER_DATA, Handle_S_USER_DATA);
    }

    static void Handle_S_GUEST_LOGIN(in byte[] InBuffer)
    {
        S_GUEST_LOGIN pkt = new S_GUEST_LOGIN(InBuffer);

        FAccountMananger.Instance.Handle_S_GUEST_LOGIN(pkt.result != 0);
    }

    static void Handle_S_CREATE_GUEST_ACCOUNT(in byte[] InBuffer)
    {
        S_CREATE_GUEST_ACCOUNT pkt = new S_CREATE_GUEST_ACCOUNT(InBuffer);

        FAccountMananger.Instance.Handle_S_CREATE_GUEST_ACCOUNT(pkt.id);
    }

    static void Handle_S_USER_DATA(in byte[] InBuffer)
    {
        S_USER_DATA pkt = new S_USER_DATA(InBuffer);

        FUserDataController.Instance.Handle_S_USER_DATA(pkt);
    }
    
}