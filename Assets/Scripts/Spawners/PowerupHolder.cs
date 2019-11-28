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
    [SerializeField]
    private GameObject godPower;
    private bool isGodPower = false;

    private void Start()
    {
        isGodPower = false;
        godPower.SetActive(false);
        bounds = availablePowerups[availablePowerups.Count - 1].MaxProbabilityRange;
        powerupSpawn = this.GetComponentInParent<PowerupSpawner>();
        ResetPowerups();
    }

    public void ActiveGodPowerup()
    {
        isSpawned = true;
        isGodPower = true;
        godPower.gameObject.SetActive(true);
    }
    public void ActiveRandomPower()
    {
        isGodPower = false;
        isSpawned = true;
        int randomPowerUp = Random.Range(0, bounds);
        //randomPowerupId = randomPowerUp;
        for (int i = 0; i < availablePowerups.Count; i++)
        {
            if (randomPowerUp >= availablePowerups[i].MinProbabilityRange && randomPowerUp <= availablePowerups[i].MaxProbabilityRange)
            {
                if (availablePowerups[i].spawnObject.tag != "GodPower")
                {
                    availablePowerups[i].spawnObject.SetActive(true);
                    randomPowerupId = i;
                }
            }
        }
    }

    //Player Collision with power up
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isSpawned == true)
            {
                if (other.gameObject.GetComponent<PlayerBase>().CurrentPowerUp == null)
                {
                    if (isGodPower == false)
                    {
                        other.GetComponent<PlayerBase>().SetPowerUp(availablePowerups[randomPowerupId].spawnObject.GetComponent<Power>().PowerHeld);
                    }
                    else if (isGodPower == true)
                    {
                        other.GetComponent<PlayerBase>().SetPowerUp(godPower.GetComponent<Power>().PowerHeld);
                    }
                    //ManageGame.instance.PowerIconUpdate(other.GetComponent<PlayerBase>().Player.playerNum, (int)availablePowerups[randomPowerupId].spawnObject.GetComponent<Power>().PowerHeld.powerUpId);
                }
                ResetPowerups();
            }
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
        godPower.SetActive(false);



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