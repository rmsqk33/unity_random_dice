using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FBattleFieldInfoPopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    Image battleFieldImage;
    [SerializeField]
    GameObject useBtn;
    [SerializeField]
    GameObject purchaseBtn;
    [SerializeField]
    TextMeshProUGUI price;

    int battlefieldID;
    
    public void OpenPopup(int InID)
    {
        battlefieldID = InID;

        FBattleFieldData data = FBattleFieldDataManager.Instance.FindBattleFieldData(InID);
        if (data == null)
            return;

        FBattlefieldController battlefieldController = FLocalPlayer.Instance.FindController<FBattlefieldController>();
        if (battlefieldController == null)
            return;

        nameText.text = data.name;
        battleFieldImage.sprite = Resources.Load<Sprite>(data.skinImagePath);

        bool isAcquired = battlefieldController.IsAcquiredBattleField(InID);

        useBtn.SetActive(isAcquired);
        purchaseBtn.SetActive(isAcquired == false);

        if (isAcquired == false)
            price.text = data.price.ToString();
    }

    public void OnClickUse()
    {
        FPresetController presetController = FLocalPlayer.Instance.FindController<FPresetController>();
        if (presetController != null)
        {
            presetController.SetBattleFieldPreset(battlefieldID);
        }
        FPopupManager.Instance.ClosePopup();
    }

    public void OnClickPurchase()
    {
        FBattlefieldController battlefieldController = FLocalPlayer.Instance.FindController<FBattlefieldController>();
        if(battlefieldController != null)
        {
            battlefieldController.RequestPurchaseBattlefield(battlefieldID);
        }
        FPopupManager.Instance.ClosePopup();
    }

    public void OnClose()
    {
        FPopupManager.Instance.ClosePopup();
    }
}
