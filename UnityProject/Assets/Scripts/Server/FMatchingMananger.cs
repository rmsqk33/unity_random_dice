
using Packet;
using System;
using System.Threading.Tasks;

public class FMatchingMananger : FNonObjectSingleton<FMatchingMananger>
{
    public void RequestMatching()
    {
        FPopupManager.Instance.OpenBattleMatchingPopup();

        C_BATTLE_MATCHING packet = new C_BATTLE_MATCHING();
        FServerManager.Instance.SendMessage(packet);
    }

    public void CancelMatching()
    {
        C_BATTLE_MATCHING_CANCEL packet = new C_BATTLE_MATCHING_CANCEL();
        FServerManager.Instance.SendMessage(packet);

        FPopupManager.Instance.ClosePopup();
    }

    public async void MatchingCompleteHost()
    {
        var connectTask = Task.Run(() => FServerManager.Instance.OpenP2PServer());
        bool connected = await connectTask;
        if(connected)
        {
            FSceneManager.Instance.ChangeSceneAfterLoading(FEnum.SceneType.Battle);
        }
        else
        {
            RequestMatching();
        }
    }

    public async void MatchingCompleteClient(string InIP)
    {
        var connectTask = Task.Run(() => FServerManager.Instance.ConnectP2PServer(InIP));
        bool connected = await connectTask;
        if (connected)
        {
            FSceneManager.Instance.ChangeSceneAfterLoading(FEnum.SceneType.Battle);
        }
        else
        {
            RequestMatching();
        }
    }
}
