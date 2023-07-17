using UnityEngine;

public class FSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance 
    {
        get 
        {
            if(instance == null)
            {
                GameObject gameObject = new GameObject(typeof(T).Name);
                instance = gameObject.AddComponent<T>();
                DontDestroyOnLoad(gameObject);
            }

            return instance;
        } 
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
        }
        else if(instance.gameObject != gameObject) 
        {
            GameObject.Destroy(gameObject);
        }
    }
}
