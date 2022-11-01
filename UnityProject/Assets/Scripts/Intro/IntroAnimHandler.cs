using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroAnimHandler : MonoBehaviour
{
    public void OnCompleteAnim()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
