/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 01/10/2019
 * Last Modified: 10/12/2019
 * Modified By: Antoni Gudejko, Dominik Waldowski
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
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
    private void Start()
    {
        loading = GameObject.Find("LoadingManager").GetComponent<Loading>();
        sortedPlayers = players.OrderByDescending(o => o.playerScore).ToList();
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            sortedPlayers[i].scorePercentage = 0;
            if(i == 0)
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
            if(players[i].isActivated == true && players[i].isLocked == true)
            {
                thePlayerData[usedPodiums].GetComponent<PodiumController>().AddPlayer(players[i]);
                thePlayerData[usedPodiums].transform.position = podiumLocations[usedPodiums].transform.position;
                thePlayerData[usedPodiums].gameObject.SetActive(true);
                thePlayerData[usedPodiums].GetComponent<PodiumController>().SetTotal(total);
                usedPodiums++;
                SoundManager.Instance.SetBGMTempo(1);
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
                  //  playerScores[i].gameObject.SetActive(true);
                   // playerScores[i].GetComponent<Text>().text = $"Player {sortedPlayers[i].playerNum} score: {(int)sortedPlayers[i].scorePercentage}%";
                }
            }
        }
    }



    private void Update()
    {
        if (Input.GetButtonDown("BackButton"))
        {
            MainMenuReturnBtn();
        }
    }

    //returns to main menu button
    public void MainMenuReturnBtn()
    {
        loading.InitializeLoading();
        //SceneManager.LoadScene(0);
    }
}