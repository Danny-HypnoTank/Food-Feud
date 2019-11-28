/*
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 04/10/2019
 * Last Modified: 04/10/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private Respawn respawn;

    private void Start()
    {
        respawn = GameObject.Find("GameManager").GetComponent<Respawn>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="Player")
        {
            respawn.StartCoroutine(respawn.RespawnTimer(other.gameObject));
            other.gameObject.SetActive(false);
        }
    }
}
