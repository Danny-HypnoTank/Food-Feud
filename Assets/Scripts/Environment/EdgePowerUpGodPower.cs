using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgePowerUpGodPower : MonoBehaviour
{
    private bool isEnabled;

    [SerializeField]
    private GameObject godPowerUp;

    private void Start()
    {
        isEnabled = false;
        godPowerUp.SetActive(false);
    }
    public void EnablePowerGodPower()
    {
        isEnabled = true;
        godPowerUp.transform.position = transform.position;
        godPowerUp.SetActive(true);
    }

}
