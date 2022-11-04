using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FLobbyScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    FMainMenuUI MainMenu = null;
    [SerializeField]
    float ViewChangeDistance; // 해당 거리이상 드래그 시 메뉴 변경
    [SerializeField]
    float ScrollSec; // 스크롤 시간
    [SerializeField]
    int InitViewNumber; // 초기 메뉴, 제일 왼쪽이 1
    [SerializeField]
    List<Vector2> ViewPositionList; // 메뉴별 스크롤 위치

    ScrollRect m_ScrollRect = null;
    Vector2 m_PrevViewPosition = Vector2.zero;
    float m_DragStartX = 0f;
    float m_DragDeltaTime = 0f;
    int m_CurrentViewIndex = 0;
    bool m_Dragging = false;

    void Start()
    {
        m_CurrentViewIndex = InitViewNumber - 1;
        m_ScrollRect = GetComponent<ScrollRect>();
        m_ScrollRect.content.anchoredPosition = ViewPositionList[m_CurrentViewIndex];
    }

    void Update()
    {
        if(m_Dragging)
        {
            m_DragDeltaTime -= Time.deltaTime;
            Vector2 destPos = ViewPositionList[m_CurrentViewIndex];
            if (m_DragDeltaTime <= 0f)
            {
                m_ScrollRect.content.anchoredPosition = destPos;
                m_Dragging = false;
            }
            else
            {
                Vector2 newPos = Vector2.Lerp(destPos, m_PrevViewPosition, m_DragDeltaTime / ScrollSec);
                m_ScrollRect.content.anchoredPosition = newPos;
            }
        }
    }

    public void SelectView(int InIndex)
    {
        if(0 <= InIndex && InIndex < ViewPositionList.Count)
        {
            m_PrevViewPosition = m_ScrollRect.content.anchoredPosition;
            m_CurrentViewIndex = InIndex;
            m_Dragging = true;
            m_DragDeltaTime = ScrollSec;
        }
    }

    public void OnBeginDrag(PointerEventData InData)
    {
        m_DragStartX = InData.position.x;
        m_Dragging = false;
    }

    public void OnEndDrag(PointerEventData InData)
    {
        // 스크롤 거리 측정, 양수면 좌측 메뉴로, 음수면 우측 메뉴로 변경
        float dragDistance = InData.position.x - m_DragStartX;

        // 정해진 만큼 스크롤 시 다른 메뉴로 변경, 메뉴를 넘어가거나 스크롤량이 모자를 경우 원래 메뉴 위치로 돌아간다.
        int changeViewIndex = -1;
        if (ViewChangeDistance <= Mathf.Abs(dragDistance))
            changeViewIndex = Mathf.Clamp(dragDistance > 0 ? m_CurrentViewIndex - 1 : m_CurrentViewIndex + 1, 0, ViewPositionList.Count - 1);

        if (changeViewIndex != -1 && changeViewIndex != m_CurrentViewIndex)
            MainMenu.SelectMenu(changeViewIndex);

        SelectView(changeViewIndex == -1 ? m_CurrentViewIndex : changeViewIndex);
    }
}
