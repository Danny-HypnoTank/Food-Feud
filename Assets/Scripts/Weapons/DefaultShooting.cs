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
        Ammo = 100;
        UpdateFillBar();
    }

    private void DefaultWeapon()
    {
        if (ManageGame.instance.IsTimingDown == true)
        {

            if (Input.GetButtonDown("Shoot" + Player.playerNum))
            {
                //TODO: Change color of particle based on player
                Particle.Play();
            }
            else if (Input.GetButtonUp("Shoot" + Player.playerNum))
                Particle.Stop();

            //Using right Bumper - Ammo Consumption
            if (Input.GetButton("Shoot" + Player.playerNum))
            {
                if (Ammo > 0)
                {
                    if (dazeState.CanShoot == true)
                    {
                        //if it's equal to false
                        if (IsAxisInUse == false)
                        {
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
                        Particle.Stop();

                }
                if (Input.GetButton("Shoot" + Player.playerNum))
                {
                    IsAxisInUse = false;
                }

            }
            //Ammo Regeneration
            if (!Input.GetButton("Shoot" + Player.playerNum))
            {
                range = 1.0f; //Resetting Range when player isnt pressing button
                Ammo += (ammoRegeneration + (AmmoRegenModifier)) * Time.deltaTime;
                //Setting Maximum Cap on player ammo
                if (Ammo >= 100)
                {
                    Ammo = 100;
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
                StunBehavior pStun = player.GetComponent<StunBehavior>();
                DazeState pDaze = player.GetComponent<DazeState>();

                //hit.collider.gameObject.transform.Translate(ray.direction * 0.15f);
                if (playerBase.GodModeKnockback == false)
                {
                    hit.transform.gameObject.GetComponent<DazeState>().KnockBackEffect(ray.direction, knockBackForce);
                }
                else
                {
                    hit.transform.gameObject.GetComponent<DazeState>().KnockBackEffect(ray.direction, knockBackForce);
                }

                pStun.AddStun(0.1f, pDaze, player.GetComponent<PlayerBase>().Player);

                break;
            case "PaintableEnvironment":
                //Renderer _wallRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                float _smult;
                float _tmult;
                if (hit.collider.GetComponent<PaintSizeMultiplier>())
                {
                    _smult = hit.collider.GetComponent<PaintSizeMultiplier>().multiplier;
                    _tmult = hit.collider.GetComponent<PaintSizeMultiplier>().multiplier;
                }
                else
                {
                    _smult = 4.5f;
                    _tmult = 1;
                }
                int _id = Player.skinId;
                switch (_id)
                {
                    case (0):
                        {
                            DrawColor.DrawOnSplatmap(hit, new Color(1, 0, 0, 0), 200, _smult, _tmult);
                            break;
                        }
                    case (1):
                        {
                            DrawColor.DrawOnSplatmap(hit, new Color(0, 1, 0, 0), 200, _smult, _tmult);
                            break;
                        }
                    case (2):
                        {
                            DrawColor.DrawOnSplatmap(hit, new Color(0, 0, 1, 0), 200, _smult, _tmult);
                            break;
                        }
                    case (3):
                        {
                            DrawColor.DrawOnSplatmap(hit, new Color(0, 0, 0, 1), 200, _smult, _tmult);
                            break;
                        }
                }
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
}

