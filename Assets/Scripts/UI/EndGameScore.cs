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
    private int usedPodiums;
    private float total;
    private Loading loading;
    [SerializeField]
    private GameObject[] menuButtons;
    [SerializeField]
    private GameObject[] winningPlayer;
    [SerializeField]
    private GameObject[] winnerIcons;

    private void Start()
    {
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

            for (int i = 0; i < players.Length; i++)
            {

                Debug.Log($"P{i}: {players[i].scorePercentage}");

            }

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
                    SoundManager.Instance.SetBGM("Win");
                }
            }
            //Disables all score texts


            sortedPlayers = players.OrderByDescending(o => o.playerScore).ToList();

            for (int i = 0; i < sortedPlayers.Count; i++)
            {
                if (sortedPlayers[i].isActivated == true)
                {
                    if (sortedPlayers[i].isLocked == true)
                    {
                        winningPlayer[i].gameObject.SetActive(true);
                        winningPlayer[i].GetComponent<Text>().text = $"Player {sortedPlayers[i].playerNum} Score: {(int)sortedPlayers[i].scorePercentage}%";
                    }
                }
            }
        }



        /*private void Update()
        {
            if (Input.GetButtonDown("BackButton"))
            {
                ManageGame.instance.gridManager.UnloadGridList();
                MainMenuReturnBtn();
            }
        }*/
    }

    //returns to main menu
    public void MainMenuReturnBtn()
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

        SceneManager.LoadScene(0);
    }

    //restarts the game
    public void Rematch()
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

        //reload characters

        SceneManager.LoadScene(1);
    }

    private void DisplayWinner()
    {
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            if (sortedPlayers[i].hasWon)
            {
                winnerIcons[i].SetActive(true);
            }
        }
    }

    private IEnumerator WinnerCheck()
    {
        yield return new WaitForSeconds(3f);
        DisplayWinner();

        yield return null;
    }
}