using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{

    [SerializeField]
    private GameObject loadingScreenPanel;
    [SerializeField]
    private Image circleImg;
    [SerializeField]
    private RectTransform loadingFX;
    /*[SerializeField]
    private Slider slider;*/
    [SerializeField]
    private Text progressLoaded;
    [SerializeField]
    private int sceneToLoadId = 1;

    [SerializeField]
    [Range(0, 1)] float progress = 0f;

    private void Start()
    {
        loadingScreenPanel.gameObject.SetActive(false);
    }

    public void InitializeLoading()
    {
        StopCoroutine("LoadingProgress");
        StartCoroutine("LoadingProgress");
    }

    public void SetID(int id)
    {
        sceneToLoadId = id;
    }

    //Coroutine for loading screen
    private IEnumerator LoadingProgress()
    {
        //activate loading screen object and set scene
        //suspend coroutine for 10 secs to loading screen to last longer
        loadingScreenPanel.SetActive(true);
        yield return new WaitForSeconds(3);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoadId);
        async.allowSceneActivation = false;

        //if is async is false, use slider to calculate the loading progress
        //and clamp values between 0 & 1
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            circleImg.fillAmount = progress;
            //slider.value = progress;

            //set text
            //progressLoaded.text = Mathf.CeilToInt(progress * 100).ToString();
            progressLoaded.text = Mathf.Floor(progress * 100).ToString();
            loadingFX.rotation = Quaternion.Euler(new Vector3(0f, 0f, -progress * 360));

            if (async.progress == 0.9f)
            {
                //slider.value = 1f;
                circleImg.fillAmount = 1f;
                async.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
