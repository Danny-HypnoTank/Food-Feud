using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerCameraControl : MonoBehaviour
{
    [SerializeField]
    private Camera[] cameras;

    private int currentCamera;
    private Camera previousCamera;

    private void Awake()
    {
        currentCamera = 0;
        previousCamera = null;
    }

    void Update()
    {
        if(Input.GetButtonDown("Shoot") || Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCamera();
        }
    }

    private void SwitchCamera()
    {
        currentCamera++;
        if (currentCamera == cameras.Length)
            currentCamera = 0;

        if (previousCamera != null)
            previousCamera.gameObject.SetActive(false);

        cameras[currentCamera].gameObject.SetActive(true);
        previousCamera = cameras[currentCamera];
    }
}
