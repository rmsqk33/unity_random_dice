using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    [SerializeField]
    float PreWorkMinSec = 0f; // ���� �۾��� UI �ּ� ǥ�ýð�
    [SerializeField]
    Button LoginButton;
    [SerializeField]
    GameObject LoadingUI;
    [SerializeField]
    TextMeshProUGUI LoadingMsg;

    delegate bool PreWorkFunc();
    Dictionary<string, PreWorkFunc> m_PreWorkMap = new Dictionary<string, PreWorkFunc>();

    delegate void MainThreadFunc();
    List<MainThreadFunc> m_MainThreadFuncQueue = new List<MainThreadFunc>();

    void Start()
    {
        AddPreWork("serverConnect", FServerManager.Instance.ConnectServer);
        AddPreWork("login", FAccountMananger.Instance.TryLogin);

        LoadPreWork();
    }

    void Update()
    {
        if (0 < m_MainThreadFuncQueue.Count)
        {
            m_MainThreadFuncQueue[0]();
            m_MainThreadFuncQueue.RemoveAt(0);
        }
    }

    void AddPreWork(string InMsg, PreWorkFunc InFunc)
    {
        m_PreWorkMap.Add(InMsg, InFunc);
    }

    void AddMainThreadFunc(MainThreadFunc InFunc)
    {
        m_MainThreadFuncQueue.Add(InFunc);
    }

    void LoadPreWork()
    {
        if (m_PreWorkMap.Count == 0)
            return;

        string preWorkType = m_PreWorkMap.Keys.First();
        FDataNode node = FDataCenter.Instance.GetDataNodeWithQuery("PreWorkList.PreWork@type=" + preWorkType);
        LoadingMsg.text = node.GetStringAttr("loadingMsg");

        PreWorkFunc func = m_PreWorkMap.Values.First();
        Thread thread = new Thread(() =>
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            if (func())
            {
                while (watch.ElapsedMilliseconds < PreWorkMinSec * 1000) ;
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
        m_PreWorkMap.Remove(m_PreWorkMap.Keys.First());
        if (0 < m_PreWorkMap.Count)
            LoadPreWork();
        else
        {
           //LoginButton.gameObject.SetActive(true);
           //LoadingUI.SetActive(false);
        }
    }

    void OnFailPreWork()
    {
        string preWorkType = m_PreWorkMap.Keys.First();
        FDataNode node = FDataCenter.Instance.GetDataNodeWithQuery("PreWorkList.PreWork@type=" + preWorkType);
        
        string title = node.GetStringAttr("errorTitle");
        string msg = node.GetStringAttr("errorMsg");
        FPopupManager.Instance.OpenMsgPopup(title, msg, () => {
            Application.Quit();     
        });

        LoadingUI.SetActive(false);
    }
}
