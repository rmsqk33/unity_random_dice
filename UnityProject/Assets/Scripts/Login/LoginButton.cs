using Packet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButton : MonoBehaviour
{
    public void OnClick()
    {
        C_CREATE_GUEST_ACCOUNT pkt = new C_CREATE_GUEST_ACCOUNT();
        FServerManager.Instance.SendMessage(pkt);
    }
}
