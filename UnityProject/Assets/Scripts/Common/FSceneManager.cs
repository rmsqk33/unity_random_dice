using FEnum;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FSceneManager : FSingleton<FSceneManager>
{
    private SceneType nextSceneType;
    private SceneType currentSceneType;
    private float progress;

    private SpriteRenderer fadeSpriteRenderer;
    private float fadeTime;

    public SceneType CurrentSceneType { get { return currentSceneType; } }
    public float Progress { get { return progress; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        Instance.fadeSpriteRenderer = Instance.gameObject.AddComponent<SpriteRenderer>();
        Instance.fadeSpriteRenderer.color = new Color(0, 0, 0, 0);
        Instance.fadeSpriteRenderer.sprite = Resources.Load<Sprite>("Sprite/Loading/Square");
        Instance.fadeSpriteRenderer.sortingLayerID = SortingLayer.layers[SortingLayer.layers.Length - 1].id;
        Instance.fadeSpriteRenderer.enabled = false;

        SceneManager.sceneLoaded += Instance.OnSceneLoaded;
    }

    public FSceneManager()
    {
    }

    public void ChangeSceneAfterLoading(SceneType InType, float InFadeTime = 1)
    {
        progress = 0.0f;
        nextSceneType = InType;

        SceneManager.sceneLoaded += OnLoadingSceneLoaded;
        if (InFadeTime == 0)
        {
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            fadeTime = InFadeTime;
            StartCoroutine(ChangeSceneAfterFadeOutCoroutine(SceneType.Loading));
        }
    }

    public void ChangeScene(SceneType InType, float InFadeTime = 0.0f)
    {
        if (InFadeTime == 0)
        {
            SceneManager.LoadScene(ConvertSceneTypeToString(InType));
        }
        else
        {
            fadeTime = InFadeTime;
            StartCoroutine(ChangeSceneAfterFadeOutCoroutine(InType));
        }
    }

    private void OnSceneLoaded(Scene InScene, LoadSceneMode InMode)
    {
        currentSceneType = ConvertStringToSceneType(InScene.name);

        float worldScreenHeight = (float)(Camera.main.orthographicSize * 2.0);
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        Instance.transform.localScale = new Vector3(worldScreenWidth, worldScreenHeight, 1);

        if (0 < fadeTime)
        {
            StartCoroutine(FadeInCoroutine());
        }
    }

    private void OnLoadingSceneLoaded(Scene InScene, LoadSceneMode InMode)
    {
        if (SceneType.Loading != ConvertStringToSceneType(InScene.name))
            return;

        StartCoroutine(ChangeSceneAfterLoadingCoroutine());
    }

    private IEnumerator ChangeSceneAfterLoadingCoroutine()
    {
        yield return new WaitForSecondsRealtime(fadeTime);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(ConvertSceneTypeToString(nextSceneType));
        asyncOperation.allowSceneActivation = false;

        float timer = 0.0f;
        while (!asyncOperation.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            float maxProgress = asyncOperation.progress < 0.9f ? asyncOperation.progress : 1.0f;
            progress = Mathf.Lerp(progress, maxProgress, timer);
            if (maxProgress <= progress)
            {
                timer = 0f;
            }

            if (1.0f <= progress)
            {
                SceneManager.sceneLoaded -= OnLoadingSceneLoaded;
                currentSceneType = nextSceneType;
                nextSceneType = SceneType.None;

                if(0 < fadeTime)
                {
                    yield return StartCoroutine(FadeOutCoroutine());
                }
                else
                {
                    yield return new WaitForSecondsRealtime(0.5f);
                }

                asyncOperation.allowSceneActivation = true;
                yield break;
            }
        }
    }
    
    private IEnumerator ChangeSceneAfterFadeOutCoroutine(SceneType InType)
    {
        yield return StartCoroutine(FadeOutCoroutine());

        SceneManager.LoadScene(ConvertSceneTypeToString(InType));
    }

    private IEnumerator FadeOutCoroutine()
    {
        fadeSpriteRenderer.enabled = true;
        SetFade(0);

        float deltaTime = 0;
        while (fadeSpriteRenderer.color.a < 1)
        {
            yield return null;
         
            deltaTime += Time.unscaledDeltaTime;
            SetFade(Mathf.Lerp(0, 1, deltaTime / fadeTime));
        }
    }

    private IEnumerator FadeInCoroutine()
    {
        SetFade(1);

        float deltaTime = 0;
        while (0 < fadeSpriteRenderer.color.a)
        {
            yield return null;
         
            deltaTime += Time.unscaledDeltaTime;
            SetFade(Mathf.Lerp(0, 1, 1 - deltaTime / fadeTime));
        }

        fadeSpriteRenderer.enabled = false;

        if(currentSceneType != SceneType.Loading)
        {
            fadeTime = 0;
        }
    }

    private void SetFade(float InAlpha)
    {
        Color color = fadeSpriteRenderer.color;
        color.a = InAlpha;
        fadeSpriteRenderer.color = color;
    }

    private string ConvertSceneTypeToString(SceneType InType)
    {
        switch (InType)
        {
            case SceneType.Login: return "LoginScene";
            case SceneType.Lobby: return "LobbyScene";
            case SceneType.Battle: return "BattleScene";
            case SceneType.Loading: return "LoadingScene";
        }

        return "";
    }

    private SceneType ConvertStringToSceneType(string InSceneName)
    {
        switch (InSceneName)
        {
            case "LoginScene": return SceneType.Login;
            case "LobbyScene": return SceneType.Lobby;
            case "BattleScene": return SceneType.Battle;
            case "LoadingScene": return SceneType.Loading;
        }

        return SceneType.None;
    }

}
