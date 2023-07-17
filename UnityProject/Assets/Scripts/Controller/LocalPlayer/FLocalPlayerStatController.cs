using FEnum;
using Packet;
using UnityEngine;

public class FLocalPlayerStatController : FControllerBase
{
    private int level;
    private int exp;
    private int maxExp;
    private int critical;
    private string name;

    public int Level { get { return level; } }
    public int Exp { get { return exp; } }
    public int MaxExp { get { return maxExp; } }
    public int Critical { get { return critical; } }
    public float CriticalDamageRate { get { return critical / 100.0f; } }
    public string Name { get { return name; } }

    public FLocalPlayerStatController(FLocalPlayer InOwner) : base(InOwner)
    {
    }

    public void Handle_S_USER_DATA(in S_USER_DATA InPacket)
    {
        name = InPacket.name;
        exp = InPacket.exp;
        SetLevel(InPacket.level);
        
        CalcCritical();

        FLobbyUserInfoUI userInfoUI = FindLobbyUserInfoUI();
        if (userInfoUI != null)
        {
            userInfoUI.Initialize();
        }

        if(Name.Length == 0)
        {
            FPopupManager.Instance.OpenNamePopup();
        }
    }

    public void Handle_S_CHANGE_EXP(in S_CHANGE_EXP InPacket)
    {
        exp = InPacket.exp;

        FLobbyUserInfoUI ui = FindLobbyUserInfoUI();
        if (ui != null)
        {
            ui.Exp = exp;
        }
    }

    public void Handle_S_CHANGE_LEVEL(in S_CHANGE_LEVEL InPacket)
    {
        SetLevel(InPacket.level);

        FLobbyUserInfoUI ui = FindLobbyUserInfoUI();
        if (ui != null)
        {
            ui.Level = level;
            ui.MaxExp = maxExp;
        }
    }

    public void Handle_S_CHANGE_NAME(in S_CHANGE_NAME InPacket)
    {
        ChangeNameResult result = (ChangeNameResult)InPacket.resultType;
        if (result == ChangeNameResult.CHANGE_NAME_RESULT_SUCCESS)
        {
            name = InPacket.name;

            FLobbyUserInfoUI userInfoUI = FindLobbyUserInfoUI();
            if (userInfoUI != null)
            {
                userInfoUI.Name = name;
            }

            FPopupManager.Instance.ClosePopup();
        }
        else
        {
            FNamePopup popup = FUIManager.Instance.FindUI<FNamePopup>();
            if (popup != null)
            {
                string errorMessage = new string("");
                switch (result)
                {
                    case ChangeNameResult.CHANGE_NAME_RESULT_ALEADY: errorMessage = "�̹� �ִ� �г����Դϴ�"; break;
                    case ChangeNameResult.CHANGE_NAME_RESULT_SPECIAL_CHARACTER: errorMessage = "Ư�����ڴ� ����� �� �����ϴ�"; break;
                    case ChangeNameResult.CHANGE_NAME_RESULT_BLANK: errorMessage = "�̸��� �Է��ϼ���"; break;
                }

                popup.ErrorMessage = errorMessage;
            }
        }
    }

    public void AddCritical(int InID, int InIncreaseLevel = 1)
    {
        FDiceGradeData data = FDiceDataManager.Instance.FindGradeDataByID(InID);
        if (data != null)
        {
            critical += data.critical * InIncreaseLevel;

            FDiceInventory diceInventory = FindDiceInventoryUI();
            if (diceInventory != null)
            {
                diceInventory.Critical = critical;
            }
        }
    }

    void CalcCritical()
    {
        critical = 0;

        FDiceController diceController = FindController<FDiceController>();
        if(diceController != null)
        {
            diceController.ForeachAcquiredDice((FDice InDice) => {
                AddCritical(InDice.id, InDice.level);
            });
        }
    }

    void SetLevel(int InLevel)
    {
        level = InLevel;
        maxExp = FDataCenter.Instance.GetIntAttribute("UserClass.Class[@class=" + level + "]@exp");
    }

    FDiceInventory FindDiceInventoryUI()
    {
        return FUIManager.Instance.FindUI<FDiceInventory>();
    }

    FLobbyUserInfoUI FindLobbyUserInfoUI()
    {
        return FUIManager.Instance.FindUI<FLobbyUserInfoUI>();
    }
}
