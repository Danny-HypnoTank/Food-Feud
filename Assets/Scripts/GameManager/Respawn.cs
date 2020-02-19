/*
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 04/10/2019
 * Last Modified: 07/10/2019
 * Modified By: Antoni Gudejko
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private int respawnTimer = 5;

    public IEnumerator RespawnTimer(GameObject playerToRespawn)
    {
        for (int i = 0; i <= respawnTimer; i++)
        {
            yield return new WaitForSeconds(1);
           // ManageGame.instance.UpdateRespawnTimer(playerToRespawn.GetComponent<PlayerController>().Player.playerNum, respawnTimer - i);
        }

        int playerNum = playerToRespawn.GetComponent<PlayerController>().Player.playerNum - 1;
        playerToRespawn.transform.position = ManageGame.instance.PlayerSpawnPositions[playerNum].transform.position;
        PlayerBase player = playerToRespawn.GetComponent<PlayerBase>();

        playerToRespawn.SetActive(true);
    }
}
