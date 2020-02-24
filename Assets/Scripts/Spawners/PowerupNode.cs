using System.Collections.Generic;
using UnityEngine;

public class PowerupNode : MonoBehaviour
{

    public bool IsSpawned { get; private set; } //Property to store if the node has a powerup spawned
    private int children; //Field to store the number of child objects
    private List<GameObject> childList = new List<GameObject>(); //List to store child objects
    private PowerupSpawner parent; //The parent spawner

    public void Awake()
    {
        //Get the number of child objects
        children = transform.childCount;
        //Cache a reference to the parent
        parent = transform.parent.GetComponent<PowerupSpawner>();

        //Add all children to childList
        for (int i = 0; i < children; i++)
        {
            childList.Add(transform.GetChild(i).gameObject);
        }
    }

    public void SpawnPower()
    {
        //Get a random child object
        int power = Random.Range(0, children);
        //Set the selected child to active
        childList[power].SetActive(true);

        //Set IsSpawned to true
        IsSpawned = true;
    }

    public void Collected()
    {
        //Set IsSpawned to false and call the PowerupCollected method on the parent
        IsSpawned = false;
        parent.PowerupCollected();
    }

}
