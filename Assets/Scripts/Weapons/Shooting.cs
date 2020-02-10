/*
 * Created by:
 * Name: Akexander Watson
 * Sid: 1507490
 * Date Created: 29/09/2019
 * Last Modified: 15/10/2019
 * Modified By: Antoni Gudejko
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    //Parent Class for all variables
    //References
    private Player player;

    //Properties
    public Player Player { get => player; set => player = value; }
    public Transform GunMuzzle { get => gunMuzzle; set => gunMuzzle = value; }
    public bool CanShoot { get => canShoot; set => canShoot = value; }
    public float Ammo { get => ammo; set => ammo = value; }
    public Image FillBar { get => fillBar; set => fillBar = value; }
    public PlayerBase PlayerBase { get => playerBase; set => playerBase = value; }
    public bool IsAxisInUse { get => isAxisInUse; set => isAxisInUse = value; }
    public float AmmoRegenModifier { get => ammoRegenModifier; set => ammoRegenModifier = value; }
    public float AmmoConsumptionModifier { get => ammoConsumptionModifier; set => ammoConsumptionModifier = value; }
    public DrawColor DrawColor { get => drawColor; set => drawColor = value; }

    public ParticleSystem Particle { get { return particle; } private set { particle = value; } }

    public Transform SecondGunMuzzle { get => secondGunMuzzle; set => secondGunMuzzle = value; }

    [SerializeField]
    private Transform gunMuzzle, secondGunMuzzle;
    [SerializeField]
    private Image fillBar;
    [SerializeField]
    private DrawColor drawColor;
    [SerializeField]
    private ParticleSystem particle;

    private PlayerBase playerBaseCharacteristics;
    private PlayerBase playerBase;

    private bool canShoot;
    private bool isAxisInUse = false;

    //Weapon Stats
    [SerializeField]
    private float ammo;
    //Power Up Modifiers
    private float ammoRegenModifier = 5;
    private float ammoConsumptionModifier = 10;

    private void Start()
    {

        // lineRenderer = GetComponentInChildren<LineRenderer>();
        playerBaseCharacteristics = GetComponent<PlayerBase>();
        drawColor = GameObject.Find("GameManager").GetComponent<DrawColor>();
        canShoot = true;
        if (particle != null)
        {
            ParticleSystem.MainModule main = particle.main;

            switch (PlayerBase.Player.skinId)
            {
                case 0:
                    main.startColor = Color.red;
                    fillBar.color = Color.red;
                    break;
                case 1:
                    main.startColor = Color.green;
                    fillBar.color = Color.green;
                    break;
                case 2:
                    main.startColor = Color.blue;
                    fillBar.color = Color.blue;
                    break;
                case 3:
                    main.startColor = Color.yellow;
                    fillBar.color = Color.yellow;
                    break;
            }
        }
    }


    public void UpdateFillBar()
    {
        if (FillBar != null)
        {
            FillBar.fillAmount = Ammo / 100;
        }
    }
}