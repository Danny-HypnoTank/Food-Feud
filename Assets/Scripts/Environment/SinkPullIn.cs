using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkPullIn : MonoBehaviour
{

    [SerializeField]
    private float duration;
    [SerializeField]
    private GameObject waterPlane;
    [SerializeField]
    private float baseWaterSpeed;
    private float waterSpeed;
    [SerializeField]
    private float pullInSpeed;

    private bool isPulling = false;

    private void OnEnable()
    {
        StartCoroutine(LateStart());
        waterSpeed = baseWaterSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPulling)
        {
            for (int i = 0; i < ManageGame.instance.allPlayerControllers.Count; i++)
            {
                Vector3 forceDirection = this.transform.position - ManageGame.instance.allPlayerControllers[i].transform.position;
                ManageGame.instance.allPlayerControllers[i].chc.Move(forceDirection * pullInSpeed);
            }
        }
        waterPlane.transform.Translate(Vector3.up * waterSpeed * Time.deltaTime);
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(duration);
        isPulling = true;
        StartCoroutine(ResetTimer());
    }

    IEnumerator ResetTimer()
    {
        waterSpeed = -baseWaterSpeed;
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
        isPulling = false;
    }
}
