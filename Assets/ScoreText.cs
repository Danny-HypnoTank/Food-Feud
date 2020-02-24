using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    public float DestroyTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this, DestroyTime);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
