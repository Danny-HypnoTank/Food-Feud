using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    /*public Transform teleportTarget;
    public GameObject thePlayer;

    private void OnTriggerEnter(Collider other)
    {
        thePlayer.transform.position = teleportTarget.transform.position;
    }
}*/

    public GameObject objToTP;
    public Transform tpLoc;



    void OnTrigerStay(Collider other)
    {

        if ((other.gameObject.tag == "Player") && Input.GetButton("Special2"))
        {
            objToTP.transform.position = tpLoc.transform.position;
        }
    }
}