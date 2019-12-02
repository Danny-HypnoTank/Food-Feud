﻿using System.Collections;
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
    private int selectId;
    [SerializeField]
    private bool isAxis = false;
    private bool isSlider;

    private void OnEnable()
    {
        for (int i = 0; i < Sliderbox.Length; i++)
        {
            Sliderbox[i].GetComponent<CustomSlider>().enabled = true;
        }
        for (int i = 0; i < Sliderbox.Length; i++)
        {
            Sliderbox[i].GetComponent<CustomSlider>().enabled = false;
        }
        canPressBtn = true;
        isAxis = false;
        selectId = 0;
        doorAnimation = doorHolder.GetComponent<Animator>();
        doorAnimation.enabled = false;
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
                    Sliderbox[selectId].GetComponent<CustomSlider>().SaveSettings();
                    ResetSliders();
                }
            }

            if (isSlider == false)
            {
                if (Input.GetAxis("Horizontal") < -0.3f)
                {
                    if (isAxis == false)
                    {
                        isAxis = true;
                        selectId++;
                        if (selectId > Sliderbox.Length - 1)
                        {
                            selectId = Sliderbox.Length - 1;
                        }

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
            Sliderbox[i].GetComponent<CustomSlider>().enabled = false;
        }
    }

    private void SelectBox()
    {
        isSlider = true;
        Sliderbox[selectId].GetComponent<CustomSlider>().enabled = true;
    }
    private void ReturnToMainMenu()
    {
        doorAnimation.enabled = true;
        StartCoroutine("CameraIn");
    }


    private IEnumerator CameraIn()
    {
        canPressBtn = false;
        
        bool arrived = false;
        while (!arrived)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, mainMenuCameraPoint.position, cameraMoveSpeed);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, mainMenuCameraPoint.rotation, cameraMoveSpeed);
            if (Vector3.Distance(cameraTransform.position, mainMenuCameraPoint.position) < 0.1f) arrived = true;
            yield return null;
        }
        doorAnimation.speed = animationSpeed;
        doorAnimation.SetInteger("DoorAnim", 1);
        yield return new WaitForSeconds(waitBetweenAnimation);
        MenuController.instance.OptionsMenuToMainMenu();
        yield return null;
    }
}
