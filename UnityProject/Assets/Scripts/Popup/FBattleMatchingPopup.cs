using UnityEngine;
using Packet;
using TMPro;

public class FBattleMatchingPopup : FPopupBase
{
    [SerializeField]
    TextMeshProUGUI elapsedTimeText;

    FTimer timer = new FTimer(1);

    public void OpenPopup()
    {
        timer.Start();
     
        UpdateElapsedTimeText();
    }

    private void Update()
    {
        if(timer.IsElapsedCheckTime())
        {
            UpdateElapsedTimeText();
            timer.Interval++;
        }
    }

    private void UpdateElapsedTimeText()
    {
        string format = "경과시간 : ";
        if (0 < timer.Hours)
            format += "h시 ";

        if (0 < timer.Minutes)
            format += "m분 ";

        format += "s초";

        elapsedTimeText.text = timer.ToString(format);
    }

    public void OnClickCancel()
    {
        FMatchingMananger.Instance.CancelMatching();
    }
}
