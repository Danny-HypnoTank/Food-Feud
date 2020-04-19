using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinReset : MonoBehaviour
{
    [SerializeField]
    private Transform[] pinLoc;
    [SerializeField]
    private Transform[] pin;
    [SerializeField]
    private CharacterPin[] pinImg;
    [SerializeField]
    private float resetDelayTime = 0.1f;


    public void CallReset()
    {
        StopCoroutine("ResetDelay");
        StartCoroutine("ResetDelay");
    }
    private void ResetMode()
    {
        for (int i = 0; i < pin.Length; i++)
        {
            pin[i].transform.position = pinLoc[i].transform.position;
        }
        for (int i = 0; i < pinImg.Length; i++)
        {
            pinImg[i].UnOwnPin();
        }
    }

    private IEnumerator ResetDelay()
    {
        yield return new WaitForSeconds(resetDelayTime);
        ResetMode();
        yield return null;
    }

}
