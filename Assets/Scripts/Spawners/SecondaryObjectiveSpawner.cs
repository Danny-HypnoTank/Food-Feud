using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryObjectiveSpawner : MonoBehaviour
{
    [SerializeField]
    private float timeToSpawn = 10;
    private float timeCount = 0;
    private bool hasSpawned = false;
    [SerializeField]
    private Transform spawnLoc;
    [SerializeField]
    private GameObject spawnObj;

    public Transform SpawnLoc { get => spawnLoc; set => spawnLoc = value; }

    private void Start()
    {
        spawnObj.gameObject.SetActive(true);
        spawnObj.GetComponent<SecondaryObjective>().GetSpawner(this);
        spawnObj.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(hasSpawned == false)
        {
            timeCount += Time.deltaTime;
            if(timeCount >= timeToSpawn)
            {
                spawnObj.transform.position = spawnLoc.transform.position;
                spawnObj.SetActive(true);
                hasSpawned = true;
                timeCount = 0;
            }
        }
    }

    public void ResetSecondaryObj()
    {
        hasSpawned = false;
        timeCount = 0;
    }
}
