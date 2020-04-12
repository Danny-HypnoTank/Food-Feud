using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{

    [SerializeField]
    private float timeoutTime = 5;
    [SerializeField]
    private int fadeSpeed;

    private float fadeValue;
    private bool isFading;
    private Vector3 startLoc;
    private MeshRenderer mRender;
    private Rigidbody rigidBody;

    private void Awake()
    {
        mRender = GetComponent<MeshRenderer>();
        rigidBody = GetComponent<Rigidbody>();
        startLoc = transform.position;
    }

    private void OnEnable()
    {
        fadeValue = 1;
        transform.position = startLoc;
        isFading = false;
    }

    private void Update()
    {
        if(isFading)
        {
            if (mRender.material.GetFloat("_Transparency") > 0)
            {
                fadeValue -= Time.deltaTime * fadeSpeed;
                mRender.material.SetFloat("_Transparency", fadeValue);
            }
            else
                gameObject.SetActive(false);
        }
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
        isFading = true;    
    }    



}
