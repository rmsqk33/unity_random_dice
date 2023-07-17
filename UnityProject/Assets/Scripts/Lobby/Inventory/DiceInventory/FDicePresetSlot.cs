using FEnum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FDicePresetSlot : MonoBehaviour
{
    [SerializeField]
    FDiceImage diceImage;
    [SerializeField]
    TextMeshProUGUI level;
    [SerializeField]
    Image presetRegistGuideArrow;

    public void SetSlot(int InDiceID)
    {
        FDiceController diceController = FLocalPlayer.Instance.FindController<FDiceController>();
        if (diceController == null)
            return;

        FDice dice = diceController.FindAcquiredDice(InDiceID);
        if (dice == null)
            return;

        diceImage.SetImage(InDiceID);
        level.text = dice.level.ToString();
    }


    public void SetPresetRegistActive(bool InActive)
    {
        presetRegistGuideArrow.gameObject.SetActive(InActive);
        GetComponent<Button>().interactable = InActive;
    }
}
