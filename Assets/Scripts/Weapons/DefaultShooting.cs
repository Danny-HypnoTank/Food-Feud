using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultShooting : Shooting
{

    //Weapon Specific Stats
    //Ammo Consumption
    private float ammoConsumption = 60;
    //Ammo Regen
    private float ammoRegeneration = 40;

    private DazeState dazeState;

    [SerializeField]
    private float weaponSplashMultiplier = 1;

    private ObjectAudioHandler audioHandler;
    private bool canPlaySplat;
    private bool reloading;

    [SerializeField]
    private List<Material> gunMaterials;

    #region raycast
    private RaycastHit hit;
    private Ray ray;
    private Ray downRay;
    //In case we'd like to make a more precise "arc"
    private int anglePrecision = 1;
    private int maxAngle;
    [SerializeField]
    private float maxRange = 10;
    private float minRange = 1;
    private float range = 1;
    private float rangeRateIncrease = 2.0f;

    private float knockBackForce = 5;
    #endregion

   

    private void Update()
    {
        DefaultWeapon();
        dazeState = GetComponentInParent<DazeState>();
    }

    private void OnEnable()
    {
        reloading = false;
        Ammo = 100;
        UpdateFillBar();
        audioHandler = GetComponent<ObjectAudioHandler>();
        canPlaySplat = true;

        Renderer _rend = gameObject.GetComponent<Renderer>();

        //TODO: fix pls, "Object reference not set to a reference of an object"
        /*switch (Player.skinId)
        {
            case (0):
                {
                    _rend.material = gunMaterials[0];
                    break;
                }
            case (1):
                {
                    _rend.material = gunMaterials[1];
                    break;
                }
            case (2):
                {
                    _rend.material = gunMaterials[2];
                    break;
                }
            case (3):
                {
                    _rend.material = gunMaterials[3];
                    break;
                }
        }*/

    }

    private void DefaultWeapon()
    {
        if (ManageGame.instance.IsTimingDown == true)
        {

            if (Input.GetButtonDown("Shoot" + Player.playerNum))
            {
                //TODO: Change color of particle based on player
                if (Ammo > 0)
                {
                    Particle.Play();
                    reloading = false;
                }
            }
            else if (Input.GetButtonUp("Shoot" + Player.playerNum))
            {
                Particle.Stop();
                audioHandler.StopSFX("Spray");
                canPlaySplat = true;
                reloading = true;
            }

            //Using right Bumper - Ammo Consumption
            if (Input.GetButton("Shoot" + Player.playerNum) && !reloading)
            {

                if (!Particle.isPlaying)
                    Particle.Play();

                if (Ammo > 0)
                {

                    if (!Particle.isPlaying)
                        Particle.Play();

                    if (dazeState.CanMove == true)
                    {
                        //if it's equal to false
                        if (IsAxisInUse == false)
                        {

                            if (canPlaySplat)
                            {
                                audioHandler.SetSFX("Spray");
                                StartCoroutine(SplatSFXCooldown());
                            }

                            range += rangeRateIncrease * Time.deltaTime;
                            //Debug.Log(range);
                               if(range > maxRange)
                               {
                                  range = maxRange;
                               }
                            Ammo -= (ammoConsumption - (AmmoConsumptionModifier)) * Time.deltaTime;

                            UpdateFillBar();
                            Vector3 _lineOffset = ray.direction * range;
                            ray = new Ray(GunMuzzle.transform.position, GunMuzzle.transform.forward);
                            Debug.DrawRay(GunMuzzle.transform.position, GunMuzzle.transform.forward * range, Color.green);

                            if (ManageGame.instance.IsTimingDown == true)
                            {

                                Vector3 _endRayCopy = ray.origin + range * ray.direction;

                                if (Physics.Raycast(ray, out hit, range))
                                {
                                    CollideWith(hit.collider.gameObject.tag, PlayerBase);
                                }
                                else
                                {
                                    for (int i = 0; i < anglePrecision; i++)
                                    {
                                        //rotation is bugged at some angles, replace "parent.transform.up" if fixed
                                        //var _rotation = Quaternion.Euler(0, 0, 30) * parent.transform.up;
                                        Ray _newRay = new Ray(_endRayCopy, (this.transform.forward - this.transform.up).normalized);

                                        Debug.DrawRay(_newRay.origin, _newRay.direction * range, Color.blue);

                                        if (Physics.Raycast(_newRay, out hit, range))
                                        {
                                            CollideWith(hit.collider.gameObject.tag, PlayerBase);
                                            i = anglePrecision;
                                            return;
                                        }

                                        _endRayCopy = _newRay.origin + _newRay.direction * 1;

                                    }
                                }
                            }

                            IsAxisInUse = true;
                        }
                    }
                }
                else
                {
                    if (Particle.isPlaying)
                    {
                        Particle.Stop();
                        audioHandler.StopSFX("Spray");
                        canPlaySplat = true;
                        reloading = true;
                    }

                    reloading = true;

                }
                if (Input.GetButton("Shoot" + Player.playerNum))
                {
                    IsAxisInUse = false;
                }

            }
            //Ammo Regeneration
            //if (!Input.GetButton("Shoot" + Player.playerNum))
            //{
            //    range = 1.0f; //Resetting Range when player isnt pressing button
            //    Ammo += (ammoRegeneration + (AmmoRegenModifier)) * Time.deltaTime;
            //    //Setting Maximum Cap on player ammo
            //    if (Ammo >= 100)
            //    {
            //        Ammo = 100;
            //    }
            //    UpdateFillBar();

            //}
            if (reloading)
            {
                range = 1.0f; //Resetting Range when player isnt pressing button
                Ammo += (ammoRegeneration + (AmmoRegenModifier)) * Time.deltaTime;
                //Setting Maximum Cap on player ammo
                if (Ammo >= 100)
                {
                    Ammo = 100;
                    reloading = false;
                }
                UpdateFillBar();

            }
            else if (Input.GetButton("Shoot" + Player.playerNum) && reloading)
            {

                range = 1.0f; //Resetting Range when player isnt pressing button
                Ammo += (ammoRegeneration + (AmmoRegenModifier)) * Time.deltaTime;
                //Setting Maximum Cap on player ammo
                if (Ammo >= 100)
                {
                    Ammo = 100;
                    reloading = false;
                }
                UpdateFillBar();

            }
        }
    }

    private void CollideWith(string tag, PlayerBase playerBase)
    {

        switch (tag)
        {
            case "Player":
                GameObject player = hit.transform.gameObject;
                DazeState pDaze = player.GetComponent<DazeState>();

                break;
            case "PaintableEnvironment":

                PaintSizeMultiplier mult = hit.collider.GetComponent<PaintSizeMultiplier>();

                //Renderer _wallRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                float _smult;
               
                if (mult)
                {
                    _smult = (1f *  mult.multiplier) * weaponSplashMultiplier;
                }
                else
                {
                    _smult = 1f * weaponSplashMultiplier;
                }
                int _id = Player.playerNum;
                DrawColor.DrawOnSplatmap(hit, _id, Player, _smult);
                
                break;
            default:
                downRay = new Ray(ray.GetPoint(range), -playerBase.transform.up);
                Debug.DrawRay(ray.GetPoint(range), -playerBase.transform.up, Color.red);
                if (Physics.Raycast(downRay, out hit, range))
                {

                    //TODO: implement painting of floor
                    if (hit.collider.gameObject.tag == "Floor")
                        Debug.Log("hit floor");
                }
                break;
        }
    }

    private IEnumerator SplatSFXCooldown()
    {

        canPlaySplat = false;
        yield return new WaitForSeconds(3f);
        canPlaySplat = true;

    }

}

