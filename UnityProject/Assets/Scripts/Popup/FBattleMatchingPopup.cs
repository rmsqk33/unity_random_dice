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
        string format = "����ð� : ";
        if (0 < timer.Hours)
            format += "h�� ";

        if (0 < timer.Minutes)
            format += "m�� ";

        format += "s��";

        elapsedTimeText.text = timer.ToString(format);
    }

    public void OnClickCancel()
    {
        FMatchingMananger.Instance.CancelMatching();
    }
}
