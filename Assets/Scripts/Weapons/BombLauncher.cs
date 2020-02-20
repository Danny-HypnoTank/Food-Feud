using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLauncher : Shooting
{
    //Weapon Specific Stats
    //Ammo Consumption
    private float bombAmmoConsumption = 20;
    private bool hasShot;
    private ObjectPooling bombPool;
    private ObjectAudioHandler audioHandler;

    private void Update()
    {
            BombLauncherShooting();
    }

    private void OnEnable()
    {
        audioHandler = GetComponent<ObjectAudioHandler>();
        Ammo = 100;
        UpdateFillBar();
        hasShot = false;
    }
    private void Start()
    {
        bombPool = GameObject.Find("GameManager").GetComponent<ObjectPooling>();
    }

    private void BombLauncherShooting()
    {
        if (ManageGame.instance.IsTimingDown == true)
        {
            //Bomb Launcher Shooting - Ammo Consumption
            if (Input.GetButton("Shoot" + Player.playerNum))
            {
                if (Ammo > 0)
                {
                    if (/*CanShoot == true &&*/ hasShot == false)
                    {

                        audioHandler.SetSFX("Pop");

                        GameObject bomb = bombPool.GetPooledObject();
                        bomb.transform.rotation = GunMuzzle.transform.rotation;
                        bomb.transform.position = GunMuzzle.transform.position;
                        bomb.GetComponent<Bomb>().SetParent(PlayerBase);
                        bomb.SetActive(true);
                        StartCoroutine(BombLaunchedDelay());
                    }
                }
            }
        }
    }
    private IEnumerator BombLaunchedDelay()
    {
        Ammo -= bombAmmoConsumption;
        UpdateFillBar();
        hasShot = true;
        CanShoot = false;
        yield return new WaitForSeconds(0.5f);
        hasShot = false;
        CanShoot = true;
    }
}
