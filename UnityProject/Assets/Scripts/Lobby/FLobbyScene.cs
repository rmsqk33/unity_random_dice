using UnityEngine;

public class FLobbyScene : MonoBehaviour
{
    private void Awake()
    {
#if DEBUG
        if(!FServerManager.Instance.IsConnected)
        {
            FServerManager.Instance.ConnectMainServer();
            FAccountMananger.Instance.TryLogin();
        }
#endif
    }
}
