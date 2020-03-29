using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{

    [SerializeField]
    private float timeoutTime = 5;

    private MeshRenderer mRender;
    private Rigidbody rigidBody;

    private void Awake()
    {
        mRender = GetComponent<MeshRenderer>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log(rigidBody.velocity.y);
    }

    private void OnTriggerEnter(Collider other)
    {

        StartCoroutine(Timeout());

        if (other.CompareTag("Player"))
        {
            if (rigidBody.velocity.y < 0)
            {
                PlayerController playerHit = other.GetComponent<PlayerController>();
                StartCoroutine(playerHit.PlayerStun.Stun(0.5f));
            }
        }
    }

    private IEnumerator Timeout()
    {
        yield return new WaitForSeconds(timeoutTime);

        /*Color temp = mRender.material.color;
        temp.a = Mathf.MoveTowards(1, 0, Time.deltaTime);
        mRender.material.color = temp;

        if (mRender.material.color.a == 1)*/
        gameObject.SetActive(false);
    }    
}
