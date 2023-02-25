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
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_STORE_DICE_LIST, Handle_S_STORE_DICE_LIST);
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_PURCHASE_DICE, Handle_S_PURCHASE_DICE);
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_CHANGE_GOLD, Handle_S_CHANGE_GOLD);
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

    static void Handle_S_STORE_DICE_LIST(in byte[] InBuffer)
    {
        S_STORE_DICE_LIST pkt = new S_STORE_DICE_LIST(InBuffer);

        FStoreController.Instance.Handle_S_STORE_DICE_LIST(pkt);
    }

    static void Handle_S_PURCHASE_DICE(in byte[] InBuffer)
    {
        S_PURCHASE_DICE pkt = new S_PURCHASE_DICE(InBuffer);

        FStoreController.Instance.Handle_S_PURCHASE_DICE(pkt);
    }

    static void Handle_S_CHANGE_GOLD(in byte[] InBuffer)
    {
        S_CHANGE_GOLD pkt = new S_CHANGE_GOLD(InBuffer);

        FUserDataController.Instance.Gold = pkt.gold;
    }
}