using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLauncher : Shooting
{
    //Weapon Specific Stats
    //Ammo Consumption
    private float bombAmmoConsumption = 25;
    private bool hasShot;
    private ObjectPooling bombPool;

    private void Update()
    {
            BombLauncherShooting();
    }

    private void OnEnable()
    {
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
                        GameObject bomb = bombPool.GetPooledObject();
                        bomb.transform.rotation = GunMuzzle.transform.rotation;
                        bomb.transform.position = GunMuzzle.transform.position;
                        bomb.GetComponent<Bomb>().SetParent(PlayerBase);
                        bomb.SetActive(true);
                        StartCoroutine(BombLaunchedDelay());
                    }
                }
            }

            //Reseting to Default Weapon when no ammo left
            if (Ammo <= 0)
            {
                PlayerBase.ResetWeapon();
                PlayerBase.DefaultWeaponSet();
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
