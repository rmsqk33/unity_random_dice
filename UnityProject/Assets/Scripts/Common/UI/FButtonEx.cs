using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FButtonEx : Button
{
    FAllColorChanger colorChanger;
    
    protected override void Awake()
    {
        base.Awake();

        colorChanger = new FAllColorChanger(gameObject);
    }

    public void SetInteractable(bool InInteractable)
    {
        colorChanger.SetEnable(InInteractable);
        interactable = InInteractable;
    }
}
