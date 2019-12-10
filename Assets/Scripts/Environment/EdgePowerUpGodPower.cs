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
        
    }
    public void EnablePowerGodPower()
    {
        isEnabled = true;
        ManageGame.instance.GodPowerUp.transform.position = transform.position;
        ManageGame.instance.GodPowerUp.SetActive(true);
    }

}
