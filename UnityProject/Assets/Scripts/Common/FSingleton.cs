using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance = null;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            GameObject.Destroy(gameObject);
        }
    }
}
