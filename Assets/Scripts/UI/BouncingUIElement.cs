using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingUIElement : MonoBehaviour
{

    [Header("Properties")]
    [SerializeField]
    private float lerpDuration;
    [SerializeField]
    private float lerpDistance;

    private float timer;
    private int direction;
    private Vector3 originalLoc;
    private Vector3 upLoc;
    private Vector3 downLoc;

    void Start()
    {
        //Initialisation
        originalLoc = transform.position;
        upLoc = new Vector3(originalLoc.x, originalLoc.y - lerpDistance, originalLoc.z) ;
        downLoc = new Vector3(originalLoc.x, originalLoc.y + lerpDistance, originalLoc.z);
        timer = 0;
    }

    void Update()
    {
        Bounce();
    }

    private void Bounce()
    {
        //Increment timer by deltaTime
        timer += Time.deltaTime;

        //If timer is greater than, or equal to, the duration of the lerp recalculate direction and reset the timer
        if (timer > lerpDuration)
        {
            direction = (direction + 1) % 2; //Returns 0 on an even number or 1 on an odd number, e.g. (0 + 1) % 2 = 1 or (1 + 1) % 2 = 0
            timer = 0;
        }

        //If state = 0 lerp upwards, else if state = 1 lerp downwards
        if (direction == 0)
        {
            transform.position = Vector3.Lerp(downLoc, upLoc, timer / lerpDuration);
        }
        else if (direction == 1)
        {
            transform.position = Vector3.Lerp(upLoc, downLoc, timer / lerpDuration);
        }
    }

}