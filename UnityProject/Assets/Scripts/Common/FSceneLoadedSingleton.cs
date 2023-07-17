using UnityEngine;

public class FSceneLoadedSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
        }
        else if (instance.gameObject != gameObject)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
