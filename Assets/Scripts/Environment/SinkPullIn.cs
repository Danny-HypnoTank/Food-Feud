using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkPullIn : MonoBehaviour
{
    [SerializeField]
    GameObject[] playerObjects;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerObjects.Length; i++)
        {
            PlayerController playerToCheck = playerObjects[i].GetComponent<PlayerController>();
            Vector3 forceDirection = this.transform.position - playerToCheck.transform.position;
            playerToCheck.GetComponent<CharacterController>().Move(forceDirection * 0.002f);
                
            
        }
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(2);
        playerObjects = GameObject.FindGameObjectsWithTag("Player");
    }
}
