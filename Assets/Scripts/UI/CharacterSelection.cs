/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 29/09/2019
 * Last Modified: 06/10/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private Transform characterSelectionPanel, mapselectionPanel, confirmSelectionPanel;                    //stores all the panels
    [SerializeField]
    private Player[] players;                                                                            //stores all players
    private bool isConfirminationOn;
    private MainMenu mainMenu;
    private ControllerInputDetection controllerInput;
    [SerializeField]
    private int[] skinNumberIterations;

    public bool IsConfirminationOn { get => isConfirminationOn; set => isConfirminationOn = value; }

    //resets all variables
    private void OnEnable()
    {
        skinNumberIterations = new int[4];
        isConfirminationOn = false;
        confirmSelectionPanel.gameObject.SetActive(false);
        characterSelectionPanel.gameObject.SetActive(true);
        mapselectionPanel.gameObject.SetActive(false);
        for (int i = 0; i < players.Length; i++)
        {
            players[i].isActivated = false;
            players[i].isLocked = false;
        }
        for (int i = 0; i < skinNumberIterations.Length; i++)
        {
            skinNumberIterations[i] = 9999;
        }
    }

    private void Start()
    {
        mainMenu = gameObject.GetComponentInParent<MainMenu>();
        controllerInput = GameObject.Find("EventSystem").GetComponent<ControllerInputDetection>();
    }

    //Moves to map selection
    public void GoToLevelSelect()
    {
        characterSelectionPanel.gameObject.SetActive(false);
        mapselectionPanel.gameObject.SetActive(true);
        confirmSelectionPanel.gameObject.SetActive(false);
    }

    //checks if all players are locked in
    public void AllPlayersReadyCheck()
    {
        bool canStart = true;
        int activePlayer = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if(players[i].isActivated == true)
            {
                if(players[i].isLocked == false)
                {
                    canStart = false;
                }
                else if (players[i].isLocked == true)
                {
                    activePlayer++;
                }
            }
        }
        if (canStart == true)
        {
            if (activePlayer >= 2)
            {
                confirmSelectionPanel.gameObject.SetActive(true);
                isConfirminationOn = true;
            }
        }
    }

    //takes in controller inputs
    private void Update()
    {
        if (isConfirminationOn == true)
        {
            if (Input.GetButtonDown("Submit"))
            {
                ConfirmSelectionOfCharacters();
            }
            if (Input.GetButtonDown("BackButton"))
            {
                CancelSelectionOfCharacters();
            }
        }
        else if(isConfirminationOn == false)
        {
            if (Input.GetButtonDown("BackButton"))
            {
                mainMenu.InitializeMenu();
            }
        }
    }

    //returns to character selection panel
    public void CancelSelectionOfCharacters()
    {     
        confirmSelectionPanel.gameObject.SetActive(false);
        isConfirminationOn = false;
    }

    //enables level select panel
    public void ConfirmSelectionOfCharacters()
    {
        mapselectionPanel.gameObject.SetActive(true);
        confirmSelectionPanel.gameObject.SetActive(false);
        characterSelectionPanel.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public bool CharacterIterationCheck(int iteration, int playerNum)
    {
        bool alreadyExists = false;

        //skinNumberIterations[playerNum] = iteration;

        for (int i = 0; i < skinNumberIterations.Length; i++)
        {
            if (players[i].isActivated == true)
            {
                if (iteration == skinNumberIterations[i])
                {
                    alreadyExists = true;
                }
            }
        }

        if (alreadyExists == true)
        {
        }
        else
        {
            skinNumberIterations[playerNum] = iteration;
        }
        return alreadyExists;
    }

    public void cancelLock(int playerNum)
    {
        //skinNumberIterations[playerNum] = 9999;
    }
}
