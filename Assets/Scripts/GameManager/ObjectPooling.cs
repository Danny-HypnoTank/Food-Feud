/*
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 01/10/2019
 * Last Modified: 01/10/2019
 */
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    [SerializeField]
    private GameObject objectToPool;
    [SerializeField]
    private int amountPooled = 10;
    private bool isGrowing = true;

    private List<GameObject> pooledObjects = new List<GameObject>();

    //Creates the set amount of objects to pool for later
    private void Start()
    {
        for (int i = 0; i < amountPooled; i++)
        {
            GameObject pooledObj = Instantiate(objectToPool) as GameObject;
            pooledObj.SetActive(false);
            pooledObjects.Add(pooledObj);
        }
    }

    //pools object(enables it) and creates more if necessary
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        if (isGrowing == true)
        {
            GameObject pooledObj = Instantiate(objectToPool) as GameObject;
            pooledObjects.Add(pooledObj);
            return pooledObj;
        }
        return null;
    }
}
