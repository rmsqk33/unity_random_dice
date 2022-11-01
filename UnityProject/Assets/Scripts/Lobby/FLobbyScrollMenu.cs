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
    float MenuChangeDistance; // �ش� �Ÿ��̻� �巡�� �� �޴� ����
    [SerializeField]
    float MenuScrollSec; // �޴� ���� �� ��ũ�� �ð�
    [SerializeField]
    List<Vector2> ScrollPositionList; // �޴��� ��ũ�� ��ġ
    [SerializeField]
    int InitMenuNumber; // �ʱ� �޴�, ���� ������ 1

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
        // ��ũ�� �Ÿ� ����, ����� ���� �޴���, ������ ���� �޴��� ����
        float dragDistance = InData.position.x - m_DragStartX;
        
        // ������ ��ŭ ��ũ�� �� �ٸ� �޴��� ����, �޴��� �Ѿ�ų� ��ũ�ѷ��� ���ڸ� ��� ���� �޴� ��ġ�� ���ư���.
        if (MenuChangeDistance <= Mathf.Abs(dragDistance))
        {
            m_CurrentScrollIndex = Mathf.Clamp(dragDistance > 0 ? m_CurrentScrollIndex - 1 : m_CurrentScrollIndex + 1, 0, ScrollPositionList.Count - 1);
        }

        ChangeMenu(m_CurrentScrollIndex);
    }
}
