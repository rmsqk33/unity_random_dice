using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FMsgPopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI TitleTextMesh;
    [SerializeField]
    TextMeshProUGUI MsgTextMesh;

    public delegate void OKButtonFunc();
    OKButtonFunc m_OKButtonHandler = null;

    public string Title { set { TitleTextMesh.text = value; } }
    public string Message { set { MsgTextMesh.text = value; } }
    public bool Visible { set { foreach (Transform child in transform) child.gameObject.SetActive(value); } }

    public OKButtonFunc OKButtonHandler { set { m_OKButtonHandler = value; } }

    public void OnClickOK()
    {
        if (m_OKButtonHandler != null)
            m_OKButtonHandler();

        FPopupManager.Instance.ClosePopup();
    }

}
