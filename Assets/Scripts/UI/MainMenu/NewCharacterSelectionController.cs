
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
using System;

public class NewCharacterSelectionController : MonoBehaviour
{
    private ControllerNav controlNav;
    [SerializeField]
    private bool isConfirmation;
    [SerializeField]
    private Transform readyMsgBar;
    [SerializeField]
    private GameObject readyImage1;
    [SerializeField]
    private GameObject readyImage2;
    [SerializeField]
    private bool canPressBtn = true;
    [SerializeField]
    private Transform doorHolder;
    [SerializeField]
    private Transform cameraTransform, mainMenuCameraPoint, SinkPoint, BinPoint;
    [SerializeField]
    private float waitBetweenAnimation = 0.5f;
    [SerializeField]
    private float animationSpeed = 0.5f;
    [SerializeField]
    private float cameraMoveSpeed = 0.2f;
    [SerializeField]
    private float goToLevelDelay = 0.2f;
    [SerializeField]
    private bool usingLerp = true;
    [SerializeField]
    private Transform[] pinLocations, playerPins;
    [SerializeField]
    private PinController[] pinController;
    [SerializeField]
    private Player[] players;
    private int minPlayers = 2;
    [Header("Animation and animation points")]
    private Animator doorAnimation;
    private bool isTransition = false;
    //private Loading loadingManager;
    [SerializeField]
    private bool forceload;

    [SerializeField]
    [Range(0,2)]
    private int forceLevel;

    public bool CanPressBtn { get => canPressBtn; set => canPressBtn = value; }
    public bool IsConfirmation { get => isConfirmation; set => isConfirmation = value; }

    private void Awake()
    {
        controlNav = GameObject.Find("EventSystem").GetComponent<ControllerNav>();
    }
    private void Start()
    {
        //loadingManager = GameObject.Find("LoadingManager").GetComponent<Loading>();
        readyMsgBar.gameObject.SetActive(false);
        readyImage1.SetActive(true);
        readyImage2.SetActive(false);
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
        if (canStart == true && readyPlayers > 1)
        {
            DisplayReadyMSg();
        }
    }

    private void OnDisable()
    {
        if (doorAnimation != null)
        {
            doorAnimation.enabled = false;
        }
        canPressBtn = true;
        isTransition = false;
        isConfirmation = false;
    }

    private void OnEnable()
    {
        doorAnimation = doorHolder.GetComponent<Animator>();
        doorAnimation.enabled = false;
        canPressBtn = true;
        isTransition = false;
        isConfirmation = false;
        ResetPlayers();
        //ReadyCancel();  
    }

    private void ResetSoft()
    {
        Debug.Log("Ran at end of door opening!");
    }

    private void ResetPlayers()
    {
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
    }
    //Default rotation(closed) 117.585 open rotation 45.021

    private void Update()
    {
        if (isConfirmation == true)
        {
            if (Input.GetButtonDown("BackButton"))
            {
                ReadyCancel();
            }
            if (Input.GetButtonDown("Dash"))
            {
                ReadConfirm();
            }
        }
        else if (canPressBtn == true)
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

    private void DisplayReadyMSg()
    {
        canPressBtn = false;
        isConfirmation = true;
        readyImage1.SetActive(true);
        readyImage2.SetActive(false);
        readyMsgBar.gameObject.SetActive(true);
        controlNav.ButtonID = 1;
    }

    public void ReadConfirm()
    {
        readyImage1.SetActive(false);
        readyImage2.SetActive(true);
        StartCoroutine(GoToLevelSelect());
    }

    public void ReadyCancel()
    {
        canPressBtn = true;
        isConfirmation = false;
        readyImage1.SetActive(true);
        readyImage2.SetActive(false);
        readyMsgBar.gameObject.SetActive(false);
        for (int i = 0; i < pinController.Length; i++)
        {
            pinController[i].UnlockPlayer();
        }
        for (int i = 0; i < pinController.Length; i++)
        {
            pinController[i].gameObject.SetActive(true);
        }
    }

    //Coroutine for going to the level select
    private IEnumerator GoToLevelSelect()
    {
        yield return new WaitForSeconds(goToLevelDelay);
        readyImage2.SetActive(false);
        //Call method for enabling level select UI
        MenuController.instance.CharacterSelectionToLevelSelect();
    }

    //Movement of camera to main menu
    private void ReturnToMainMenu()
    {
        //StartCoroutine("CameraUp");
        StartCoroutine(MoveCamera(mainMenuCameraPoint, 0.5f));
    }

    //Movement of camera to main menu
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

    private IEnumerator MoveCamera(Transform destination, float time)
    {
        canPressBtn = false;

        bool arrived = false;

        float t = 0;
        while (t <= time)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, destination.position, t / time);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, destination.rotation, t / time);
            //if (Vector3.Distance(cameraTransform.position, destination.position) < 0.01f) arrived = true;

