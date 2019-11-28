/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 08/10/2019
 * Last Modified: 08/10/2019
 */
using System.Collections.Generic;
using UnityEngine;

public class PaintedAreaCalculator : MonoBehaviour
{
    [SerializeField]
    private List<Transform> walls = new List<Transform>();
    private int[] playerColouredWalls;
    [SerializeField]
    private Transform ground;
    [SerializeField]
    private Transform wallsHolder;

    //sets up array for 4 players and adds every wall into the game
    private void Start()
    {
        playerColouredWalls = new int[4];
        for (int i = 0; i < playerColouredWalls.Length; i++)
        {
            playerColouredWalls[i] = 0;
        }
        foreach (Transform child in wallsHolder)
        {
            walls.Add(child);
        }
        foreach (Transform child in ground)
        {
            walls.Add(child);
        }
    }

    //calculates score based on amount of times coloured in players skin colour
    private void CalculateScore()
    {
        for (int i = 0; i < walls.Count; i++)
        {
            Renderer wallRenderer = walls[i].GetComponent<Renderer>();
            if (wallRenderer.sharedMaterial != null)
            {
               /* if (wallRenderer.sharedMaterial == ManageGame.instance.Players[0].skins[ManageGame.instance.Players[0].skinId].GetComponent<Renderer>().sharedMaterial)
                {
                    playerColouredWalls[0] += 1;
                }
                else if (wallRenderer.sharedMaterial == ManageGame.instance.Players[1].skins[ManageGame.instance.Players[1].skinId].GetComponent<Renderer>().sharedMaterial)
                {
                    playerColouredWalls[1] += 1;
                }
                else if (wallRenderer.sharedMaterial == ManageGame.instance.Players[2].skins[ManageGame.instance.Players[2].skinId].GetComponent<Renderer>().sharedMaterial)
                {
                    playerColouredWalls[2] += 1;
                }
                else if (wallRenderer.sharedMaterial == ManageGame.instance.Players[3].skins[ManageGame.instance.Players[3].skinId].GetComponent<Renderer>().sharedMaterial)
                {
                    playerColouredWalls[3] += 1;
                }*/
            }
        }
        for (int i = 0; i < playerColouredWalls.Length; i++)
        {
            ManageGame.instance.Players[i].playerScore = playerColouredWalls[i];
        }
    }

    //subscribes to the event
    private void OnEnable()
    {
        ManageGame.OnGameWin += CalculateScore;
    }

    //unsubscribes to the event
    private void OnDisable()
    {
        ManageGame.OnGameWin -= CalculateScore;
    }
}
