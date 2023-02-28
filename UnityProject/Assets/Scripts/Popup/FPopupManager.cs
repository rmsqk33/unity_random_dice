using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPopupManager : FNonObjectSingleton<FPopupManager>
{
    FPopupBase m_Popup;

    public void OpenMsgPopup(in string InTitle, in string InMsg, FMsgPopup.OKButtonFunc InFunc = null)
    {
        FMsgPopup popup = null;
        GameObject gameObject = CreatePopup("Prefabs/Popup/MsgPopup");
        if (gameObject != null)
            popup = gameObject.GetComponent<FMsgPopup>();

        popup.Title = InTitle;
        popup.Message = InMsg;
        popup.OKButtonHandler = InFunc;
    }

    public void OpenAcquiredDiceInfoPopup(int InID, FDiceInfoPopup.ButtonHandler InFunc, FDiceInfoPopup.ButtonHandler InFunc2)
    {
        FDiceInfoPopup popup = null;
        GameObject gameObject = CreatePopup("Prefabs/Popup/DiceInfoPopup");
        if (gameObject != null)
            popup = gameObject.GetComponent<FDiceInfoPopup>();

        popup.UpgradeHandler = InFunc;
        popup.UseHandler = InFunc2;
        popup.OpenAcquiredDiceInfo(InID);
    }

    public void OpenNotAcquiredDiceInfoPopup(int InID)
    {
        FDiceInfoPopup popup = null;
        GameObject gameObject = CreatePopup("Prefabs/Popup/DiceInfoPopup");
        if (gameObject != null)
            popup = gameObject.GetComponent<FDiceInfoPopup>();

        popup.OpenNotAcquiredDiceInfo(InID);
    }

    public void OpenAcquiredBattleFieldInfoPopup(int InID, FBattleFieldInfoPopup.ButtonHandler InFunc, FBattleFieldInfoPopup.ButtonHandler InFunc2)
    {
        FBattleFieldInfoPopup popup = null;
        GameObject gameObject = CreatePopup("Prefabs/Popup/BattleFieldInfoPopup");
        if (gameObject != null)
            popup = gameObject.GetComponent<FBattleFieldInfoPopup>();

        popup.UpgradeHandler = InFunc;
        popup.UseHandler = InFunc2;
        popup.OpenAcquiredBattleFieldInfo(InID);
    }

    public void OpenNotAcquiredBattleFieldInfoPopup(int InID, FBattleFieldInfoPopup.ButtonHandler InFunc)
    {
        FBattleFieldInfoPopup popup = null;
        GameObject gameObject = CreatePopup("Prefabs/Popup/BattleFieldInfoPopup");
        if (gameObject != null)
            popup = gameObject.GetComponent<FBattleFieldInfoPopup>();

        popup.PurchaseBtnHandler = InFunc;
        popup.OpenNotAcquiredBattleFieldInfo(InID);
    }

    public void OpenDicePurchasePopup(int InID, FDicePurchasePopup.ButtonHandler InFunc)
    {
        FDicePurchasePopup popup = null;
        GameObject gameObject = CreatePopup("Prefabs/Popup/DicePurchasePopup");
        if (gameObject != null)
            popup = gameObject.GetComponent<FDicePurchasePopup>();

        popup.PurchaseBtnHandler = InFunc;
        popup.OpenPopup(InID);
    }

    public void OpenBoxPurchasePopup(int InID, FBoxPurchasePopup.ButtonHandler InFunc)
    {
        FBoxPurchasePopup popup = null;
        GameObject gameObject = CreatePopup("Prefabs/Popup/BoxPurchasePopup");
        if (gameObject != null)
            popup = gameObject.GetComponent<FBoxPurchasePopup>();

        popup.PurchaseBtnHandler = InFunc;
        popup.OpenPopup(InID);
    }

    public void OpenAcquiredDicePopup(List<KeyValuePair<int, int>> InDiceList)
    {
        FAcquiredDicePopup popup = null;
        GameObject gameObject = CreatePopup("Prefabs/Popup/AcquiredDicePopup");
        if (gameObject != null)
            popup = gameObject.GetComponent<FAcquiredDicePopup>();

        popup.OpenPopup(InDiceList);
    }

    public void ClosePopup()
    {
        if(m_Popup != null)
        {
            m_Popup.Close();
            m_Popup = null;
        }
    }

    GameObject CreatePopup(in string InPath)
    {
        if (m_Popup != null)
            GameObject.Destroy(m_Popup.gameObject);

        GameObject popup = Resources.Load<GameObject>(InPath);
        if (popup == null)
            return null;

        GameObject canvas = GameObject.Find("UI");
        if (canvas == null)
        {
            GameObject.Destroy(popup);
            return null;
        }

        popup = GameObject.Instantiate(popup, canvas.transform);
        m_Popup = popup.GetComponent<FPopupBase>();

        return popup;
    }
}
