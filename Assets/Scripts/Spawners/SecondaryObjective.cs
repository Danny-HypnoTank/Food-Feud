using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SecondaryObjective : MonoBehaviour
{
    private SecondaryObjectiveSpawner spawner;
    private bool isHandicap;
    [SerializeField]
    private float handicapDuration = 5, pickUpDelayS = 3;
    [SerializeField]
    private float currentTimer;
    [SerializeField]
    private Collider boxCollision;

    public Collider BoxCollision { get => boxCollision; set => boxCollision = value; }

    private void OnEnable()
    {
        isHandicap = true;
        currentTimer = 0;
        if(boxCollision == null)
        {
            this.gameObject.GetComponent<Collider>();
        }
        boxCollision.enabled = true;
        boxCollision.isTrigger = true;
    }

    public void GetSpawner(SecondaryObjectiveSpawner _spawner)
    {
        spawner = _spawner;
    }

    public void Dropped(Transform holder)
    {
        // Vector3 spawnPos = new Vector3(Random.insideUnitSphere.x * 5, 0.77f, Random.insideUnitSphere.z * 5);
        // this.transform.position = spawnPos.
        float radius = 4;
        Vector3 origin = this.transform.position;
        Debug.Log(origin);
        origin.y = 0.77f;
        origin.z += Random.Range(-radius, radius);
        origin.x += Random.Range(-radius, radius);
        Debug.Log(origin);
        this.transform.position = origin;
        Invoke("PickUpDelay", pickUpDelayS);
    }

    private void PickUpDelay()
    {
        boxCollision.enabled = true;
        boxCollision.isTrigger = true;
    }

    private void Update()
    {
        if (isHandicap == true)
        {
            currentTimer += Time.deltaTime;
            if(currentTimer >= handicapDuration)
            {
                isHandicap = false;
                currentTimer = 0;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(isHandicap == true)
        {
            //Only let player with lowest score pick it up
        }
        else if(isHandicap == false)
        {
            //let everyone pick it up
        }
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<SecondaryObjCollector>().TransferObj(this.gameObject);
        }
    }
}
