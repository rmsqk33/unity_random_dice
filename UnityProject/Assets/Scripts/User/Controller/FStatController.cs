using Packet;
using UnityEngine;

public class FStatController : FControllerBase
{
    public int Level { get; set; }
    public int Exp { get; set; }
    public int MaxExp { get; set; }
    public int Critical { get; private set; }
    public string Name { get; set; }

    public FStatController(FLocalPlayer InOwner) : base(InOwner)
    {
    }

    public void Handle_S_USER_DATA(in S_USER_DATA InPacket)
    {
        Name = InPacket.name;
        Level = InPacket.level;
        Exp = InPacket.exp;
        MaxExp = FDataCenter.Instance.GetIntAttribute("UserClass.Class[@class=" + InPacket.level + "]@exp");
        
        CalcCritical();

        FLobbyUserInfoUI userInfoUI = FindLobbyUserInfoUI();
        if (userInfoUI != null)
        {
            userInfoUI.InitUserInfo();
        }
    }

    private void CalcCritical()
    {
        Critical = 0;

        FDiceController diceController = FindController<FDiceController>();
        if(diceController != null)
        {
            diceController.ForeachAcquiredDice((FDice InDice) => {
                AddCritical(InDice.id, InDice.level);
            });
        }
    }

    public void AddCritical(int InID, int InIncreaseLevel = 1)
    {
        FDiceGradeData? data = FDiceDataManager.Instance.FindGradeDataByID(InID);
        if (data != null)
        {
            Critical += data.Value.Critical * InIncreaseLevel;

            FDiceInventory diceInventory = FindDiceInventoryUI();
            if (diceInventory != null)
            {
                diceInventory.Critical = Critical;
            }
        }
    }

    private FDiceInventory FindDiceInventoryUI()
    {
        return GameObject.FindObjectOfType<FDiceInventory>();
    }

    private FLobbyUserInfoUI FindLobbyUserInfoUI()
    {
        return GameObject.FindObjectOfType<FLobbyUserInfoUI>();
    }
}
