using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPopupManager : FNonObjectSingleton<FPopupManager>
{
    FPopupBase m_Popup;

    public void OpenMsgPopup(in string InTitle, in string InMsg, FMsgPopup.OKButtonFunc InFunc = null)
    {
        if (m_Popup != null)
            GameObject.Destroy(m_Popup.gameObject);

        FMsgPopup popup = null;
        GameObject gameObject = CreatePopup("Prefabs/Popup/MsgPopup");
        if (gameObject != null)
            popup = gameObject.GetComponent<FMsgPopup>();

        popup.Title = InTitle;
        popup.Message = InMsg;
        popup.OKButtonHandler = InFunc;
        m_Popup = popup;
    }

    public void OpenAcquiredDiceInfoPopup(int InID, FDiceInfoPopup.ButtonHandler InFunc, FDiceInfoPopup.ButtonHandler InFunc2)
    {
        if (m_Popup != null)
            GameObject.Destroy(m_Popup.gameObject);

        FDiceInfoPopup popup = null;
        GameObject gameObject = CreatePopup("Prefabs/Popup/DiceInfoPopup");
        if (gameObject != null)
            popup = gameObject.GetComponent<FDiceInfoPopup>();

        popup.UpgradeHandler = InFunc;
        popup.UseHandler = InFunc2;
        popup.OpenAcquiredDiceInfo(InID);
        m_Popup = popup;
    }

    public void OpenDisableDiceInfoPopup(int InID)
    {
        if (m_Popup != null)
            GameObject.Destroy(m_Popup.gameObject);

        FDiceInfoPopup popup = null;
        GameObject gameObject = CreatePopup("Prefabs/Popup/DiceInfoPopup");
        if (gameObject != null)
            popup = gameObject.GetComponent<FDiceInfoPopup>();

        popup.OpenDisableDiceInfo(InID);
        m_Popup = popup;
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

        return popup;
    }
}
