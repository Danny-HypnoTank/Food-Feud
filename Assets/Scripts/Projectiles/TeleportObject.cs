using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour
{

    private PlayerController objToTP;
    private Rigidbody rigid;

    public void SetObject(PlayerController player)
    {

        objToTP = player;

    }

    private void Awake() => rigid = GetComponent<Rigidbody>();

    private void Start()
    {
        rigid.AddForce(transform.forward * 10);
        rigid.AddForce(transform.up * 5);
    }

    private void OnTrigerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            objToTP.gameObject.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }
}