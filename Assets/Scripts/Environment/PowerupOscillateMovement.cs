using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupOscillateMovement : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 pos1;
    private Vector3 pos2;
    private float oscillatingSpeed = 1.0f;
    private float maxHeight = 1;

    private void Start()
    {
        startPos = transform.position;
        pos1 = startPos;
        pos2 = new Vector3(startPos.x, startPos.y + maxHeight, startPos.z);
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(oscillatingSpeed * Time.time) + 1.0f) / 2.0f);
    }
}
