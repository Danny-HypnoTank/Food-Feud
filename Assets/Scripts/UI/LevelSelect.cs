/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 06/10/2019
 * Date Modified: 22/10/2019
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject mapbtn;                                      //used for input control(assign any button that will be first interactable button
    private ControllerInputDetection controllerInput;               //stores reference to input class
    private MainMenu mainMenu;                                      //stores reference to main menu
    private int iteration = 0;                                      //iteration to find out what map is currently picked
    [SerializeField]
    private MapDisplay[] listOfMaps;                                //stores all maps and their info
    private float increment = 15;                                   //by how much time increments in seconds
    private float minTime = 30;                                     //minimum time value
    private float maxTime = 90;                                     //maximum time value
    private float currentTime = 60;                                 //current round time
    [SerializeField]
    private Text currentTimeText;                                   //text that displays current time
    [SerializeField]
    private Image currentMapImg;                                    //image that displays map visuals
    [SerializeField]
    private Text mapNameText;                                       //text that stores mapName
    // Start is called before the first frame update
    private void Start()
    {
        mainMenu = gameObject.GetComponentInParent<MainMenu>();
        SetEventSys();
    }

    private void SetEventSys()
    {
        controllerInput = GameObject.Find("EventSystem").GetComponent<ControllerInputDetection>();
    }

    private void OnEnable()
    {
        if(controllerInput == null)
        {
            SetEventSys();
        }
        controllerInput.SetMainButton(mapbtn);
        iteration = 0;
        currentTime = 60;
        UpdateTimeData();
        UpdateMapData();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("BackButton"))
        {
            mainMenu.InitializeMenu();
        }
    }

    //sets next map
    public void NextMap()
    {
        iteration++;
        if (iteration > listOfMaps.Length - 1)
        {
            iteration = 0;
        }
        UpdateMapData();
    }

    //sets previous map
    public void PreviousMap()
    {
        iteration--;
        if (iteration < 0)
        {
            iteration = listOfMaps.Length - 1;
        }
        UpdateMapData();
    }

    //increases the time by 15
    public void IncreaseTime()
    {
        currentTime += increment;
        if(currentTime > maxTime)
        {
            currentTime = maxTime;
        }
        currentTimeText.text = "Round Time: " + currentTime;
    }

    //Updates map visuals
    private void UpdateMapData()
    {
        currentMapImg.sprite = listOfMaps[iteration].mapImg;
        mapNameText.text = listOfMaps[iteration].mapName;
        if (iteration != listOfMaps[iteration].mapId)
        {
            Debug.LogWarning("Iteration doesnt match map ID, listOfMaps Order is incorrect!");
        }
    }

    //updates time display
    private void UpdateTimeData()
    {
        currentTimeText.text = "Round Time: " + currentTime;
    }

    //Decreeases the time by 15
    public void DecreaseTime()
    {
        currentTime -= increment;
        if (currentTime < minTime)
        {
            currentTime = minTime;
        }
        currentTimeText.text = "Round Time: " + currentTime;
    }

    //starts the game
    public void StartGameFromLevel()
    {
        PlayerPrefs.SetFloat("RoundDuration", currentTime);
        PlayerPrefs.SetInt("MapID", iteration);
        SceneManager.LoadScene("GameScene" + iteration);
    }
}
