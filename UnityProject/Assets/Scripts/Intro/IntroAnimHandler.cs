using UnityEngine;
using FEnum;

public class IntroAnimHandler : MonoBehaviour
{
    public void OnCompleteAnim()
    {
        FSceneManager.Instance.ChangeScene(SceneType.Login);
    }
}
