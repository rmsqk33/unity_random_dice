using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // - 고유 인스턴스
    private static T instance;

    // - 인스턴스 얻기
    public static T Instance
    {
        get
        {
            // - null일경우 생성
            if( instance == null )
            {
                // - 오브젝트에 있을경우 찾기
                GameObject obj;
                obj = GameObject.Find(typeof(T).Name);
                // - 없을경우 생성
                if( obj == null )
                {
                    obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
                else
                {
                    instance = obj.GetComponent<T>();
                }
            }

            return instance;
        }
    }

    // - 생성시 파괴불가 설정
    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);
    }
}
