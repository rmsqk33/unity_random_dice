using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FEnum;

public class FNotAcquiredDiceSlot : MonoBehaviour
{
    [SerializeField]
    Image background;
    [SerializeField]
    FDiceImage diceImage;
    [SerializeField]
    TextMeshProUGUI gradeText;

    public int ID { get; set; }

    public void Init(in FDiceData InData)
    {
        ID = InData.id;
        diceImage.SetNotAcquiredDice(InData);
        
        FDiceGradeData gradeData = FDiceDataManager.Instance.FindGradeData(InData.grade);
        if (gradeData != null)
        {
            background.sprite = Resources.Load<Sprite>(gradeData.backgroundPath);
            gradeText.text = gradeData.gradeName;
        }
    }

    public void OnClickSlot()
    {
        FPopupManager.Instance.OpenNotAcquiredDiceInfoPopup(ID);
    }
}
