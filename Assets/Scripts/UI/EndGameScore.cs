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
            int _currentPodiumStun = 0;
            int _currentPodiumStunOthers = 0;
            int _currentPodiumDash = 0;
            int _currentPodiumPickUp = 0;

            foreach (Player p in sortedPlayers)
            {
                if (p.playerNum == medalManager.GetTopStunned())
                {
                    _currentPodiumStun = p.playerNum;
                }

                if (p.playerNum == medalManager.GetTopStunOther())
                {
                    _currentPodiumStunOthers = p.playerNum;
                }

                if (p.playerNum == medalManager.GetTopDashes())
                {
                    _currentPodiumDash = p.playerNum;
                }

                if (p.playerNum == medalManager.GetTopPowerPickup())
                {
                    _currentPodiumPickUp = p.playerNum;
                }
            }

            medalManager.SpawnMedal(podiumLocations[_currentPodiumStun].transform.position, podiumLocations[_currentPodiumStunOthers].transform.position, 
                podiumLocations[_currentPodiumDash].transform.position, podiumLocations[_currentPodiumPickUp].transform.position);


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