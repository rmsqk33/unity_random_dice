using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FLobbyMenuUI : MonoBehaviour
{
    [SerializeField]
    FLobbyScrollView scrollView = null;
    [SerializeField]
    Image focusImage = null;
    [SerializeField]
    int selectedMenuIndex = 0;
    [SerializeField]
    float focusMoveSec = 0f;
    [SerializeField]
    List<FLobbyMenuButton> menuList;

    bool moveFocus = false;
    float remainFocusMoveSec = 0f;
    Vector2 precFocusPos = Vector2.zero;

    void Start()
    {
        SelectMenu(selectedMenuIndex);
    }

    private void Update()
    {
        if(moveFocus)
        {
            remainFocusMoveSec -= Time.deltaTime;

            Vector2 destPos = menuList[selectedMenuIndex].Position;
            Vector2 focusPos = focusImage.transform.position;
            if (remainFocusMoveSec <= 0f)
            {
                moveFocus = false;
                focusPos.x = destPos.x;
            }
            else
            {
                focusPos.x = Vector2.Lerp(destPos, precFocusPos, remainFocusMoveSec / focusMoveSec).x;
            }
            focusImage.transform.position = focusPos;
        }
    }

    public void SelectMenu(int InIndex)
    {
        if (selectedMenuIndex != -1)
            menuList[selectedMenuIndex].NormalTriggerOn();

        selectedMenuIndex = InIndex;
        menuList[selectedMenuIndex].SelectTriggerOn();
        remainFocusMoveSec = focusMoveSec;
        precFocusPos = focusImage.transform.position;
        moveFocus = true;
    }

    public void OnClickMenu(int InIndex)
    {
        if (selectedMenuIndex == InIndex)
            return;

        SelectMenu(InIndex);
        scrollView.SelectView(InIndex);
    }
}
