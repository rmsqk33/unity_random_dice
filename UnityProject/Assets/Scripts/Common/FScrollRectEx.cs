using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class FScrollRectEx : ScrollRect
    {
        ScrollRect parentScrollRect = null;
        FLobbyScrollView parentLobbyScrollView = null;

        bool notiParent = false;

        protected override void Awake()
        {
            base.Awake();

            Transform parent = transform.parent;
            while (parent != null)
            {
                parentScrollRect = parent.GetComponent<ScrollRect>();
                parentLobbyScrollView = parent.GetComponent<FLobbyScrollView>();
                if (parentScrollRect != null && parentLobbyScrollView != null)
                    break;

                parent = parent.parent;
            }
            
        }

        override public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            base.OnInitializePotentialDrag(eventData);
            if (parentScrollRect != null)
                parentScrollRect.OnInitializePotentialDrag(eventData);
        }

        override public void OnBeginDrag(PointerEventData eventData)
        {
            notiParent = Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y);

            if(notiParent)
            {

                if (parentScrollRect != null)
                    parentScrollRect.OnBeginDrag(eventData);

                if (parentLobbyScrollView != null)
                    parentLobbyScrollView.OnBeginDrag(eventData);
            }
            else
                base.OnBeginDrag(eventData);
        }

        override public void OnEndDrag(PointerEventData eventData)
        {
            if (notiParent)
            {
                if (parentScrollRect != null)
                    parentScrollRect.OnEndDrag(eventData);

                if (parentLobbyScrollView != null)
                    parentLobbyScrollView.OnEndDrag(eventData);
            }
            else
                base.OnEndDrag(eventData);
            
        }

        override public void OnDrag(PointerEventData eventData)
        {
            if (notiParent)
            {
                if (parentScrollRect != null)
                    parentScrollRect.OnDrag(eventData);
            }
            else
                base.OnDrag(eventData);
            
        }
    }

}