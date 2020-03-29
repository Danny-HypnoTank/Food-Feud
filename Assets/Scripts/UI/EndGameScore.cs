/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 01/10/2019
 * Last Modified: 16/02/2020
 * Modified By: Antoni Gudejko, Dominik Waldowski, Alex Watson
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

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
    private GameObject[] menuButtons;
    [SerializeField]
    private GameObject[] winningPlayer;
    [SerializeField]
    private GameObject[] winnerIcons;

    private int usedPodiums;
    private bool canUseInput;
    private float total;
    private Loading loading;

    MedalManager medalManager;

    private void Start()
    {
        medalManager = MedalManager.Instance;

        canUseInput = false;
        StartCoroutine(WinnerCheck());

        //enables end score menu/rematch buttons
        foreach (GameObject goMB in menuButtons)
        {
            goMB.SetActive(true);
        }
        //enables player score texts
        foreach (GameObject goWP in winningPlayer)
        {
            goWP.SetActive(true);
        }


        loading = GameObject.Find("LoadingManager").GetComponent<Loading>();
        sortedPlayers = players.OrderByDescending(o => o.playerScore).ToList();
        {

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


            foreach(KeyValuePair<int,int> pair in _medalList)
            {
                if (pair.Key== medalManager.GetTopStunned() && _medalClaimed[0] == false)
                {
                    _medalClaimed[0] = true;
                    _medalForPlayer.Add(pair.Key, 0);
                }
                else if (pair.Key == medalManager.GetTopStunOther() && _medalClaimed[1] == false)
                {
                    _medalClaimed[1] = true;
                    _medalForPlayer.Add(pair.Key, 1);
                }
                else if (pair.Key == medalManager.GetTopDashes() && _medalClaimed[2] == false)
                {
                    _medalClaimed[2] = true;
                    _medalForPlayer.Add(pair.Key, 2);
                }
                else if (pair.Key == medalManager.GetTopPowerPickup() && _medalClaimed[3] == false)
                {
                    _medalClaimed[3] = true;
                    _medalForPlayer.Add(pair.Key, 3);
                }
                else if (pair.Key == medalManager.GetUntouchable() && _medalClaimed[4] == false)
                {
                    _medalClaimed[4] = true;
                    _medalForPlayer.Add(pair.Key, 4);
                }
            }

            foreach(KeyValuePair<int,int> pair in _medalForPlayer)
            {
                medalManager.SpawnMedal(podiumLocations[pair.Key].transform.position, pair.Value);
            }
            

            


            #endregion

            loading = GameObject.Find("LoadingManager").GetComponent<Loading>();
            sortedPlayers = players.OrderByDescending(o => o.scorePercentage).ToList();

            for (int i = 0; i < sortedPlayers.Count; i++)
            {
                sortedPlayers[i].scorePercentage = 0;
                if (i == 0)
                {
                    sortedPlayers[i].hasWon = true;
                }
                else
                {
                    sortedPlayers[i].hasWon = false;
                }
            }
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
        }
        medalManager.WriteMedalSaveFile();
    }

    //returns to main menu
    public void MainMenuReturnBtn()
    {
        if (canUseInput)
        {
            loading.InitializeLoading();

            //disables player score texts and menu buttons when main menu button is pressed
            foreach (GameObject goMB in menuButtons)
            {
                goMB.SetActive(false);
            }
            foreach (GameObject goWP in winningPlayer)
            {
                goWP.SetActive(false);
            }

            foreach (Player p in players)
            {

                p.hasWon = false;
                p.isLocked = false;
                p.isActivated = false;
                p.skinId = 0;

            }

            SceneManager.LoadScene(0);
        }
    }

    //restarts the game
    public void Rematch()
    {
        if (canUseInput)
        {
            loading.InitializeLoading();

            //disables player score texts and menu buttons when rematch button is pressed
            foreach (GameObject goMB in menuButtons)
            {
                goMB.SetActive(false);
            }
            foreach (GameObject goWP in winningPlayer)
            {
                goWP.SetActive(false);
            }

            foreach (Player p in sortedPlayers)
            {

                p.hasWon = false;

            }

            //reload characters

            SceneManager.LoadScene(1);
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
    }

    private IEnumerator WinnerCheck()
    {
        yield return new WaitForSeconds(3f);
        DisplayWinner();
        canUseInput = true;

        yield return null;
    }
}