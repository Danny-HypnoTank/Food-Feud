/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 01/10/2019
 * Last Modified: 22/10/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    private List<Transform> powerupSpawnLocations = new List<Transform>();              //stores all locations for powerups                                                                                    // private int maxSpawnRate = 4; //most amount of delay between each powerup spawn
    private int powerUpSpawnDelay = 3;
    private int initialWaitTime = 5;
    [SerializeField]
    private int maxPowerupsOnMap;                                                       //maximum number of powerups on map at any given time
    [SerializeField]
    private int currentActivePowerUps = 0;
    [SerializeField]
    private int spawnedPowerUps = 0;
    private int godPowerTime = 3;
    private bool spawnedGodPower = false;
    private bool respawnStarted;
    private bool nukeHasBeenSpawned;
    private GameObject[] godNodes;

    //grabs every child object and adds it to powerup location list as well as starts coroutine to spawn them in
    private void Start()
    {
        spawnedGodPower = false;
        nukeHasBeenSpawned = false;
        respawnStarted = false;
        spawnedPowerUps = 0;
        currentActivePowerUps = 0;
        godNodes = GameObject.FindGameObjectsWithTag("NukeSpawn");
        foreach (Transform child in this.transform)
        {
            powerupSpawnLocations.Add(child);
        }
        // StartCoroutine(RandomPowerupSpawner());
        InvokeRepeating("SpawnRandomPowerUp", (powerUpSpawnDelay + 4), powerUpSpawnDelay);
    }

    public void PowerupCollected()
    {
        if (currentActivePowerUps > 0)
        {
            currentActivePowerUps--;
        }
    }

    private void SpawnRandomPowerUp()
    {
        if (currentActivePowerUps < maxPowerupsOnMap)
        {
            if (spawnedPowerUps >= godPowerTime && !spawnedGodPower)
            {
                GodPowerUp();
            }
            else
            {
                RandomPowerUp();
            }
        }
    }

    private void GodPowerUp()
    {
        if (!nukeHasBeenSpawned)
            nukeHasBeenSpawned = true;
        spawnedGodPower = true;
        ManageGame.instance.GodPowerUp.SetActive(true);
    }

    private void RandomPowerUp()
    {
        if (nukeHasBeenSpawned)
        {
            if (!ManageGame.instance.GodPowerUp.activeSelf)
            {
                if (!respawnStarted)
                    StartCoroutine(RespawnCooldown());
            }
        }

        int randomLocation;
        randomLocation = Random.Range(0, powerupSpawnLocations.Count);

        PowerupNode chosenHolder = powerupSpawnLocations[randomLocation].GetComponent<PowerupNode>();
        if (!chosenHolder.IsSpawned)
            chosenHolder.SpawnPower();

        spawnedPowerUps++;
    }

    private IEnumerator RespawnCooldown()
    {
        respawnStarted = true;
        yield return new WaitForSeconds(10);
        spawnedGodPower = false;
        respawnStarted = false;
    }
}