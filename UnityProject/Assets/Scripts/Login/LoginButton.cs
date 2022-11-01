using Packet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginButton : MonoBehaviour
{
    public void OnClick()
    {
        S_ADD_GUEST_ACCOUNT pkt = new S_ADD_GUEST_ACCOUNT();
        FServerManager.Instance.SendMessage(pkt);
    }
}
