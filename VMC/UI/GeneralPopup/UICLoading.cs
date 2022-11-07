using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UICLoading : MonoBehaviour
{
    private AsyncOperation asyncLoad;
    private string sceneName;
    private bool isShowing;
    private float nextTime;
    private float counter;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Slider slider;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void LoadScene(string nameScene)
    {
        this.sceneName = nameScene;
        StartCoroutine(LoadAsyncScene());
    }
    IEnumerator LoadAsyncScene()
    {
        canvasGroup.alpha = 1f;
        slider.value = 0f;
        this.isShowing = false;
        this.nextTime = Time.time + 0.5f;
        this.counter = 0f;
        slider.value = 0f;
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            slider.value = Mathf.Min(asyncLoad.progress, counter);
            if (!isShowing && Time.time > nextTime && asyncLoad.progress >= 0.9f)
            {
                isShowing = true;
                asyncLoad.allowSceneActivation = true;
                canvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                });
            }
            counter += Time.deltaTime / 0.5f;
            yield return null;
        }
    }
}
