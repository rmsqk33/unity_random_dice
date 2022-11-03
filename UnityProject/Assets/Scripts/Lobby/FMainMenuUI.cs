using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMainMenuUI : MonoBehaviour
{
    [SerializeField]
    FLobbyScrollView ScrollView = null;
    [SerializeField]
    Image FocusImage = null;
    [SerializeField]
    int SelectedMenuNumber = 0;
    [SerializeField]
    float FocusMoveSec = 0f;
    

    List<FMainMenuButton> m_ButtonList = new List<FMainMenuButton>();
    int m_SelectedMenuIndex = -1;

    bool m_MoveFocus = false;
    float m_RemainFocusMoveSec = 0f;
    Vector2 m_PrecFocusPos = Vector2.zero;

    void Start()
    {
        FMainMenuButton[] buttonList = GetComponentsInChildren<FMainMenuButton>();
        m_ButtonList.AddRange(buttonList);

        SelectMenu(SelectedMenuNumber - 1);
    }

    private void Update()
    {
        if(m_MoveFocus)
        {
            m_RemainFocusMoveSec -= Time.deltaTime;

            Vector2 destPos = m_ButtonList[m_SelectedMenuIndex].Position;
            Vector2 focusPos = FocusImage.transform.position;
            if (m_RemainFocusMoveSec <= 0f)
            {
                m_MoveFocus = false;
                focusPos.x = destPos.x;
            }
            else
            {
                focusPos.x = Vector2.Lerp(destPos, m_PrecFocusPos, m_RemainFocusMoveSec / FocusMoveSec).x;
            }
            FocusImage.transform.position = focusPos;
        }
    }

    public void SelectMenu(int InIndex)
    {
        if (m_SelectedMenuIndex != -1)
            m_ButtonList[m_SelectedMenuIndex].NormalTriggerOn();

        m_SelectedMenuIndex = InIndex;
        m_ButtonList[m_SelectedMenuIndex].SelectTriggerOn();
        m_RemainFocusMoveSec = FocusMoveSec;
        m_PrecFocusPos = FocusImage.transform.position;
        m_MoveFocus = true;
    }

    public void OnClickMenu(int InIndex)
    {
        if (m_SelectedMenuIndex == InIndex)
            return;

        SelectMenu(InIndex);
        ScrollView.SelectView(InIndex);
    }
}
