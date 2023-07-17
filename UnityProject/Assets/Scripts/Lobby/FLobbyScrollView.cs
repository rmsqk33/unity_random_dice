using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FLobbyScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    FLobbyMenuUI mainMenu = null;
    [SerializeField]
    float viewChangeDistance;
    [SerializeField]
    float scrollSec;
    [SerializeField]
    int initViewIndex;
    [SerializeField]
    List<FLobbyScrollMenuBase> scrollMenuList;

    ScrollRect scrollRect = null;
    Vector2 prevViewPosition = Vector2.zero;
    Vector2 moveViewPosition = Vector2.zero;
    float dragStartX = 0f;
    float dragDeltaTime = 0f;
    int currentViewIndex = -1;
    bool dragging = false;

    void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    void Start()
    {
        SelectView(initViewIndex, true);
    }

    void Update()
    {
        if(dragging)
        {
            dragDeltaTime -= Time.deltaTime;
            if (dragDeltaTime <= 0f)
            {
                scrollRect.content.anchoredPosition = moveViewPosition;
                dragging = false;
            }
            else
            {
                Vector2 newPos = Vector2.Lerp(moveViewPosition, prevViewPosition, dragDeltaTime / scrollSec);
                scrollRect.content.anchoredPosition = newPos;
            }
        }
    }

    public void SelectView(int InIndex, bool immediately=false)
    {
        if(0 <= InIndex && InIndex < scrollMenuList.Count)
        {
            if(currentViewIndex != InIndex)
            {
                if (currentViewIndex != -1)
                    scrollMenuList[currentViewIndex].OnDeactive();

                scrollMenuList[InIndex].OnActive();
            }

            currentViewIndex = InIndex;
            moveViewPosition.x = scrollMenuList[InIndex].GetComponent<RectTransform>().anchoredPosition.x * -1;
            if (immediately)
            {
                scrollRect.content.anchoredPosition = moveViewPosition;
            }
            else
            {
                prevViewPosition.x = scrollRect.content.anchoredPosition.x;
                dragging = true;
                dragDeltaTime = scrollSec;
            }
        }
    }

    public void OnBeginDrag(PointerEventData InData)
    {
        dragStartX = InData.position.x;
        dragging = false;
    }

    public void OnEndDrag(PointerEventData InData)
    {
        // ��ũ�� �Ÿ� ����, ����� ���� �޴���, ������ ���� �޴��� ����
        float dragDistance = InData.position.x - dragStartX;

        // ������ ��ŭ ��ũ�� �� �ٸ� �޴��� ����, �޴��� �Ѿ�ų� ��ũ�ѷ��� ���ڸ� ��� ���� �޴� ��ġ�� ���ư���.
        int changeViewIndex = -1;
        if (viewChangeDistance <= Mathf.Abs(dragDistance))
            changeViewIndex = Mathf.Clamp(dragDistance > 0 ? currentViewIndex - 1 : currentViewIndex + 1, 0, scrollMenuList.Count - 1);

        if (changeViewIndex != -1 && changeViewIndex != currentViewIndex)
            mainMenu.SelectMenu(changeViewIndex);

        SelectView(changeViewIndex == -1 ? currentViewIndex : changeViewIndex);
    }
}
