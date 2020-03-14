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
    private ControllerNav controlNav;
    private bool isConfirmation;
    [SerializeField]
    private Transform readyMsgBar;
    private bool canPressBtn = true;
    [SerializeField]
    private Transform doorHolder;
    [SerializeField]
    private Transform cameraTransform, mainMenuCameraPoint, SinkPoint;
    [SerializeField]
    private float waitBetweenAnimation = 0.5f;
    [SerializeField]
    private float animationSpeed = 0.5f;
    [SerializeField]
    private float cameraMoveSpeed = 0.2f;
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
    private Loading loadingManager;

    private void Awake()
    {
        controlNav = GameObject.Find("EventSystem").GetComponent<ControllerNav>();
    }
    private void Start()
    {
        loadingManager = GameObject.Find("LoadingManager").GetComponent<Loading>();
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
            DisplayReadyMSg();
        }
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
        isConfirmation = true;
        readyMsgBar.gameObject.SetActive(true);
        controlNav.ButtonID = 1;
    }

    public void ReadConfirm()
    {
        readyMsgBar.gameObject.SetActive(false);
        RandomMap();
    }

    public void ReadyCancel()
    {
        isConfirmation = false;
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

    private void RandomMap()
    {
        int randomMap = Random.Range(0, 1);
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
        isTransition = true;
        canPressBtn = false;
        bool arrived = false;
        while (!arrived)
        {
            if (usingLerp == true)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, SinkPoint.position, cameraMoveSpeed * Time.deltaTime);
            }
            else
            {
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, SinkPoint.position, cameraMoveSpeed * Time.deltaTime);
            }
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, SinkPoint.rotation, cameraMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(cameraTransform.position, SinkPoint.position) < 0.01f) arrived = true;
            yield return null;
        }
        isTransition = false;
        yield return new WaitForSeconds(2);
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
