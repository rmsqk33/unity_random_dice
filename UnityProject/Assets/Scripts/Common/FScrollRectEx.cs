using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    public class FScrollRectEx : ScrollRect
    {
        ScrollRect m_ParentScrollRect = null;
        FLobbyScrollView m_ParentLobbyScrollView = null;

        bool m_NotiParent = false;

        protected override void Awake()
        {
            base.Awake();

            Transform parent = transform.parent;
            while (parent != null)
            {
                m_ParentScrollRect = parent.GetComponent<ScrollRect>();
                m_ParentLobbyScrollView = parent.GetComponent<FLobbyScrollView>();
                if (m_ParentScrollRect != null && m_ParentLobbyScrollView != null)
                    break;

                parent = parent.parent;
            }
            
        }

        override public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            base.OnInitializePotentialDrag(eventData);
            if (m_ParentScrollRect != null)
                m_ParentScrollRect.OnInitializePotentialDrag(eventData);
        }

        override public void OnBeginDrag(PointerEventData eventData)
        {
            m_NotiParent = Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y);

            if(m_NotiParent)
            {

                if (m_ParentScrollRect != null)
                    m_ParentScrollRect.OnBeginDrag(eventData);

                if (m_ParentLobbyScrollView != null)
                    m_ParentLobbyScrollView.OnBeginDrag(eventData);
            }
            else
                base.OnBeginDrag(eventData);
        }

        override public void OnEndDrag(PointerEventData eventData)
        {
            if (m_NotiParent)
            {
                if (m_ParentScrollRect != null)
                    m_ParentScrollRect.OnEndDrag(eventData);

                if (m_ParentLobbyScrollView != null)
                    m_ParentLobbyScrollView.OnEndDrag(eventData);
            }
            else
                base.OnEndDrag(eventData);
            
        }

        override public void OnDrag(PointerEventData eventData)
        {
            if (m_NotiParent)
            {
                if (m_ParentScrollRect != null)
                    m_ParentScrollRect.OnDrag(eventData);
            }
            else
                base.OnDrag(eventData);
            
        }
    }

}