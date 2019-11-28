/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 29/09/2019
 * Last Modified: 29/10/2019
 * Modified by: Danny Pym-Hember, Dominik Waldowski, Alexander Watson
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Display Panels")]
    [SerializeField]
    private Transform optionsMenuPanel, quitPanel, characterSelectPanel, mainMenuPanel, mapPanel, playerSelectModels, creditsPanel;         //stores all panels within first scene.
    private ControllerInputDetection controllerInput;
    [Header("Navigation")]
    [SerializeField]
    private GameObject mainMenuBtn, optionsMenuBtn, quitBtn;             //buttons that will be used for controller interactions.
    private void Start()
    {
        controllerInput = GameObject.Find("EventSystem").GetComponent<ControllerInputDetection>();
        InitializeMenu();
        SoundManager.Instance.PlayMainTheme();
    }
    private void Update()
    {
        if (characterSelectPanel.gameObject.activeInHierarchy == false || mapPanel.gameObject.activeInHierarchy == false)
        {
            if (Input.GetButtonDown("BackButton"))
            {
                InitializeMenu();
            }
        }
    }
    //Opens character selection menu
    public void StartGame()
    {
        characterSelectPanel.gameObject.SetActive(true);
        playerSelectModels.gameObject.SetActive(true);
        mainMenuPanel.gameObject.SetActive(false);
    }

    //Opens options menu
    public void OptionsMenu()
    {
        mainMenuPanel.gameObject.SetActive(false);
        optionsMenuPanel.gameObject.SetActive(true);
        creditsPanel.gameObject.SetActive(false);
        controllerInput.SetMainButton(optionsMenuBtn);
    }

    //Closes the application
    public void CloseGame()
    {
        Application.Quit();
    }

    //When user selects not to close game it closes this panel instead.
    public void CancelCloseGame()
    {
        InitializeMenu();
    }

    //Displays the menu to quit game or not
    public void QuitGameBtn()
    {
        quitPanel.gameObject.SetActive(true);
        mainMenuPanel.gameObject.SetActive(false);
        controllerInput.SetMainButton(quitBtn);
    }

    public void CreditsMenu()
    {
        mainMenuPanel.gameObject.SetActive(false);
        creditsPanel.gameObject.SetActive(true);
    }

    //Turns all menus off apart from main menu NOTE: this can be called to return from options menu back to main menu.
    public void InitializeMenu()
    {
        optionsMenuPanel.gameObject.SetActive(false);
        mapPanel.gameObject.SetActive(false);
        characterSelectPanel.gameObject.SetActive(false);
        creditsPanel.gameObject.SetActive(false);
        quitPanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(true);
        playerSelectModels.gameObject.SetActive(false);
        controllerInput.SetMainButton(mainMenuBtn);
    }
}
