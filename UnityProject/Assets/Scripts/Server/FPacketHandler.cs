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
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_CHANGE_GOLD, Handle_S_CHANGE_GOLD);
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_CHANGE_DIA, Handle_S_CHANGE_DIA);
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_PURCHASE_DICE, Handle_S_PURCHASE_DICE);
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_PURCHASE_BOX, Handle_S_PURCHASE_BOX);
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_ADD_DICE, Handle_S_ADD_DICE);
        FServerManager.Instance.AddPacketHandler(Packet.PacketType.PACKET_TYPE_S_UPGRADE_DICE, Handle_S_UPGRADE_DICE);
        
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

        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if (inventoryController != null)
        {
            inventoryController.Handle_S_USER_DATA(pkt);
        }

        FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
        if (diceController != null)
        {
            diceController.Handle_S_USER_DATA(pkt);
        }

        FBattlefieldController battlefieldController = FLocalPlayer.Instance.FindController<FBattlefieldController>();
        if (battlefieldController != null)
        {
            battlefieldController.Handle_S_USER_DATA(pkt);
        }

        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if (presetController != null)
        {
            presetController.Handle_S_USER_DATA(pkt);
        }

        FStatController statController = FLocalPlayer.Instance.FindController<FStatController>();
        if (statController != null)
        {
            statController.Handle_S_USER_DATA(pkt);
        }
    }

    static void Handle_S_STORE_DICE_LIST(in byte[] InBuffer)
    {
        S_STORE_DICE_LIST pkt = new S_STORE_DICE_LIST(InBuffer);

        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if (storeController != null)
        {
            storeController.Handle_S_STORE_DICE_LIST(pkt);
        }
    }

    static void Handle_S_PURCHASE_DICE(in byte[] InBuffer)
    {
        S_PURCHASE_DICE pkt = new S_PURCHASE_DICE(InBuffer);

        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if (storeController != null)
        {
            storeController.Handle_S_PURCHASE_DICE(pkt);
        }
    }

    static void Handle_S_ADD_DICE(in byte[] InBuffer)
    {
        S_ADD_DICE pkt = new S_ADD_DICE(InBuffer);

        FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
        if (diceController != null)
        {
            diceController.Handle_S_ADD_DICE(pkt);
        }
    }

    static void Handle_S_CHANGE_GOLD(in byte[] InBuffer)
    {
        S_CHANGE_GOLD pkt = new S_CHANGE_GOLD(InBuffer);

        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if(inventoryController != null)
        {
            inventoryController.Handle_S_CHANGE_GOLD(pkt);
        }
    }

    static void Handle_S_CHANGE_DIA(in byte[] InBuffer)
    {
        S_CHANGE_DIA pkt = new S_CHANGE_DIA(InBuffer);

        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if (inventoryController != null)
        {
            inventoryController.Handle_S_CHANGE_DIA(pkt);
        }
    }

    static void Handle_S_PURCHASE_BOX(in byte[] InBuffer)
    {
        S_PURCHASE_BOX pkt = new S_PURCHASE_BOX(InBuffer);

        FStoreController storeController = FLocalPlayer.Instance.FindController<FStoreController>();
        if(storeController != null)
        {
            storeController.Handle_S_PURCHASE_BOX(pkt);
        }
    }

    static void Handle_S_UPGRADE_DICE(in byte[] InBuffer)
    {
        S_UPGRADE_DICE pkt = new S_UPGRADE_DICE(InBuffer);

        FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
        if (diceController != null)
        {
            diceController.Handle_S_UPGRADE_DICE(pkt);
        }
    }
    
}