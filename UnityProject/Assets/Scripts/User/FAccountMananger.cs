using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Packet;
using UnityEditor.Experimental.GraphView;
using UnityEngine.SceneManagement;

public class FAccountMananger : FNonObjectSingleton<FAccountMananger>
{
    string ACCOUNT_DATA_PATH = Application.dataPath + "account_data.json";

    [Serializable]
    public class AccountData
    {
        public string id;
    }

    public bool TryLogin()
    {
        if (File.Exists(ACCOUNT_DATA_PATH))
            RequestLogin();
        else
            CreateAccount();

        return true;
    }

    public void Handle_S_GUEST_LOGIN(bool InResult)
    {
        if(InResult)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            CreateAccount();

            //FPopupManager.Instance.OpenMsgPopup("로그인 실패", "로그인 정보가 잘못되었습니다.", () =>
            //{
            //    Application.Quit();
            //});
        }
    }

    public void Handle_S_CREATE_GUEST_ACCOUNT(string InID)
    {
        SaveAccountData(InID);
        RequestLogin();
    }

    private void RequestLogin()
    {
        AccountData accountData = new AccountData();

        string loadJsonStr = File.ReadAllText(ACCOUNT_DATA_PATH);
        accountData = JsonUtility.FromJson<AccountData>(loadJsonStr);

        C_GUEST_LOGIN pkt = new C_GUEST_LOGIN();
        pkt.id = accountData.id;

        FServerManager.Instance.SendMessage(pkt);
    }

    private void SaveAccountData(string InID)
    {
        AccountData accountData = new AccountData();
        accountData.id = InID;

        string saveJsonStr = JsonUtility.ToJson(accountData);
        File.WriteAllText(ACCOUNT_DATA_PATH, saveJsonStr);
    }

    private void CreateAccount()
    {
        C_CREATE_GUEST_ACCOUNT pkt = new C_CREATE_GUEST_ACCOUNT();

        FServerManager.Instance.SendMessage(pkt);
    }
}
