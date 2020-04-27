﻿/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 01/10/2019
 * Last Modified: 16/02/2020
 * Modified By: Antoni Gudejko, Dominik Waldowski, Alex Watson
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScore : MonoBehaviour
{
    [SerializeField]
    private Player[] players;
    [SerializeField]
    private GameObject[] thePlayerData;
    [SerializeField]
    private List<Player> sortedPlayers = new List<Player>();    //stores a sorted list of players by score
    [SerializeField]
    private Transform[] podiumLocations;
    [SerializeField]
    private GameObject[] winnerIcons;
    [SerializeField]
    private UIElementControllerImage[] buttons;

    private int usedPodiums;
    private int selectID;
    private bool canUseInput;
    private bool isAxis;
    private float total;
    private Loading loading;
    private UIElementControllerImage previousSelection;

    MedalManager medalManager;

    [SerializeField]
    private int sceneToLoad;

    //Reseting 
    private void Awake()
    {
        players[0].hasWon = false;
        players[1].hasWon = false;
        players[2].hasWon = false;
        players[3].hasWon = false;
        Debug.Log("Reset has won");
        sortedPlayers.Clear();
        selectID = 0;
    }
    private void Start()
    {

        medalManager = MedalManager.Instance;
        canUseInput = false;

        //loading = GameObject.Find("LoadManager").GetComponent<Loading>();
        sortedPlayers = players.OrderByDescending(o => o.playerScore).ToList();
        //sortedPlayers = players.OrderByDescending(o => o.scorePercentage).ToList();

        #region stunMedal
        int _currentPlayer = 0;

        int[] _medalChosen = { 0, 0, 0, 0, 0 };

        bool[] _medalClaimed = { false, false, false, false, false };

        //int[] _medalsPerPlayer = { 0, 0, 0, 0 };

        Dictionary<int, int> _medalsPerPlayer = new Dictionary<int, int>();

        Dictionary<int, int> _medalForPlayer = new Dictionary<int, int>();

        foreach (Player p in sortedPlayers)
        {
            int _medalAmount = 0;

            if (p.playerNum == 0)
            {
                _medalAmount++;
            }

            if (p.playerNum == medalManager.GetTopStunned())
            {
                _medalAmount++;
            }

            if (p.playerNum == medalManager.GetTopStunOther())
            {
                _medalAmount++;
            }

            if (p.playerNum == medalManager.GetTopDashes())
            {

                _medalAmount++;
            }

            if (p.playerNum == medalManager.GetTopPowerPickup())
            {
                _medalAmount++;
            }

            if (p.playerNum == medalManager.GetUntouchable())
            {
                _medalAmount++;
            }

            _medalsPerPlayer.Add(p.playerNum, _medalAmount);
        }

        var _medalList = from pair in _medalsPerPlayer
                         orderby pair.Value ascending
                         select pair;


        foreach (KeyValuePair<int, int> pair in _medalList)
        {


            if (pair.Key == medalManager.GetTopStunned() && _medalClaimed[0] == false)
            {
                if (pair.Key != 0)
                {
                    _medalClaimed[0] = true;
                    _medalForPlayer.Add(pair.Key, 0);
                }
            }
            else if (pair.Key == medalManager.GetTopStunOther() && _medalClaimed[1] == false)
            {
                if (pair.Key != 0)
                {
                    _medalClaimed[1] = true;
                    _medalForPlayer.Add(pair.Key, 1);
                }
            }
            else if (pair.Key == medalManager.GetTopDashes() && _medalClaimed[2] == false)
            {
                if (pair.Key != 0)
                {
                    _medalClaimed[2] = true;
                    _medalForPlayer.Add(pair.Key, 2);
                }
            }
            else if (pair.Key == medalManager.GetTopPowerPickup() && _medalClaimed[3] == false)
            {
                if (pair.Key != 0)
                {
                    _medalClaimed[3] = true;
                    _medalForPlayer.Add(pair.Key, 3);
                }
            }
            else if (pair.Key == medalManager.GetUntouchable() && _medalClaimed[4] == false)
            {

                _medalClaimed[4] = true;
                _medalForPlayer.Add(pair.Key, 4);

            }
        }

        foreach (KeyValuePair<int, int> pair in _medalForPlayer)
        {
            medalManager.SpawnMedal(podiumLocations[pair.Key].transform.position, pair.Value);
        }
        #endregion

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            if (i == 0)
            {
                sortedPlayers[i].hasWon = true;
            }
        }
        //Setting total to players score to then scale podium by their individual scores
        foreach (Player p in players)
        {

            total += p.playerScore;

        }

        for (int i = 0; i < thePlayerData.Length; i++)
        {
            thePlayerData[i].gameObject.SetActive(false);
        }

        usedPodiums = 0;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isActivated == true && players[i].isLocked == true)
            {
                thePlayerData[usedPodiums].GetComponent<PodiumController>().AddPlayer(players[i]);
                thePlayerData[usedPodiums].transform.position = podiumLocations[usedPodiums].transform.position;
                thePlayerData[usedPodiums].gameObject.SetActive(true);
                thePlayerData[usedPodiums].GetComponent<PodiumController>().SetTotal(total);
                usedPodiums++;
                //SoundManager.Instance.SetBGM("Win");
            }
        }
        StartCoroutine(WinnerCheck());
        medalManager.WriteMedalSaveFile();
    }

    private void Update()
    {
        if (canUseInput)
        {
            if (Input.GetButtonDown("Dash"))
            {
                switch (selectID)
                {
                    case 0:
                        MainMenuReturnBtn();
                        break;

                    case 1:
                        Rematch();
                        break;
                }
            }

            if (Input.GetAxis("Horizontal") > 0.3f)
            {
                if (isAxis == false)
                {
                    isAxis = true;
                    selectID++;
                    if (selectID > buttons.Length - 1)
                    {
                        selectID = 0;
                    }
                    SetHover();
                }
            }
            else if (Input.GetAxis("Horizontal") < -0.3f)
            {
                if (isAxis == false)
                {
                    isAxis = true;
                    selectID--;
                    if (selectID < 0)
                    {
                        selectID = buttons.Length - 1;
                    }
                    SetHover();
                }
            }
            else
                isAxis = false;
        }
    }

    private void SetHover()
    {
        if (previousSelection != null)
            previousSelection.ChangeState(UIElementState.inactive);

        buttons[selectID].ChangeState(UIElementState.hover);
        previousSelection = buttons[selectID];
    }

    //returns to main menu
    public void MainMenuReturnBtn()
    {
        ToggleButtons();
        sortedPlayers.Clear();
        foreach (Player p in players)
        {

            p.hasWon = false;
            p.isLocked = false;
            p.isActivated = false;
            p.skinId = 0;
            p.scorePercentage = 0;
        }
        //loading.InitializeLoading();
        SceneManager.LoadScene(10);
    }

    //restarts the game
    public void Rematch()
    {
        ToggleButtons();
        sortedPlayers.Clear();
        foreach (Player p in players)
        {

            p.hasWon = false;
            p.scorePercentage = 0;
        }
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        //loading.LoadingProgress();
    }

    private void ToggleButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            bool isActive = buttons[i].gameObject.activeSelf;
            buttons[i].gameObject.SetActive(!isActive);
        }
    }

    private void DisplayWinner()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].hasWon)
            {
                winnerIcons[i].SetActive(true);
            }
        }
        ToggleButtons();
        SetHover();
    }

    private IEnumerator WinnerCheck()
    {
        yield return new WaitForSeconds(3f);
        DisplayWinner();
        canUseInput = true;

        yield return null;
    }
}