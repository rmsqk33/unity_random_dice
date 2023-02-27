using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLobbyScene : MonoBehaviour
{
    private void Awake()
    {
#if DEBUG
        if(!FServerManager.Instance.IsConnectedServer)
        {
            FServerManager.Instance.ConnectServer();
            FAccountMananger.Instance.TryLogin();
        }
#endif
    }
}
