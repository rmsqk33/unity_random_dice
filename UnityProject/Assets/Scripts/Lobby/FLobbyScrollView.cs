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
    float m_ViewChangeDistance; // �ش� �Ÿ��̻� �巡�� �� �޴� ����
    [SerializeField]
    float m_ScrollSec; // ��ũ�� �ð�
    [SerializeField]
    int m_InitViewIndex; // �ʱ� �޴�, ���� ������ 1
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
        // ��ũ�� �Ÿ� ����, ����� ���� �޴���, ������ ���� �޴��� ����
        float dragDistance = InData.position.x - m_DragStartX;

        // ������ ��ŭ ��ũ�� �� �ٸ� �޴��� ����, �޴��� �Ѿ�ų� ��ũ�ѷ��� ���ڸ� ��� ���� �޴� ��ġ�� ���ư���.
        int changeViewIndex = -1;
        if (m_ViewChangeDistance <= Mathf.Abs(dragDistance))
            changeViewIndex = Mathf.Clamp(dragDistance > 0 ? m_CurrentViewIndex - 1 : m_CurrentViewIndex + 1, 0, m_ScrollMenuList.Count - 1);

        if (changeViewIndex != -1 && changeViewIndex != m_CurrentViewIndex)
            m_MainMenu.SelectMenu(changeViewIndex);

        SelectView(changeViewIndex == -1 ? m_CurrentViewIndex : changeViewIndex);
    }
}
