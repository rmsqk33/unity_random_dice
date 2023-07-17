using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;

public class FLoginScene : MonoBehaviour
{
    [SerializeField]
    float preWorkMinSec = 0f;
    [SerializeField]
    GameObject loadingUI;
    [SerializeField]
    TextMeshProUGUI loadingMsg;

    delegate bool PreWorkFunc();
    Dictionary<string, PreWorkFunc> preWorkMap = new Dictionary<string, PreWorkFunc>();

    delegate void MainThreadFunc();
    List<MainThreadFunc> mainThreadFuncQueue = new List<MainThreadFunc>();

    void Start()
    {
        AddPreWork("serverConnect", FServerManager.Instance.ConnectMainServer);
        AddPreWork("login", FAccountMananger.Instance.TryLogin);

        LoadPreWork();
    }

    void Update()
    {
        if (0 < mainThreadFuncQueue.Count && mainThreadFuncQueue[0] != null)
        {
            mainThreadFuncQueue[0]();
            mainThreadFuncQueue.RemoveAt(0);
        }
    }

    void AddPreWork(string InMsg, PreWorkFunc InFunc)
    {
        preWorkMap.Add(InMsg, InFunc);
    }

    void AddMainThreadFunc(MainThreadFunc InFunc)
    {
        mainThreadFuncQueue.Add(InFunc);
    }

    void LoadPreWork()
    {
        if (preWorkMap.Count == 0)
            return;

        string preWorkType = preWorkMap.Keys.First();
        FDataNode node = FDataCenter.Instance.GetDataNodeWithQuery("PreWorkList.PreWork@type=" + preWorkType);
        loadingMsg.text = node.GetStringAttr("loadingMsg");

        PreWorkFunc func = preWorkMap.Values.First();
        Thread thread = new Thread(() =>
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            if (func())
            {
                while (watch.ElapsedMilliseconds < preWorkMinSec * 1000) ;
                AddMainThreadFunc(OnCompletePreWork);
            }
            else
            {
                AddMainThreadFunc(OnFailPreWork);
            }
            watch.Stop();
        });
        thread.Start();
    }

    void OnCompletePreWork()
    {
        preWorkMap.Remove(preWorkMap.Keys.First());
        if (0 < preWorkMap.Count)
            LoadPreWork();
    }

    void OnFailPreWork()
    {
        string preWorkType = preWorkMap.Keys.First();
        FDataNode node = FDataCenter.Instance.GetDataNodeWithQuery("PreWorkList.PreWork@type=" + preWorkType);
        
        string title = node.GetStringAttr("errorTitle");
        string msg = node.GetStringAttr("errorMsg");
        FPopupManager.Instance.OpenMsgPopup(title, msg, () => {
            Application.Quit();     
        });

        loadingUI.SetActive(false);
    }
}
