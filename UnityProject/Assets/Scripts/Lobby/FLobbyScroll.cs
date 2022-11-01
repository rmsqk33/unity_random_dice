using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLobbyScroll : MonoBehaviour
{
    [SerializeField]
    float ScrollStart;
    [SerializeField]
    List<float> ScrollPositionList;
    [SerializeField]
    int InitScrollNumber;

    void Start()
    {
    }

    void OnScrollChange(Vector2 InDelta)
    {

    }
}
