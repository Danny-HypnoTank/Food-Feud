using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{

    [SerializeField]
    private GameObject loadingPanel;
    [SerializeField]
    private Image circleImg;
    [SerializeField]
    private Text progressLoaded;
    [SerializeField]
    private int sceneToLoadId = 1;

    [SerializeField]
    [Range(0, 1)] float progress = 0f;

    [SerializeField]
    private string[] tipsAndHints;
    [SerializeField]
    private Text tipText;

    [SerializeField]
    private GameObject[] videoClips;
    private int currentClip;


    private void Awake()
    {
        currentClip = Random.Range(0, videoClips.Length);

        tipText.text = tipsAndHints[currentClip];
        videoClips[currentClip].SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(LoadingProgress());
    }

    /*public void InitializeLoading()
    {
        StopCoroutine("LoadingProgress");
        
    }*/

    public void SetID(int id)
    {
        sceneToLoadId = id;
    }

    //Coroutine for loading screen
    public IEnumerator LoadingProgress()
    {
        //suspend coroutine for 3 secs for loading screen to last longer
        yield return new WaitForSeconds(5);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoadId);
        async.allowSceneActivation = false;

        //if is async is false, use slider to calculate the loading progress
        //and clamp values between 0 & 1
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            circleImg.fillAmount = progress;
            
            //set text
            progressLoaded.text = Mathf.Floor(progress * 100).ToString();
            
            //loadingFX.rotation = Quaternion.Euler(new Vector3(0f, 0f, -progress * 360));

            if (async.progress == 0.9f)
            {
                circleImg.fillAmount = 1f;
                async.allowSceneActivation = true;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
