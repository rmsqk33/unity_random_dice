using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FBattleFieldInfoPopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI m_NameText;
    [SerializeField]
    Image m_BattleFieldImage;

    public delegate void ButtonHandler();
    ButtonHandler m_UpgradeBtnHandler;
    ButtonHandler m_UseBtnHandler;
    ButtonHandler m_PurchaseBtnHandler;
    
    public ButtonHandler UpgradeHandler { set { m_UpgradeBtnHandler = value; } }
    public ButtonHandler UseHandler { set { m_UseBtnHandler = value; } }
    public ButtonHandler PurchaseBtnHandler { set { m_PurchaseBtnHandler = value; } }
    
    public void OpenAcquiredBattleFieldInfo(int InID)
    {
        FBattleFieldData? data = FBattleFieldDataManager.Instance.FindBattleFieldData(InID);
        if (data == null)
            return;

        m_NameText.text = data.Value.Name;
        m_BattleFieldImage.sprite = Resources.Load<Sprite>(data.Value.SkinImage);
    }

    public void OpenNotAcquiredBattleFieldInfo(int InID)
    {
        FBattleFieldData? data = FBattleFieldDataManager.Instance.FindBattleFieldData(InID);
        if (data == null)
            return;

        m_NameText.text = data.Value.Name;
        m_BattleFieldImage.sprite = Resources.Load<Sprite>(data.Value.SkinImage);
    }

    public void OnClickUpgrade()
    {
        m_UpgradeBtnHandler();
    }

    public void OnClickUse()
    {
        m_UseBtnHandler();
    }

    public void OnClickPurchase()
    {
        m_PurchaseBtnHandler();
    }

    public void OnClose()
    {
        FPopupManager.Instance.ClosePopup();
    }
}
