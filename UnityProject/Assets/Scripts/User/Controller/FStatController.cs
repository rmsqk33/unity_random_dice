using Packet;
using UnityEngine;

public class FStatController : FControllerBase
{
    public int Level { get; set; }
    public int Exp { get; set; }
    public int MaxExp { get; set; }
    public int Critical { get; set; }
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
                FDiceGradeData? data = FDiceDataManager.Instance.FindGradeDataByID(InDice.id);
                if (data != null)
                {
                    Critical += data.Value.Critical * InDice.level;
                }
            });
        }
    }

    private FLobbyUserInfoUI FindLobbyUserInfoUI()
    {
        return GameObject.FindObjectOfType<FLobbyUserInfoUI>();
    }
}
