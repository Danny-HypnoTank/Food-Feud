/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 01/10/2019
 * Last Modified: 08/10/2019
 * Modified By: Antoni Gudejko
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
public class EndGameScore : MonoBehaviour
{
    [SerializeField]
    private Player[] players;
    [SerializeField]
    private Transform[] playerScores;

    [SerializeField]
    private List<Player> sortedPlayers = new List<Player>();    //stores a sorted list of players by score

    private void Start()
    {
        //Disables all score texts
        for (int i = 0; i < playerScores.Length; i++)
        {
            playerScores[i].gameObject.SetActive(false);
        }

        sortedPlayers = players.OrderByDescending(o => o.playerScore).ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            if (sortedPlayers[i].isActivated == true)
            {
                if (sortedPlayers[i].isLocked == true)
                {
                    playerScores[i].gameObject.SetActive(true);
                    playerScores[i].GetComponent<Text>().text = "Player " + sortedPlayers[i].playerNum + ": score: " + sortedPlayers[i].playerScore;
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
        SceneManager.LoadScene("MainMenu");
    }

    //adds ability to replay the game with same map and characters
    public void Rematch()
    {
        int iteration;
        iteration = PlayerPrefs.GetInt("MapID");
        SceneManager.LoadScene("GameScene" + iteration);
    }

}