using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginAnimHandler : MonoBehaviour
{
    [SerializeField]
    List<GameObject> ActiveList;

    void Start()
    {
        foreach(GameObject gameObject in ActiveList)
        {
            gameObject.SetActive(false);
        }
    }

    void OnCompleteAnim()
    {
        foreach(GameObject gameObject in ActiveList)
        {
            gameObject.SetActive(true);
        }
    }
}
