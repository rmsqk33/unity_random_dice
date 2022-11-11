using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class FInventoryMenu : FLobbyScrollMenuBase
{
    [SerializeField]
    private FGroupMenu m_Tab;
    [SerializeField]
    int m_InitMenuIndex = 0;

    public override void OnActive()
    {
        if (m_Tab.SelectedMenuIndex != m_InitMenuIndex)
            m_Tab.SetSelectedMenu(m_InitMenuIndex);

        m_Tab.GetSelectedMenu().OnActive();
    }

    public override void OnDeactive()
    {
        m_Tab.GetSelectedMenu().OnDeactive();
    }
}
