using TMPro;
using UnityEngine;

public class FMsgPopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI title;
    [SerializeField]
    TextMeshProUGUI message;

    public delegate void OKButtonFunc();
    OKButtonFunc okButtonHandler = null;

    public string Title { set { title.text = value; } }
    public string Message { set { message.text = value; } }

    public OKButtonFunc OKButtonHandler { set { okButtonHandler = value; } }

    public void OnClickOK()
    {
        if (okButtonHandler != null)
            okButtonHandler();

        FPopupManager.Instance.ClosePopup();
    }

}
