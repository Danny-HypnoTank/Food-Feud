/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 22/11/2019
 * Last Modified: 22/11/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCharacterSelectionController : MonoBehaviour
{
    private bool canPressBtn = true;
    [SerializeField]
    private Transform doorHolder;
    [SerializeField]
    private Transform cameraTransform, mainMenuCameraPoint, sinkPos;
    [SerializeField]
    private float waitBetweenAnimation = 0.5f;
    [SerializeField]
    private float animationSpeed = 0.5f;
    [SerializeField]
    private float cameraMoveSpeed = 0.2f;
    [SerializeField]
    private bool usingLerp = true;
    [SerializeField]
    private Transform[] pinLocations, playerPins, pinController;
    [SerializeField]
    private Player[] players;
    private int minPlayers = 2;
    [Header("Animation and animation points")]
    private Animator doorAnimation;
    private bool isTransition = false;
    private Loading loadingManager;

    private void Start()
    {
        loadingManager = GameObject.Find("LoadingManager").GetComponent<Loading>();
    }

    private void OnEnable()
    {
        doorAnimation = doorHolder.GetComponent<Animator>();
        doorAnimation.enabled = false;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].isActivated = false;
            players[i].isLocked = false;
            players[i].playerScore = 0;
        }
        for (int i = 0; i < playerPins.Length; i++)
        {
            playerPins[i].gameObject.SetActive(false);
            playerPins[i].transform.position = pinLocations[i].transform.position;
            playerPins[i].gameObject.SetActive(true);
        }
        canPressBtn = true;
        isTransition = false;
    }
    //Default rotation(closed) 117.585 open rotation 45.021

    private void Update()
    {
        if (canPressBtn == true)
        {
            if (Input.GetButtonDown("BackButton"))
            {
                if (isTransition == false)
                {
                    bool canReturn = true;
                    for (int i = 0; i < pinController.Length; i++)
                    {
                        if (pinController[i].GetComponent<PinController>().IsActive == true)
                        {
                            canReturn = false;
                        }
                    }
                    if (canReturn == true)
                    {
                        ReturnToMainMenu();
                    }
                }
            }

        }
    }

    public void CheckSubmission()
    {
        bool canStart = true;
        int readyPlayers = 0;
        for (int i = 0; i < pinController.Length; i++)
        {
            if (pinController[i].GetComponent<PinController>().IsActive == true)
            {
                if (pinController[i].GetComponent<PinController>().IsLocked == true)
                {
                    readyPlayers++;
                }
                else if (pinController[i].GetComponent<PinController>().IsLocked == false)
                {
                    canStart = false;
                }
            }
        }
        // Debug.Log(canStart + " and ready players num: " + readyPlayers);
        if (canStart == true && readyPlayers >= 2)
        {
            RandomMap();
        }
    }

    private void RandomMap()
    {
        int randomMap = Random.Range(0, 2);
        if (randomMap == 0)
        {
            doorAnimation.enabled = true;
            doorAnimation.speed = animationSpeed;
            doorAnimation.SetInteger("CloseAnim", 1);
            StartCoroutine("OpenFridge");
        }
        else
        {
            StartCoroutine("CameraSide");
        }
    }

    private IEnumerator OpenFridge()
    {
        canPressBtn = false;
        isTransition = true;
        yield return new WaitForSeconds(waitBetweenAnimation);
        isTransition = false;
        loadingManager.SetID(1);
        loadingManager.InitializeLoading();
        yield return null;
    }


    private void ReturnToMainMenu()
    {
        StartCoroutine("CameraUp");
    }

    private IEnumerator CameraSide()
    {
        canPressBtn = false;

        bool arrived = false;
        while (!arrived)
        {
            if (usingLerp == true)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, sinkPos.position, cameraMoveSpeed);
            }
            else
            {
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, sinkPos.position, cameraMoveSpeed);
            }
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, sinkPos.rotation, cameraMoveSpeed);
            if (Vector3.Distance(cameraTransform.position, sinkPos.position) < 0.1f) arrived = true;
            Debug.Log("Has arrived");
            yield return null;
        }

        yield return new WaitForSeconds(waitBetweenAnimation);
        loadingManager.SetID(3);
        loadingManager.InitializeLoading();
        yield return null;
    }

    private IEnumerator CameraUp()
    {
        canPressBtn = false;

        bool arrived = false;
        while (!arrived)
        {
            if (usingLerp == true)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, mainMenuCameraPoint.position, cameraMoveSpeed);
            }
            else
            {
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, mainMenuCameraPoint.position, cameraMoveSpeed);
            }
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, mainMenuCameraPoint.rotation, cameraMoveSpeed);
            if (Vector3.Distance(cameraTransform.position, mainMenuCameraPoint.position) < 0.1f) arrived = true;
            yield return null;
        }

        yield return new WaitForSeconds(waitBetweenAnimation);
        MenuController.instance.CharacterSelectionToMainMenu();
        yield return null;
    }
}
