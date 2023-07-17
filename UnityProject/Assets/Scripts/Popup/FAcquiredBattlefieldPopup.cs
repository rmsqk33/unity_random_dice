using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FAcquiredBattlefieldPopup : FPopupBase
{
    [SerializeField]
    Image battlefieldImage;
    [SerializeField]
    TextMeshProUGUI battlefieldName;

    public void OpenPopup(int InID)
    {
        FBattleFieldData battlefieldData = FBattleFieldDataManager.Instance.FindBattleFieldData(InID);
        if(battlefieldData != null)
        {
            battlefieldImage.sprite = Resources.Load<Sprite>(battlefieldData.skinImagePath);
            battlefieldName.text = battlefieldData.name;
        }
    }
}
