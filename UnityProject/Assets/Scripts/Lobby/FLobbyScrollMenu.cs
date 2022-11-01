using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FLobbyScrollMenu : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    ScrollRect ScrollRect;
    [SerializeField]
    float MenuChangeDistance; // 해당 거리이상 드래그 시 메뉴 변경
    [SerializeField]
    float MenuScrollSec; // 메뉴 변경 시 스크롤 시간
    [SerializeField]
    List<Vector2> ScrollPositionList; // 메뉴별 스크롤 위치
    [SerializeField]
    int InitMenuNumber; // 초기 메뉴, 제일 왼쪽이 1

    Vector2 m_PrevMenuPosition = Vector2.zero;
    float m_DragStartX = 0f;
    float m_DragDeltaTime = 0f;
    int m_CurrentScrollIndex = 0;
    bool m_Dragging = false;

    void Start()
    {
        m_CurrentScrollIndex = InitMenuNumber - 1;
        ScrollRect.content.anchoredPosition = ScrollPositionList[m_CurrentScrollIndex];
    }

    void Update()
    {
        if(m_Dragging)
        {
            m_DragDeltaTime -= Time.deltaTime;
            Vector2 destPos = ScrollPositionList[m_CurrentScrollIndex];
            if (m_DragDeltaTime <= 0f)
            {
                ScrollRect.content.anchoredPosition = destPos;
                m_Dragging = false;
            }
            else
            {
                Vector2 newPos = Vector2.Lerp(destPos, m_PrevMenuPosition, m_DragDeltaTime / MenuScrollSec);
                ScrollRect.content.anchoredPosition = newPos;
            }
        }
    }

    void ChangeMenu(int InMenuIndex)
    {
        if(0 <= InMenuIndex && InMenuIndex < ScrollPositionList.Count)
        {
            m_PrevMenuPosition = ScrollRect.content.anchoredPosition;
            m_CurrentScrollIndex = InMenuIndex;
            m_Dragging = true;
            m_DragDeltaTime = MenuScrollSec;
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
        if (MenuChangeDistance <= Mathf.Abs(dragDistance))
        {
            m_CurrentScrollIndex = Mathf.Clamp(dragDistance > 0 ? m_CurrentScrollIndex - 1 : m_CurrentScrollIndex + 1, 0, ScrollPositionList.Count - 1);
        }

        ChangeMenu(m_CurrentScrollIndex);
    }
}
