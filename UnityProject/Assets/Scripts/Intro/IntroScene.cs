using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    public void CompleteIntroAnim()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
