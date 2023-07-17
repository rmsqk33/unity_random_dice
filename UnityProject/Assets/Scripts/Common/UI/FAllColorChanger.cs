
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FAllColorChanger
{
    SpriteRenderer[] spriteList;
    List<Color> spriteOriginColorList = new List<Color>();

    Image[] imageList;
    List<Color> imageOriginColorList = new List<Color>();

    TextMeshProUGUI[] textList;
    List<Color> textOriginColorList = new List<Color>();

    Color disableColor = new Color(0.5f, 0.5f, 0.5f);
    bool enable = true;

    public FAllColorChanger(GameObject InOwner)
    {
        spriteList = InOwner.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteList)
        {
            spriteOriginColorList.Add(sprite.color);
        }

        imageList = InOwner.GetComponentsInChildren<Image>();
        foreach(Image image in imageList)
        {
            imageOriginColorList.Add(image.color);
        }

        textList = InOwner.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in textList)
        {
            textOriginColorList.Add(text.color);
        }
    }

    public void SetEnable(bool InEnable)
    {
        if (enable == InEnable)
            return;

        enable = InEnable;
        for (int i = 0; i < spriteList.Length; ++i)
        {
            spriteList[i].color = enable ? spriteOriginColorList[i] : disableColor;
        }

        for (int i = 0; i < imageList.Length; ++i)
        {
            imageList[i].color = enable ? imageOriginColorList[i] : disableColor;
        }

        for (int i = 0; i < textList.Length; ++i)
        {
            textList[i].color = enable ? textOriginColorList[i] : disableColor;
        }
    }
}
