/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 22/11/2019
 * Last Modified: 21/11/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewCharacterSelectionController : MonoBehaviour
{
    private bool canPressBtn = true;
    [SerializeField]
    private Transform doorHolder;
    [SerializeField]
    private Transform cameraTransform, mainMenuCameraPoint;
    [SerializeField]
    private float waitBetweenAnimation = 0.5f;
    [SerializeField]
    private float cameraMoveSpeed = 0.2f;
    [SerializeField]
    private bool usingLerp = true;
    [SerializeField]
    private Transform[] pinLocations, playerPins, pinController;
    [SerializeField]
    private Player[] players;
    private int minPlayers = 2;

    private void OnEnable()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].isActivated = false;
            players[i].isLocked = false;
            players[i].playerScore = 0;
        }
        for (int i = 0; i < playerPins.Length; i++)
        {
            playerPins[i].transform.position = pinLocations[i].transform.position;
        }
        canPressBtn = true;
    }


    private void Update()
    {
        if (canPressBtn == true)
        {
            if (Input.GetButtonDown("BackButton"))
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
        Debug.Log(canStart + " and ready players num: " + readyPlayers);
        if (canStart == true && readyPlayers >= 2)
        {
            RandomMap();
        }
    }

    private void RandomMap()
    {
        int randomNumber = Random.Range(1, 6);
        SceneManager.LoadScene("Level" + randomNumber);
    }


    private void ReturnToMainMenu()
    {
        StartCoroutine("CameraUp");
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
