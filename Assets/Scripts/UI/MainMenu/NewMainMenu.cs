/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 21/11/2019
 * Modified by: Alex Watson
 * Last Modified: 08/03/2020
 */
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewMainMenu : MonoBehaviour
{
    [Header("Main menu buttons and sprites")]
    [SerializeField]
    private UIElementController[] mainMenuButtons;
    [SerializeField]
    private Transform confirmationMsg;
    [SerializeField]
    private Transform quitPanel;
    [Header("Logic")]
    [SerializeField]
    private int selectId;
    [SerializeField]
    private bool isAxis = false;
    [Header("Animation and animation points")]
    private Animator doorAnimation;
    [SerializeField]
    private Transform doorHolder;
    [SerializeField]
    private Transform cameraTransform, optionsCameraPoint, characterSelectPoint, medalViewPoint, medalViewPointB, defaultCameraPoint, controlsCameraPoint;
    [Header("Handles animation for moving into upper fridge")]
    [SerializeField]
    private bool usingLerp = true;
    [SerializeField]
    private float waitBetweenAnimation = 0.5f;
    [SerializeField]
    private float cameraMoveSpeed = 0.2f;
    [SerializeField]
    private float animationSpeed = 0.5f;
    [SerializeField]
    private float cameraSpeedMedals = 0.4f, cameraSpeedReset = 0.6f;
    [SerializeField]
    private bool canPressBtn = true;
    [SerializeField]
    private bool usingToolTips;
    [SerializeField]
    private GameObject toolTip;
    private bool isTransition = false;
    private int previousID;
    private ObjectAudioHandler audioHandler;
    private ControllerNav controlNav;
    private UIElementController previousSelection;
    private bool previewingMedals = false;
    private bool previewingControls = false;

    [SerializeField]
    private Text[] medalTexts;

    bool bonusStats = false;
    bool isRunning = false;

    private void Start()
    {
        // MedalManager.Instance.ReadMedalSaveFile();
       // MedalManager.Instance.ReadMedalSaveFile();
        cameraTransform.position = defaultCameraPoint.position;

        doorAnimation = doorHolder.GetComponent<Animator>();
        audioHandler = GetComponent<ObjectAudioHandler>();
        doorAnimation.enabled = false;
        selectId = 0;
        SetHover();
    }
    private void OnEnable()
    {
        UpdateMedalCounts();
        previewingMedals = false;
        previewingControls = false;
        isTransition = false;
        confirmationMsg.gameObject.SetActive(false);
        quitPanel.gameObject.SetActive(false);
        canPressBtn = true;
        //Closed door 115.504 open door 24.249
        selectId = 0;
        SetHover();
        doorAnimation = doorHolder.GetComponent<Animator>();
        audioHandler = GetComponent<ObjectAudioHandler>();
        doorAnimation.enabled = false;
        if(usingToolTips == true)
        {
            toolTip.SetActive(true);
        }
        else if(usingToolTips == false)
        {
            toolTip.SetActive(false);
        }

        if (SoundManager.Instance != null)
        {
            //SoundManager.Instance.PlayMainTheme();
        }
    }

    private void InputSelect()
    {
        mainMenuButtons[selectId].ChangeState(UIElementState.pressed);
        //audioHandler.SetSFX("Accept");
        if (selectId == 0)
        {
            //StartCoroutine("CameraDown");

            StartCoroutine(MoveCamera(characterSelectPoint, selectId, 0.5f));
        }
        else if (selectId == 1)
        {
            doorAnimation.enabled = true;
            doorAnimation.speed = animationSpeed;
            doorAnimation.SetInteger("DoorAnim", 0);
            //StartCoroutine("CameraIn");
            StartCoroutine(MoveCamera(optionsCameraPoint, selectId, 0.1f));
        }
        else if (selectId == 2)
        {
            quitPanel.gameObject.SetActive(true);
            canPressBtn = false;
        }
        else if (selectId == 3)
        {
            //StopCoroutine("CameraSide");
            StartCoroutine(MoveCamera(medalViewPoint, 2, 0.1f));
            //StopCoroutine("CameraReset");
            //StartCoroutine("CameraSide");
        }
        else if (selectId == 4)
        {
            //StopCoroutine("CameraSide");
            //StopCoroutine("CameraReset");
            //StartCoroutine("CameraControls");

            StartCoroutine(MoveCamera(controlsCameraPoint, 3, 0.1f));
        }
    }
    private IEnumerator CameraDown()
    {
        isTransition = true;
        canPressBtn = false;
        bool arrived = false;
        while (!arrived)
        {
            if (usingLerp == true)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, characterSelectPoint.position, cameraSpeedMedals * Time.deltaTime);
            }
            else
            {
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, characterSelectPoint.position, cameraSpeedMedals * Time.deltaTime);
            }
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, characterSelectPoint.rotation, (cameraSpeedMedals + 0.2f) * Time.deltaTime);
            if (Vector3.Distance(cameraTransform.position, characterSelectPoint.position) < 0.01f) arrived = true;
            yield return null;
        }
        yield return new WaitForSeconds(waitBetweenAnimation);
        isTransition = false;
        MenuController.instance.MainMenuToCharacterSelect();
        yield return null;
    }

    private IEnumerator CameraReset()
    {
        previewingMedals = false;
        previewingControls = false;
        isTransition = true;
        bool arrived = false;
        while (!arrived)
        {
            if (usingLerp == true)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, defaultCameraPoint.position, cameraMoveSpeed * Time.deltaTime);
            }
            else
            {
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, defaultCameraPoint.position, cameraMoveSpeed * Time.deltaTime);
            }
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, defaultCameraPoint.rotation, cameraMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(cameraTransform.position, defaultCameraPoint.position) < 0.1f) arrived = true;
            yield return null;
        }
        yield return new WaitForSeconds(waitBetweenAnimation);
        cameraTransform.transform.position = defaultCameraPoint.transform.position;
        isTransition = false;
        canPressBtn = true;
        yield return null;
    }

    private IEnumerator CameraSide()
    {
        previewingMedals = true;
        isTransition = true;
        canPressBtn = false;
        bool arrived = false;
        while (!arrived)
        {
            if (usingLerp == true)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, medalViewPoint.position, cameraSpeedMedals * Time.deltaTime);
            }
            else
            {
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, medalViewPoint.position, cameraSpeedMedals * Time.deltaTime);
            }
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, medalViewPoint.rotation, cameraSpeedMedals * Time.deltaTime);
            if (Vector3.Distance(cameraTransform.position, medalViewPoint.position) < 0.01f) arrived = true;
            yield return null;
        }
        yield return new WaitForSeconds(waitBetweenAnimation);
        isTransition = false;
        yield return null;
    }

    private IEnumerator CameraControls()
    {
        previewingControls = true;
        isTransition = true;
        canPressBtn = false;
        bool arrived = false;
        while (!arrived)
        {
            if (usingLerp == true)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, controlsCameraPoint.position, cameraMoveSpeed * Time.deltaTime);
            }
            else
            {
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, controlsCameraPoint.position, cameraMoveSpeed * Time.deltaTime);
            }
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, controlsCameraPoint.rotation, cameraMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(cameraTransform.position, controlsCameraPoint.position) < 0.01f) arrived = true;
            yield return null;
        }
        yield return new WaitForSeconds(waitBetweenAnimation);
        isTransition = false;
        yield return null;
    }

    private IEnumerator CameraIn()
        {
        canPressBtn = false;
        isTransition = true;
        yield return new WaitForSeconds(waitBetweenAnimation);
        bool arrived = false;
        while (!arrived)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, optionsCameraPoint.position, cameraMoveSpeed);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, optionsCameraPoint.rotation, cameraMoveSpeed);
            if (Vector3.Distance(cameraTransform.position, optionsCameraPoint.position) < 0.01f) arrived = true;
            yield return null;
        }
        isTransition = false;
        MenuController.instance.MainMenuToOptionsTransition();
        yield return null;
    }

    private void Update()
    {
        if (previewingMedals == true)
        {
            if (Input.GetButtonDown("BackButton"))
            {
                canPressBtn = true;
                //StopCoroutine("CameraSide");
                //StartCoroutine("CameraReset");
                StartCoroutine(MoveCamera(defaultCameraPoint, 4, 0.5f));
            }
            else if (Input.GetButtonDown("Dash") && isTransition == false)
            {
                if (bonusStats == false)
                {
                    StartCoroutine(MoveCamera(medalViewPointB, 5, 0.5f));
                }
                else
                {
                    StartCoroutine(MoveCamera(medalViewPoint, 5, 0.5f));
                    
                }
            }

        }
        else if (previewingControls == true)
        {
            if (Input.GetButtonDown("BackButton"))
            {
                canPressBtn = true;
                //StopCoroutine("CameraControls");
                //StartCoroutine("CameraReset");
                StartCoroutine(MoveCamera(defaultCameraPoint, 4, 0.5f));
            }
        }
        else
        {
            if (canPressBtn == true)
            {

                if (Input.GetButtonDown("Dash"))
                {
                    InputSelect();
                }
                if (Input.GetAxis("Horizontal") < -0.3f)
                {
                    if (isAxis == false)
                    {
                        isAxis = true;
                        selectId++;
                        if (selectId > mainMenuButtons.Length - 1)
                        {
                            selectId = 0;
                        }
                        SetHover();
                    }
                }
                else if (Input.GetAxis("Horizontal") > 0.3f)
                {
                    if (isAxis == false)
                    {
                        isAxis = true;
                        selectId--;
                        if (selectId < 0)
                        {
                            selectId = mainMenuButtons.Length - 1;
                        }
                        SetHover();
                    }
                }
                else if (Input.GetAxis("Vertical") < -0.3f)
                {
                    if (isAxis == false)
                    {

                        previousID = selectId;
                        isAxis = true;
                        if (selectId < 2)
                            selectId = 2;
                        SetHover();
                    }
                }
                else if (Input.GetAxis("Vertical") > 0.3f)
                {
                    if (isAxis == false)
                    {

                        isAxis = true;
                        if (selectId == 2)
                        {
                            selectId = previousID;
                        }
                        SetHover();
                    }
                }
                else
                {
                    isAxis = false;
                }
            }

            if (quitPanel.gameObject.activeInHierarchy == true)
            {
                if (Input.GetButtonDown("BackButton"))
                {
                    ResumeGame();

                    audioHandler.SetSFX("Cancel");
                }
            }
            else
            {
                if (Input.GetButtonDown("BackButton"))
                {
                    if (isTransition == false)
                    {
                        quitPanel.gameObject.SetActive(true);
                        canPressBtn = false;

                        audioHandler.SetSFX("Cancel");
                    }
                }
            }
        }
    }

    private void SetHover()
    {
        if (previousSelection != null)
            previousSelection.ChangeState(UIElementState.inactive);

        mainMenuButtons[selectId].ChangeState(UIElementState.hover);
        previousSelection = mainMenuButtons[selectId];
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        StopCoroutine("SlightDelay");
        StartCoroutine("SlightDelay");
    }

    private IEnumerator SlightDelay()
    {
        quitPanel.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        canPressBtn = true;
        yield return null;
    }

    private void UpdateMedalCounts()
    {
        medalTexts[0].text = ("Stuned Most: " + MedalManager.Instance.totalMedalCounts[0].ToString());
        medalTexts[1].text = ("Most Stuns: " + MedalManager.Instance.totalMedalCounts[1].ToString());
        medalTexts[2].text = ("Most Dashes: " + MedalManager.Instance.totalMedalCounts[2].ToString());
        medalTexts[3].text = ("Most Power Ups: " + MedalManager.Instance.totalMedalCounts[3].ToString());
    }

    private IEnumerator MoveCamera(Transform destination, int transitionNumber, float time)
    {
        if (transitionNumber != 4)
        {
            canPressBtn = false;
        }
        isTransition = true;
        //yield return new WaitForSeconds(waitBetweenAnimation);
        bool arrived = false;

        float t = 0;
        while (t <= time)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, destination.position, t/time);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, destination.rotation, t/time);
            //if (Vector3.Distance(cameraTransform.position, destination.position) < 0.01f) arrived = true;

            t += Time.deltaTime;


            yield return null;
        }
        cameraTransform.position = destination.position;
        cameraTransform.rotation = destination.rotation;
        isTransition = false;
        arrived = true;
        switch (transitionNumber)
        {
            case (0):
                {
                    MenuController.instance.MainMenuToCharacterSelect();
                    break;
                }
            case (1):
                {
                    MenuController.instance.MainMenuToOptionsTransition();
                    break;
                }
            case (2):
                {
                    previewingMedals = true;
                    break;
                }
            case (3):
                {
                    previewingControls = true;
                    break;
                }
            case (4):
                {
                    previewingMedals = false;
                    previewingControls = false;

                    break;
                }
            case (5):
                {
                    bonusStats = !bonusStats;
                    break;
                }
        }

        //canPressBtn = true;

        yield return null;
    }
}
