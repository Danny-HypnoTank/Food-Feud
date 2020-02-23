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
    private Slider slider;
    [SerializeField]
    private Text percentLoaded;
    [SerializeField]
    private int sceneToLoadId = 1;

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
        yield return new WaitForSeconds(10);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoadId);
        async.allowSceneActivation = false;

        //if is async is false, use slider to calculate the loading progress
        //and clamp values between 0 & 1
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            slider.value = progress;

            //set text
            percentLoaded.text = Mathf.CeilToInt(progress * 100).ToString() + "%";

            if (async.progress == 0.9f)
            {
                slider.value = 1f;
                
                async.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
