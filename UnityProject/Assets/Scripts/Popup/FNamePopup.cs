using TMPro;
using UnityEngine;
using Packet;
using System.Text.RegularExpressions;

public class FNamePopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI errorMessage;

    string NameText { get { return nameText.text; } }
    public string ErrorMessage 
    {
        set 
        {
            errorMessage.text = value;

            Animator animator = errorMessage.GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger("Emphasize");
        } 
        get { return errorMessage.text; } 
    }

    public void OpenPopup()
    {
        ErrorMessage = "";
    }

    public void OnClickOK()
    {
        string inputText = NameText.Remove(NameText.Length - 1);
        if(inputText.Length == 0)
        {
            ErrorMessage = "�̸��� �Է��ϼ���";
            return;
        }

        Match match = Regex.Match(inputText, "^[0-9a-zA-Z��-�R]+$");
        if (match.Success == false)
        {
            ErrorMessage = "Ư�����ڴ� ����� �� �����ϴ�";
            return;
        }

        C_CHANGE_NAME packet = new C_CHANGE_NAME();
        packet.name = inputText;

        FServerManager.Instance.SendMessage(packet);
    }
}
