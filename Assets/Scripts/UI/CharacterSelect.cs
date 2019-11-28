/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 29/09/2019
 * Last Modified: 29/10/2019
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField]
    private Player player;                                //gets all player data
    [SerializeField]
    private int iteration;                                //for cycling between each skin
    [SerializeField]
    private Text pressToActivateTxt;                                 //temporary way to display skins with text
    [SerializeField]
    private Transform activatedPlayerPanel;             //activates panel to select the characer
    [SerializeField]
    private CharacterSelection characterSelection;
    [SerializeField]
    private GameObject lockBtn;
    private bool isAxisInUse = false;
    [SerializeField]
    private List<GameObject> skins = new List<GameObject>();
    [SerializeField]
    private Transform skinHolder;
    private void OnEnable()
    {
        characterSelection = GameObject.Find("CharacterSelectionPanel").GetComponent<CharacterSelection>();
        Initializers();
        player.playerScore = 0;
        activatedPlayerPanel.gameObject.SetActive(false);
    }

    private void Initializers()
    {
        skins.Clear();
        foreach (Transform child in skinHolder)
        {
            skins.Add(child.gameObject);
        }
        player.isLocked = true;
        LockCheck();
        player.isActivated = false;
        iteration = 0;
        pressToActivateTxt.text = "Press Start To Join";
        pressToActivateTxt.gameObject.SetActive(true);
        DisableSkins();
    }

    private void DisableSkins()
    {
        for (int i = 0; i < skins.Count; i++)
        {
            skins[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Activate" + player.playerNum))
        {
            ActivatePlayer();
        }
        if(Input.GetButtonDown("Dash" + player.playerNum))
        {
                LockSelection();
            
        }
        if(Input.GetAxis("Horizontal" + player.playerNum) > 0.1f)
        {
            if (isAxisInUse == false)
            {
                isAxisInUse = true;
                NextCharacter();
            }
        }
        else if (Input.GetAxis("Horizontal" + player.playerNum) < -0.1f)
        {
            if (isAxisInUse == false)
            {
                isAxisInUse = true;
                PreviousCharacter();
            }
        }
        else if(Input.GetAxis("Horizontal" + player.playerNum) == 0)
        {
            isAxisInUse = false;
        }
    }

        //selects next skin
        public void NextCharacter()
    {
        if (player.isLocked == false)
        {
            iteration++;
            if (iteration > player.namesOfSkins.Length - 1)
            {
                iteration = 0;
            }
            DisplaySkin();
        }
    }

    //locks selection not allowing user to change character
    public void LockSelection()
    {
        if (characterSelection.IsConfirminationOn == false)
        {
            if (!characterSelection.CharacterIterationCheck(iteration, player.playerNum - 1))
            {
                LockCheck();
            }
            else if (player.isLocked == true)
            {
                player.isLocked = false;
                lockBtn.GetComponent<Image>().color = Color.white;
                characterSelection.cancelLock(player.playerNum - 1);
            }
        }

    }

    private void LockCheck()
    {
        if (player.isLocked == true)
        {
            player.isLocked = false;
            lockBtn.GetComponent<Image>().color = Color.white;
            characterSelection.cancelLock(player.playerNum - 1);
            player.skinId = iteration;
        }
        else if (player.isLocked == false)
        {
            player.isLocked = true;
            characterSelection.AllPlayersReadyCheck();
            lockBtn.GetComponent<Image>().color = Color.red;
        }
    }


    //Selects previous skin
    public void PreviousCharacter()
    {
        if (player.isLocked == false)
        {
            iteration--;
            if (iteration < 0)
            {
                iteration = player.namesOfSkins.Length - 1;
            }
            DisplaySkin();
        }
    }


    //displays visually selected player skin and updates data to scriptable object
    private void DisplaySkin()
    {
        DisableSkins();
        player.skinId = iteration;
        skins[iteration].gameObject.SetActive(true);
    }


    //enables or disables panel for active player
    public void ActivatePlayer()
    {
        if (characterSelection.IsConfirminationOn == false)
        {
            if (player.isActivated == true)
            {
                activatedPlayerPanel.gameObject.SetActive(false);
                player.isActivated = false;
                pressToActivateTxt.gameObject.SetActive(true);
                player.isLocked = true;
            }
            else if (player.isActivated == false)
            {
                activatedPlayerPanel.gameObject.SetActive(true);
                player.isActivated = true;
                pressToActivateTxt.gameObject.SetActive(false);
                player.isLocked = false;
                DisplaySkin();
            }
        }
    }




}
