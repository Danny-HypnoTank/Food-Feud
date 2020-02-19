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

public class PowerupHolder : MonoBehaviour
{
    [SerializeField]
    private List<SpawnChance> availablePowerups = new List<SpawnChance>();

    private PowerupSpawner powerupSpawn;
    private bool isSpawned;
    private int randomPowerupId;
    public bool IsSpawned { get => isSpawned; set => isSpawned = value; }
    private int bounds = 0;

    private void Start()
    {
        bounds = availablePowerups[availablePowerups.Count -1].MaxProbabilityRange;
        powerupSpawn = this.GetComponentInParent<PowerupSpawner>();
        ResetPowerups();
    }
    public void ActiveRandomPower()
    {
        isSpawned = true;
        int randomPowerUp = Random.Range(0, bounds);
        //randomPowerupId = randomPowerUp;
        for (int i = 0; i < availablePowerups.Count; i++)
        {
            if (randomPowerUp >= availablePowerups[i].MinProbabilityRange && randomPowerUp <= availablePowerups[i].MaxProbabilityRange)
            {
                    availablePowerups[i].spawnObject.SetActive(true);
                    randomPowerupId = i;
            }
        }
    }

    //Player Collision with power up
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
        }
    }

    private void ResetPowerups()
    {
        isSpawned = false;
        powerupSpawn.PowerupCollected();

        for (int i = 0; i < availablePowerups.Count; i++)
        {
            availablePowerups[i].spawnObject.SetActive(false);
        }



        /*foreach (Transform child in this.transform)
        {

            //availablePowerups.Add(child.gameObject.gameObject);
            child.gameObject.SetActive(false);
        }*/
    }
}

[System.Serializable]
public class SpawnChance
{
    public GameObject spawnObject;
    [SerializeField]
    private int minProbabilityRange = 0;
    [SerializeField]
    private int maxProbabilityRange = 0;

    public int MinProbabilityRange { get => minProbabilityRange; }
    public int MaxProbabilityRange { get => maxProbabilityRange; }
}