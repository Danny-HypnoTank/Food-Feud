using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject owner;
    private SecondaryObjectiveSpawner spawner;

    public GameObject Owner { get => owner; set => owner = value; }

    private void Start()
    {
        spawner = GameObject.Find("SecondaryObjSpawner").GetComponent<SecondaryObjectiveSpawner>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == owner)
        {
            if(other.GetComponent<SecondaryObjCollector>().HasSecondaryObj == true &&
                other.GetComponent<SecondaryObjCollector>().SecondaryObj != null)
            {
                other.gameObject.GetComponent<SecondaryObjCollector>().Delivery();
            }
        }
    }
}
