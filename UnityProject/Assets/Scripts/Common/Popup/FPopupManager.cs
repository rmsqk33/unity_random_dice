using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPopupManager : FNonObjectSingleton<FPopupManager>
{
    FMsgPopup m_MsgPopup = null;
    
    public void OpenMsgPopup(in string InTitle, in string InMsg, FMsgPopup.OKButtonFunc InFunc = null)
    {
        if (m_MsgPopup == null)
            m_MsgPopup = CreateMsgPopup();

        if (m_MsgPopup == null)
            return;

        m_MsgPopup.Title = InTitle;
        m_MsgPopup.Message = InMsg;
        m_MsgPopup.OKButtonHandler = InFunc;
        m_MsgPopup.Visible = true;
    }

    FMsgPopup CreateMsgPopup()
    {
        GameObject popup = Resources.Load<GameObject>("Prefabs/Popup/MsgPopup");
        if (popup == null)
            return null;

        GameObject canvas = GameObject.Find("UI");
        if (canvas == null)
        {
            GameObject.Destroy(popup);
            return null;
        }

        popup = GameObject.Instantiate(popup, canvas.transform);

        return popup.GetComponent<FMsgPopup>();
    }
}
