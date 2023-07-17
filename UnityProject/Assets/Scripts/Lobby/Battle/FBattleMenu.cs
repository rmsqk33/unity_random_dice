using Packet;
using TMPro;
using UnityEngine;

public class FBattleMenu : FLobbyScrollMenuBase
{
    [SerializeField]
    FButtonEx cardButton = null;
    [SerializeField]
    TextMeshProUGUI card = null;

    int cardPrice;

    public int Card 
    { 
        set 
        { 
            card.text = value + "/" + cardPrice;
            cardButton.SetInteractable(cardPrice < value);
        }
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        FStoreBoxData battleCardBox = FStoreDataManager.Instance.FindStoreBoxData(FStoreDataManager.Instance.BattleCardBoxID);
        if (battleCardBox != null)
        {
            cardPrice = battleCardBox.cardPrice;
        }

        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if (inventoryController != null)
        {
            Card = inventoryController.Card;
        }
    }

    public void OnClickOpenBox()
    {
        FPopupManager.Instance.OpenBoxPurchasePopup(FStoreDataManager.Instance.BattleCardBoxID);
    }

    public void OnClickCoopBattleMatching()
    {
        FMatchingMananger.Instance.RequestMatching();
    }
}
