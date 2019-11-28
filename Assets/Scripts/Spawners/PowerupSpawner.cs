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
    private GameObject[] godNodes;

    //grabs every child object and adds it to powerup location list as well as starts coroutine to spawn them in
    private void Start()
    {
        spawnedGodPower = false;
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

            if (spawnedGodPower == false)
            {
                if (spawnedPowerUps >= godPowerTime)
                {
                    GodPowerUp();
                }
                else
                {
                    RandomPowerUp();
                }
            }
            else if (spawnedGodPower == true)
            {
                RandomPowerUp();
            }
        }
    }

    private void GodPowerUp()
    {
        int randomLocation = Random.Range(0, godNodes.Length);
        spawnedGodPower = true;
        godNodes[randomLocation].GetComponent<EdgePowerUpGodPower>().EnablePowerGodPower();
    }

    private void RandomPowerUp()
    {

        if (!GameObject.FindGameObjectWithTag("Nuke") && spawnedGodPower)
            spawnedGodPower = false;

        int randomLocation;

        randomLocation = Random.Range(0, powerupSpawnLocations.Count);
        if (powerupSpawnLocations[randomLocation].GetComponent<PowerupHolder>().IsSpawned == false)
        {
            powerupSpawnLocations[randomLocation].GetComponent<PowerupHolder>().ActiveRandomPower();
            currentActivePowerUps++;
            spawnedPowerUps++;
        }
    }

    private IEnumerator RespawnCooldown()
    {

        yield return new WaitForSeconds(10);
        spawnedGodPower = false;

    }
    //Spawning of a powerup
}