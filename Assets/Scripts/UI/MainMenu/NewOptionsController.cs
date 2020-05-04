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

public class NewOptionsController : MonoBehaviour
{
    [Header("Handles animation for moving out of upper fridge")]
    private Animator doorAnimation;
    [SerializeField]
    private Transform doorHolder;
    [SerializeField]
    private Transform cameraTransform, mainMenuCameraPoint;
    [SerializeField]
    private float waitBetweenAnimation = 0.5f;
    [SerializeField]
    private float cameraMoveSpeed = 0.2f;
    [SerializeField]
    private float animationSpeed = 0.5f;
    private bool canPressBtn = true;
    [Header("Logic")]
    [SerializeField]
    private GameObject[] Sliderbox;
    [SerializeField]
    private GameObject[] sliderHover;
    [SerializeField]
    private int selectId;
    [SerializeField]
    private bool isAxis = false;
    private bool isSlider;
    private int previousId;
    private UIElementController previousSelection;

    private void OnEnable()
    {
        for (int i = 0; i < Sliderbox.Length; i++)
        {
            //Sliderbox[i].GetComponent<CustomSlider>().enabled = true;
            Sliderbox[i].GetComponent<CustomSlider>().inUse = true; ;
        }
        for (int i = 0; i < Sliderbox.Length; i++)
        {
            //Sliderbox[i].GetComponent<CustomSlider>().enabled = false;
            Sliderbox[i].GetComponent<CustomSlider>().inUse =  false;
        }
        canPressBtn = true;
        isAxis = false;
        selectId = 0;
        SetHover();
        doorAnimation = doorHolder.GetComponent<Animator>();
        doorAnimation.enabled = false;
        sliderHover[0].SetActive(true);
    }

    private void ResetSliderHovers()
    {
        for (int i = 0; i < sliderHover.Length; i++)
        {
            sliderHover[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (canPressBtn == true)
        {
            if (Input.GetButtonDown("Dash"))
            {
                if (isSlider == false)
                {
                    SelectBox();
                }
                else if(isSlider == true)
                {
                    SetHover();
                    ResetSliders();
                }
            }

            if (isSlider == false)
            {
                if (Input.GetAxis("Horizontal1") > 0.3f)
                {
                    if (isAxis == false)
                    {

                        isAxis = true;
                        selectId++;
                        if (selectId > Sliderbox.Length - 1)
                        {
                            selectId = 0;
                        }
                        ResetSliderHovers();
                        SetHover();
                        sliderHover[selectId].SetActive(true);
                    }
                }
                else if (Input.GetAxis("Horizontal1") < -0.3f)
                {
                    if (isAxis == false)
                    {

                        isAxis = true;
                        selectId--;
                        if (selectId < 0)
                        {
                            selectId = Sliderbox.Length -1;
                        }
                        ResetSliderHovers();
                        SetHover();
                        sliderHover[selectId].SetActive(true);

                    }
                }
                else if(Input.GetAxis("Vertical1") > 0.3f)
                {
                    if (isAxis == false)
                    {
                        previousId = selectId;

                        isAxis = true;
                        if (selectId > 0)
                        {
                            selectId = 0;
                        }
                        SetHover();
                    }
                }
                else if (Input.GetAxis("Vertical1") < -0.3f)
                {
                    if (isAxis == false)
                    {
                        isAxis = true;
                        if (selectId == 0)
                        {
                            selectId = previousId;
                        }
                        SetHover();
                    }
                }
                else
                {
                    isAxis = false;
                }
            }
            if (Input.GetButtonDown("BackButton"))
            {
                if (isSlider == false)
                {
                    ReturnToMainMenu();
                }
                else
                {
                    Sliderbox[selectId].GetComponent<CustomSlider>().CancelChanges();
                    ResetSliders();
                }
            }
        }
    }

    private void ResetSliders()
    {
        isSlider = false;
        for (int i = 0; i < Sliderbox.Length; i++)
        {
            Sliderbox[i].GetComponent<CustomSlider>().inUse = false;
        }
    }

    private void SelectBox()
    {
        isSlider = true;
        Sliderbox[selectId].GetComponent<CustomSlider>().inUse = true;
        Sliderbox[selectId].GetComponentInChildren<UIElementController>().ChangeState(UIElementState.pressed);
    }

    private void SetHover()
    {
        if(previousSelection != null)
            previousSelection.ChangeState(UIElementState.inactive);

        Sliderbox[selectId].GetComponentInChildren<UIElementController>().ChangeState(UIElementState.hover);
        previousSelection = Sliderbox[selectId].GetComponentInChildren<UIElementController>();
    }

    private void ReturnToMainMenu()
    {
        doorAnimation.enabled = true;
        //StartCoroutine("CameraIn");
        StartCoroutine(MoveCamera(mainMenuCameraPoint, 0.5f));
    }


    private IEnumerator CameraIn()
    {
        canPressBtn = false;
        
        bool arrived = false;
        while (!arrived)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, mainMenuCameraPoint.position, cameraMoveSpeed);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, mainMenuCameraPoint.rotation, cameraMoveSpeed);
            if (Vector3.Distance(cameraTransform.position, mainMenuCameraPoint.position) < 0.01f) arrived = true;
            yield return null;
        }
        doorAnimation.speed = animationSpeed;
        doorAnimation.SetInteger("DoorAnim", 1);
        yield return new WaitForSeconds(waitBetweenAnimation);
        MenuController.instance.OptionsMenuToMainMenu();
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
        doorAnimation.speed = animationSpeed;
        doorAnimation.SetInteger("DoorAnim", 1);
        yield return new WaitForSeconds(waitBetweenAnimation);
        cameraTransform.position = destination.position;
        cameraTransform.rotation = destination.rotation;
        arrived = true;
        MenuController.instance.OptionsMenuToMainMenu();

        //canPressBtn = true;

        yield return null;
    }
}
