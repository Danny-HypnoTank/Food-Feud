using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    private bool active;
    [SerializeField]
    private bool useDecimals;
    [SerializeField]
    private int targetFPS;

    [Header("Visual")]
    [SerializeField]
    private Text fpsText;

    private float currentFPS;
    private float minFPS;
    private float maxFPS;
    private float avgFPS;
    private bool display;

    // Start is called before the first frame update
    void Start()
    {
        minFPS = targetFPS;
        maxFPS = targetFPS;
        display = false;
        fpsText.gameObject.SetActive(active);
        DontDestroyOnLoad(this);
        StartCoroutine(StartupDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (display)
            {
                currentFPS = 1 / Time.unscaledDeltaTime;
                CalcMinMaxAvg(currentFPS);
                UpdateText();
            }
        }
    }

    private void UpdateText()
    {

        if(!useDecimals)
        {
            currentFPS = Mathf.Round(currentFPS);
            minFPS = Mathf.Round(minFPS);
            maxFPS = Mathf.Round(maxFPS);
            avgFPS = Mathf.Round(avgFPS);
        }

        StringBuilder strBuilder = new StringBuilder();

        strBuilder.AppendLine($"FPS: {currentFPS}");
        strBuilder.AppendLine($"Minimum: {minFPS}");
        strBuilder.AppendLine($"Maximum: {maxFPS}");
        strBuilder.AppendLine($"Average: {avgFPS}");

        fpsText.text = strBuilder.ToString();
    }

    private void CalcMinMaxAvg(float fps)
    {
        if (fps < minFPS)
            minFPS = fps;
        if (fps > maxFPS)
            maxFPS = fps;

        avgFPS = (minFPS + maxFPS) / 2;
    }

    private IEnumerator StartupDelay()
    {
        yield return new WaitForSeconds(1);
        display = true;
    }

}