            t += Time.deltaTime;


            yield return null;
        }
        
        //yield return new WaitForSeconds(waitBetweenAnimation);
        cameraTransform.position = destination.position;
        cameraTransform.rotation = destination.rotation;
        arrived = true;
        MenuController.instance.CharacterSelectionToMainMenu();

        //canPressBtn = true;

        yield return null;
    }

    private IEnumerator OpenFridge()
    {
        yield return new WaitForSeconds(0.1f);
        doorAnimation.enabled = true;
        doorAnimation.speed = animationSpeed;
        doorAnimation.SetInteger("CloseAnim", 1);
        canPressBtn = false;
        isTransition = true;
        yield return new WaitForSeconds(waitBetweenAnimation);
        isTransition = false;
        yield return null;
    }

    #region Old Code DO NOT TOUCH
    //private void RandomMap()
    //{
    //    int mapID = -1;

    //    if (forceload)
    //    {
    //        mapID = forceLevel;
    //    }
    //    else
    //    {
    //        mapID = UnityEngine.Random.Range(0, 3);
    //    }
    //    switch (mapID)
    //    {
    //        case 0:
    //            StartCoroutine("OpenFridge");
    //            break;

    //        case 1:
    //            StartCoroutine("CameraBin");
    //            break;

    //        case 2:
    //            StartCoroutine("CameraSide");
    //            break;

    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }
    //}

    //private IEnumerator CameraBin()
    //{
    //    yield return new WaitForSeconds(0.7f);
    //    readyMsgBar.gameObject.SetActive(false);
    //    isTransition = true;
    //    canPressBtn = false;
    //    bool arrived = false;
    //    while (!arrived)
    //    {
    //        if (usingLerp == true)
    //        {
    //            cameraTransform.position = Vector3.Lerp(cameraTransform.position, BinPoint.position, cameraMoveSpeed * Time.deltaTime);
    //        }
    //        else
    //        {
    //            cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, BinPoint.position, cameraMoveSpeed * Time.deltaTime);
    //        }
    //        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, BinPoint.rotation, cameraMoveSpeed * Time.deltaTime);
    //        if (Vector3.Distance(cameraTransform.position, BinPoint.position) < 0.01f) arrived = true;
    //        yield return null;
    //    }
    //    isTransition = false;
    //    yield return new WaitForSeconds(2);
    //    loadingManager.SetID(4);
    //    loadingManager.InitializeLoading();
    //    yield return null;
    //}

    //private IEnumerator CameraSide()
    //{
    //    yield return new WaitForSeconds(0.7f);
    //    readyMsgBar.gameObject.SetActive(false);
    //    isTransition = true;
    //    canPressBtn = false;
    //    bool arrived = false;
    //    while (!arrived)
    //    {
    //        if (usingLerp == true)
    //        {
    //            cameraTransform.position = Vector3.Lerp(cameraTransform.position, SinkPoint.position, cameraMoveSpeed * Time.deltaTime);
    //        }
    //        else
    //        {
    //            cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, SinkPoint.position, cameraMoveSpeed * Time.deltaTime);
    //        }
    //        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, SinkPoint.rotation, cameraMoveSpeed * Time.deltaTime);
    //        if (Vector3.Distance(cameraTransform.position, SinkPoint.position) < 0.01f) arrived = true;
    //        yield return null;
    //    }
    //    isTransition = false;
    //    yield return new WaitForSeconds(2);
    //    loadingManager.SetID(3);
    //    loadingManager.InitializeLoading();
    //    yield return null;
    //}
    #endregion
}
