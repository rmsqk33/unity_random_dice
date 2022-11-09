using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FLobbyScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    FMainMenuUI m_MainMenu = null;
    [SerializeField]
    float m_ViewChangeDistance; // 해당 거리이상 드래그 시 메뉴 변경
    [SerializeField]
    float m_ScrollSec; // 스크롤 시간
    [SerializeField]
    int m_InitViewIndex; // 초기 메뉴, 제일 왼쪽이 1
    [SerializeField]
    List<FLobbyScrollMenuBase> m_ScrollMenuList;

    ScrollRect m_ScrollRect = null;
    Vector2 m_PrevViewPosition = Vector2.zero;
    Vector2 m_MoveViewPosition = Vector2.zero;
    float m_DragStartX = 0f;
    float m_DragDeltaTime = 0f;
    int m_CurrentViewIndex = -1;
    bool m_Dragging = false;

    void Awake()
    {
        m_ScrollRect = GetComponent<ScrollRect>();
    }

    void Start()
    {
        SelectView(m_InitViewIndex);
    }

    void Update()
    {
        if(m_Dragging)
        {
            m_DragDeltaTime -= Time.deltaTime;
            if (m_DragDeltaTime <= 0f)
            {
                m_ScrollRect.content.anchoredPosition = m_MoveViewPosition;
                m_Dragging = false;
            }
            else
            {
                Vector2 newPos = Vector2.Lerp(m_MoveViewPosition, m_PrevViewPosition, m_DragDeltaTime / m_ScrollSec);
                m_ScrollRect.content.anchoredPosition = newPos;
            }
        }
    }

    public void SelectView(int InIndex)
    {
        if(0 <= InIndex && InIndex < m_ScrollMenuList.Count)
        {
            if(m_CurrentViewIndex != InIndex)
            {
                if (m_CurrentViewIndex != -1)
                    m_ScrollMenuList[m_CurrentViewIndex].OnDeactive();

                m_ScrollMenuList[InIndex].OnActive();
            }

            m_PrevViewPosition.x = m_ScrollRect.content.anchoredPosition.x;
            m_MoveViewPosition.x = m_ScrollMenuList[InIndex].GetComponent<RectTransform>().anchoredPosition.x * -1;
            m_CurrentViewIndex = InIndex;
            m_Dragging = true;
            m_DragDeltaTime = m_ScrollSec;
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
        if (m_ViewChangeDistance <= Mathf.Abs(dragDistance))
            changeViewIndex = Mathf.Clamp(dragDistance > 0 ? m_CurrentViewIndex - 1 : m_CurrentViewIndex + 1, 0, m_ScrollMenuList.Count - 1);

        if (changeViewIndex != -1 && changeViewIndex != m_CurrentViewIndex)
            m_MainMenu.SelectMenu(changeViewIndex);

        SelectView(changeViewIndex == -1 ? m_CurrentViewIndex : changeViewIndex);
    }
}
