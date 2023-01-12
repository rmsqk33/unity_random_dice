using UnityEngine;
using InGamePacket;

public class PacketHandler
{
    [RuntimeInitializeOnLoadMethod]
    static void RegistPacketHandler()
    {
        PTPServerManager.Instance.AddPacketHandler(InGamePacket.PacketType.PACKET_TYPE_S_MOVE, HANDLE_S_MOVE);
        PTPServerManager.Instance.AddPacketHandler(InGamePacket.PacketType.PACKET_TYPE_C_MOVE, HANDLE_C_MOVE);
    }

    static void HANDLE_S_MOVE(in byte[] InBuffer)
    {
        S_MOVE message = new S_MOVE(InBuffer);
    }

    static void HANDLE_C_MOVE(in byte[] InBuffer)
    {
        S_MOVE message = new S_MOVE(InBuffer);
        PTPServerManager.Instance.SendMessage(message);
    }
}
