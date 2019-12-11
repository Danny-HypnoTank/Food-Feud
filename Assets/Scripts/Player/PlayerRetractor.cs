using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRetractor : MonoBehaviour
{
    [SerializeField]
    private Transform playerHolder;

    private void Update()
    {
        playerHolder.transform.position = this.transform.position;
    }
}
