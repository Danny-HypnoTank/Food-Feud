using System.Collections;
using UnityEngine;

public class TwinSpray : Shooting
{
    //Weapon Specific Stats
    //Weapon Specific Stats
    //Ammo Consumption
    private float ammoConsumption = 30;
    //Ammo Regen
    private float ammoRegeneration = 20;
    //Ammo Consumption
    // private float twinsprayAmmoConsumption = 60;
    private DazeState dazeState;
    [SerializeField]
    private ParticleSystem rightParticle;

    private float weaponSplashMultiplier = 0.5f;
    private ObjectAudioHandler audioHandler;
    private bool canPlaySplat;
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
    private float range = 3;
    private float rangeRateIncrease = 2.0f;

    private float knockBackForce = 5;

    [SerializeField]
    private LineRenderer lineRenderer;
    #endregion

    private void OnEnable()
    {
        Ammo = 100;
        UpdateFillBar();
        DrawColor = GameObject.Find("GameManager").GetComponent<DrawColor>();
        audioHandler = GetComponent<ObjectAudioHandler>();
        canPlaySplat = true;
    }

    private void Start()
    {
        dazeState = GetComponentInParent<DazeState>();
    }

    private void Update()
    {
        if (ManageGame.instance.IsTimingDown == true)
        {
            if (Input.GetButtonDown("Shoot" + Player.playerNum))
            {
                //TODO: Change color of particle based on player
                if (Ammo > 0)
                {
                    Particle.Play();
                    rightParticle.Play();
                }
            }
            else if (Input.GetButtonUp("Shoot" + Player.playerNum))
            {
                Particle.Stop();
                rightParticle.Stop();
                audioHandler.StopSFX("Spray");
            }

            if (Input.GetButton("Shoot" + Player.playerNum))
            {
                IsAxisInUse = false;
                if (Ammo > 0)
                {
                    if (dazeState.CanMove == true)
                    {
                        if (IsAxisInUse == false)
                        {

                            if (canPlaySplat)
                            {
                                audioHandler.SetSFX("Spray");
                                StartCoroutine(SplatSFXCooldown());
                            }

                            if (range > maxRange)
                            {
                                range = maxRange;
                            }
                            Ammo -= (ammoConsumption - (AmmoConsumptionModifier)) * Time.deltaTime;

                            TwinSprayShooting();
                            TwinShootTwo();
                            UpdateFillBar();
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
                }
                else
                {
                    if (Particle.isPlaying)
                    {
                        Particle.Stop();
                        rightParticle.Stop();
                        audioHandler.StopSFX("Spray");
                    }
                }
            }
        }
    }


    private void TwinSprayShooting()
    {
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
                    //  Debug.Log(_endRayCopy + " " + (this.transform.forward - this.transform.up).normalized);
                    if (Physics.Raycast(_newRay, out hit, range))
                    {
                        // Debug.Log("if forward!");
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

    private void TwinShootTwo()
    {
        Vector3 _lineOffset = ray.direction * range;
        ray = new Ray(SecondGunMuzzle.transform.position, SecondGunMuzzle.transform.forward);
        Debug.DrawRay(SecondGunMuzzle.transform.position, SecondGunMuzzle.transform.forward * range, Color.green);
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
    private void CollideWith(string tag, PlayerBase playerBase)
    {
        //Debug.Log("Collide with function");
        switch (tag)
        {
            case "Player":
                GameObject player = hit.transform.gameObject;
                DazeState pDaze = player.GetComponent<DazeState>();
                transform.gameObject.GetComponent<DazeState>().KnockBackEffect(ray.direction, knockBackForce);

                break;
            case "PaintableEnvironment":
                //Renderer _wallRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                // Debug.Log("running paint!");
                float _smult;

                if (hit.collider.GetComponent<PaintSizeMultiplier>())
                {
                    _smult = (1f * hit.collider.GetComponent<PaintSizeMultiplier>().multiplier) * weaponSplashMultiplier;
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
        yield return new WaitForSeconds(0.5f);
        canPlaySplat = true;

    }

}
