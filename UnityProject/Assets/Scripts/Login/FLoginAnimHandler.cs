using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLoginAnimHandler : MonoBehaviour
{
    [SerializeField]
    List<GameObject> activeList;

    void Start()
    {
        foreach(GameObject gameObject in activeList)
        {
            gameObject.SetActive(false);
        }
    }

    void OnCompleteAnim()
    {
        foreach(GameObject gameObject in activeList)
        {
            gameObject.SetActive(true);
        }
    }
}
