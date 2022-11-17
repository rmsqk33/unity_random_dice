using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FNonObjectSingleton<T> where T : new() 
{
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();

            return instance;
        }
    }
}
