using UnityEngine;

public class FInventoryMenu : FLobbyScrollMenuBase
{
    [SerializeField]
    FInventoryTabUI tabUI;
    [SerializeField]
    int initTabIndex = 0;

    public override void OnActive()
    {
        if (tabUI.SelectedTabIndex != initTabIndex)
            tabUI.SetSelectedTab(initTabIndex);
    }

    public override void OnDeactive()
    {
        FDiceInventory diceInventory = FindDiceInventory();
        if (diceInventory != null)
        {
            diceInventory.OnDeactive();
        }

        FBattleFieldInventory battlefieldInventory = FindBattlefieldInventory();
        if (battlefieldInventory != null)
        {
            battlefieldInventory.OnDeactive();
        }
    }

    FDiceInventory FindDiceInventory()
    {
        return FUIManager.Instance.FindUI<FDiceInventory>();
    }

    FBattleFieldInventory FindBattlefieldInventory()
    {
        return FUIManager.Instance.FindUI<FBattleFieldInventory>();
    }
}
