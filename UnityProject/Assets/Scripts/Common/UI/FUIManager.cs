using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FUIManager : FSingleton<FUIManager>
{
    Canvas topSiblingCanvas;

    class CanvasSiblingComparer : IComparer<Canvas>
    {
        public int Compare(Canvas x, Canvas y)
        {
            return x.transform.GetSiblingIndex() - y.transform.GetSiblingIndex();
        }
    }

    Dictionary<Type, FUIBase> uiMap = new Dictionary<Type, FUIBase>();

    public T FindUI<T>() where T : FUIBase
    {
        Type type = typeof(T);
        if (uiMap.ContainsKey(type) == false)
        {
            uiMap.Add(type, GameObject.FindObjectOfType<T>());
        }
        else if (uiMap[type] == null)
        {
            uiMap[type] = GameObject.FindObjectOfType<T>();
        }

        return (T)uiMap[type];
    }

    public Canvas TopSiblingCanvas
    {
        get
        {
            if(topSiblingCanvas == null)
            {
                Canvas[] canvasArray = GameObject.FindObjectsOfType<Canvas>();
                if (canvasArray.Length == 0)
                    return null;

                List<Canvas> canvasList = canvasArray.ToList<Canvas>();
                canvasList.Sort(new CanvasSiblingComparer());

                topSiblingCanvas = canvasList[canvasList.Count - 1];
            }

            return topSiblingCanvas;
        }
    }
}
