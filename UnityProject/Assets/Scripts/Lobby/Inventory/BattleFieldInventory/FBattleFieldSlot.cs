using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FBattleFieldSlot : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    Image skinImage;

    public void Init(in FBattleFieldData InData)
    {
        nameText.text = InData.name;
        skinImage.sprite = Resources.Load<Sprite>(InData.skinImagePath);
    }
}
