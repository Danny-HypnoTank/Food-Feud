using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float power = 0.7f;
    public float duration = 1.0f;
    public float slowDownAmount = 1.0f;
    private bool canShake;

    Vector3 startPosition;
    float initialDuration;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.gameObject.transform.localPosition;
        initialDuration = duration;
        canShake = false;
    }

    private void Update()
    {

        if (canShake)
        {
            if (duration > 0)
            {
                this.gameObject.transform.localPosition = startPosition + Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }
            else
            {
                duration = initialDuration;
                this.gameObject.transform.localPosition = startPosition;
                canShake = false;
            }

        }

    }

    public void ShakeCamera()
    {

        canShake = true;

    }
}
        
