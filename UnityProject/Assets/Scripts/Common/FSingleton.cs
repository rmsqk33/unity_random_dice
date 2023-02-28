using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance = null;

    public static T Instance 
    {
        get 
        {
            if(m_Instance == null)
            {
                GameObject gameObject = new GameObject(typeof(T).Name);
                m_Instance = gameObject.AddComponent<T>();
                DontDestroyOnLoad(gameObject);
            }

            return m_Instance;
        } 
    }

    protected virtual void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
        else if(m_Instance.gameObject != gameObject) 
        {
            GameObject.Destroy(gameObject);
        }
    }
}
