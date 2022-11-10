using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FBattleFieldSlot : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_NameText;
    [SerializeField]
    Image m_SkinImage;

    public void Init(in FBattleFieldData InData)
    {
        m_NameText.text = InData.Name;
        m_SkinImage.sprite = Resources.Load<Sprite>(InData.SkinImage);
    }
}
