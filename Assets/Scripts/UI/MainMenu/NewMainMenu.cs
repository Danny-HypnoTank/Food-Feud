/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 21/11/2019
 * Last Modified: 21/11/2019
 */
using System.Collections;
using UnityEngine;

public class NewMainMenu : MonoBehaviour
{
    [Header("Main menu buttons and sprites")]
    [SerializeField]
    private Transform[] mainMenuButtons;
    [SerializeField]
    private Transform quitPanel;
    [SerializeField]
    private Sprite[] defaultSprites, hoverSprites;
    [SerializeField]
    private SpriteRenderer[] imageOfSprites;
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
    private Transform cameraTransform, optionsCameraPoint, characterSelectPoint;
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
    private bool canPressBtn = true;
    [SerializeField]
    private bool usingToolTips;
    [SerializeField]
    private GameObject toolTip;
    private void OnEnable()
    {
        quitPanel.gameObject.SetActive(false);
        canPressBtn = true;
        //Closed door 115.504 open door 24.249
        selectId = 0;
        SetHover();
        doorAnimation = doorHolder.GetComponent<Animator>();
        doorAnimation.enabled = false;
        if(usingToolTips == true)
        {
            toolTip.SetActive(true);
        }
        else if(usingToolTips == false)
        {
            toolTip.SetActive(false);
        }
    }

    private void InputSelect()
    {
        if(selectId == 0)
        {
            StartCoroutine("CameraDown");
        }
        else if(selectId == 1)
        {
                doorAnimation.enabled = true;
                doorAnimation.speed = animationSpeed;
                doorAnimation.SetInteger("DoorAnim", 0);
                StartCoroutine("CameraIn");
        }
        else if(selectId == 2)
        {
            quitPanel.gameObject.SetActive(true);
            canPressBtn = false;
        }
    }
    private IEnumerator CameraDown()
    {
        canPressBtn = false;
        bool arrived = false;
        while (!arrived)
        {
            if (usingLerp == true)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, characterSelectPoint.position, cameraMoveSpeed * Time.deltaTime);
            }
            else
            {
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, characterSelectPoint.position, cameraMoveSpeed * Time.deltaTime);
            }
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, characterSelectPoint.rotation, cameraMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(cameraTransform.position, characterSelectPoint.position) < 0.1f) arrived = true;
            yield return null;
        }
        MenuController.instance.MainMenuToCharacterSelect();
        yield return null;
    }

    
        private IEnumerator CameraIn()
        {
        canPressBtn = false;
        yield return new WaitForSeconds(waitBetweenAnimation);
        bool arrived = false;
        while (!arrived)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, optionsCameraPoint.position, cameraMoveSpeed);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, optionsCameraPoint.rotation, cameraMoveSpeed);
            if (Vector3.Distance(cameraTransform.position, optionsCameraPoint.position) < 0.1f) arrived = true;
            yield return null;
        }
        MenuController.instance.MainMenuToOptionsTransition();
        yield return null;
    }

    private void Update()
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
                    if (selectId > mainMenuButtons.Length -1)
                    {
                        selectId = mainMenuButtons.Length -1;
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
                        selectId = 0;
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
            }
        }
        else
        {
            if (Input.GetButtonDown("BackButton"))
            {
                quitPanel.gameObject.SetActive(true);
                canPressBtn = false;
            }
        }
    }

    private void SetHover()
    {
        for (int i = 0; i < imageOfSprites.Length; i++)
        {
            imageOfSprites[i].sprite = defaultSprites[i];
        }
        imageOfSprites[selectId].sprite = hoverSprites[selectId];
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
}
