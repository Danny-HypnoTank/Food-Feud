using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private Loading LoadScript;

    [Header("Position References")]
    [SerializeField]
    private Transform cameraPosition;
    [SerializeField]
    private Transform[] levelViewPoints;

    [Header("Movement Properties")]
    [SerializeField]
    private float cameraMoveSpeed;
    [SerializeField]
    private int levelIndex;
    private int previousIndex;

    [SerializeField]
    private bool canPressButton = true;
    [SerializeField]
    private bool isAxis = false;
    [SerializeField]
    private bool inTransition = false;

    [Header("Door Controls and Animation")]
    [SerializeField]
    private Animator doorAnimation;
    [SerializeField]
    private float waitBetweenAnimation = 0.5f;
    [SerializeField]
    private float animationSpeed = 0.5f;
    private PinReset resetPin;

    private void OnEnable()
    {
        resetPin = GameObject.Find("ScriptControl").GetComponent<PinReset>();
        
        levelIndex = 0;
        canPressButton = true;
        isAxis = false;
        inTransition = false;
        StartCoroutine("OpenFridge");
    }



    private void Update()
    {
        if (inTransition == false)
        {
            if (Input.GetButtonDown("Dash"))
            {
                SelectLevel();
            }

            if (Input.GetButton("BackButton"))
            {
                resetPin.CallReset();
                StartCoroutine(CloseFridge());

            }
        }
        InputLevelSelect();
    }

    private void InputLevelSelect()
    {
        if (Input.GetAxis("Horizontal") > 0.5f)
        {
            if (isAxis == false)
            {
                if (canPressButton == true)
                {
                    isAxis = true;
                    levelIndex++;
                    if (levelIndex == levelViewPoints.Length)
                    {
                        levelIndex = 0;
                    }
                    CameraMovement();
                }
            }
        }
        else  if (Input.GetAxis("Horizontal") < -0.5f)
        {
            if (isAxis == false)
            {
                if (canPressButton == true)
                {
                    isAxis = true;
                    levelIndex--;
                    if (levelIndex < 0)
                    {
                        levelIndex = levelViewPoints.Length;
                    }
                    CameraMovement();
                }
            }
        }
        else
        {
            isAxis = false;
        }
       
    }

    private void GetControllerInput()
    {
        if (!isAxis)
        {
            int selection = (int)Input.GetAxis("Horizontal1");
           // isAxis = true;
            if (canPressButton == true)
            {
                previousIndex = levelIndex;
                levelIndex += selection;
                if (levelIndex < 0)
                {
                    levelIndex = levelViewPoints.Length - 1;
                }
                if (levelIndex == levelViewPoints.Length)
                {
                    levelIndex = 0;
                }
                CameraMovement();
            }
        }
    }

    private void ReturnToCharacterSelect()
    {
        MenuController.instance.LevelSelectToCharacterSelection();
    }

    //Selection of level
    private void SelectLevel()
    {
        if (levelIndex == 0)
        {
            SceneManager.LoadScene(7);
        }
        else if (levelIndex == 1)
        {
            SceneManager.LoadScene(8);
            
            //loadingManager.SetID(4);
            //loadingManager.InitializeLoading();
        }
        else if (levelIndex == 2)
        {
            SceneManager.LoadScene(9);

            //loadingManager.SetID(5);
            //loadingManager.InitializeLoading();
        }
    }

    private IEnumerator OpenFridge()
    {
        yield return new WaitForEndOfFrame();
        doorAnimation.enabled = true;
        doorAnimation.speed = animationSpeed;
        doorAnimation.SetInteger("CloseAnim", 1);
        canPressButton = false;
        inTransition = true;
        yield return new WaitForSeconds(waitBetweenAnimation);
        inTransition = false;
        canPressButton = true;
        yield return null;
    }

    private IEnumerator CloseFridge()
    {
        yield return new WaitForEndOfFrame();
        doorAnimation.enabled = true;
        doorAnimation.speed = animationSpeed;
        doorAnimation.SetInteger("CloseAnim", 2);
        canPressButton = false;
        inTransition = true;
        yield return new WaitForSeconds(waitBetweenAnimation);
        inTransition = false;
      //  yield return new WaitForSeconds(4);
        ReturnToCharacterSelect();
    }

    #region Level Select Camera Control
    //Method for moving around view points
    private void CameraMovement()
    {
        if (inTransition == false)
        {
            switch (levelIndex)
            {
                case 0://Fridge Movement
                    StartCoroutine(CameraFridge());
                    break;
                case 1://Bin Movement
                    StartCoroutine(CameraBin());
                    break;
                case 2://Sink Movement
                    StartCoroutine(CameraSink());
                    break;
            }
        }
    }

    //Coroutine for moving to fridge
    private IEnumerator CameraFridge()
    {
        canPressButton = false;
        inTransition = true;
        bool arrived = false;
        while (!arrived)
        {
            cameraPosition.position = Vector3.Lerp(cameraPosition.position, levelViewPoints[0].position, cameraMoveSpeed * Time.deltaTime);
            cameraPosition.rotation = Quaternion.Slerp(cameraPosition.rotation, levelViewPoints[0].rotation, cameraMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(cameraPosition.position, levelViewPoints[0].position) < 0.01f)
            {
                arrived = true;
            }
            yield return null;
        }
        inTransition = false;
        canPressButton = true;
        isAxis = false;
        yield return null;
    }

    //Coroutine for moving to bin
    private IEnumerator CameraBin()
    {
        canPressButton = false;
        inTransition = true;
        bool arrived = false;
        while (!arrived)
        {
            cameraPosition.position = Vector3.Lerp(cameraPosition.position, levelViewPoints[1].position, cameraMoveSpeed * Time.deltaTime);
            cameraPosition.rotation = Quaternion.Slerp(cameraPosition.rotation, levelViewPoints[1].rotation, cameraMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(cameraPosition.position, levelViewPoints[1].position) < 0.01f)
            {
                arrived = true;
            }
            yield return null;
        }
        inTransition = false;
        canPressButton = true;
        isAxis = false;
        yield return null;
    }

    //Coroutine for moving to sink
    private IEnumerator CameraSink()
    {
        canPressButton = false;
        inTransition = true;
        bool arrived = false;
        while (!arrived)
        {
            cameraPosition.position = Vector3.Lerp(cameraPosition.position, levelViewPoints[2].position, cameraMoveSpeed * Time.deltaTime);
            cameraPosition.rotation = Quaternion.Slerp(cameraPosition.rotation, levelViewPoints[2].rotation, cameraMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(cameraPosition.position, levelViewPoints[2].position) < 0.01f)
            {
                arrived = true;
            }
            yield return null;
        }
        inTransition = false;
        canPressButton = true;
        isAxis = false;
        yield return null;
    }
#endregion
}
