using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FGroupMenu : MonoBehaviour
{
    [SerializeField]
    List<Button> m_TabList;
    [SerializeField]
    List<FGroupMenuBase> m_MenuList;

    int m_SelectedMenuIndex = -1;
    public int SelectedMenuIndex { get { return m_SelectedMenuIndex; } }

    private void Awake()
    {
        for (int i = 0; i < m_TabList.Count; ++i)
        {
            int index = i;
            m_TabList[i].onClick.AddListener( () => { OnClickMenu(index); });
        }
    }

    public void OnClickMenu(int InIndex)
    {
        if (m_SelectedMenuIndex == InIndex)
            return;

        SetSelectedMenu(InIndex);
    }

    public void SetSelectedMenu(int InIndex)
    {
        if (InIndex < 0 || m_TabList.Count <= InIndex)
            return;

        if (m_SelectedMenuIndex != -1)
        {
            DeactiveMenu(m_SelectedMenuIndex);
        }

        m_SelectedMenuIndex = InIndex;
        ActiveMenu(InIndex);
    }

    public FGroupMenuBase GetSelectedMenu()
    {
        return m_MenuList[m_SelectedMenuIndex];
    }

    void ActiveMenu(int InIndex)
    {
        m_TabList[InIndex].GetComponent<Animator>().SetTrigger("Selected");
        m_MenuList[InIndex].gameObject.SetActive(true);
        m_MenuList[InIndex].OnActive();
    }

    void DeactiveMenu(int InIndex)
    {
        m_TabList[InIndex].GetComponent<Animator>().SetTrigger("Normal");
        m_MenuList[InIndex].OnDeactive();
        m_MenuList[InIndex].gameObject.SetActive(false);
    }
}
