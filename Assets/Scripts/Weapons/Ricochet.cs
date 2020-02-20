using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : Shooting
{
    //Weapon Specific Stats
    //Ammo Consumption
    private float ricochetAmmoConsumption = 25;
    private ObjectPooling bullets;
    private ObjectAudioHandler audioHandler;
    #region raycast
    private RaycastHit hit;
    private Ray ray;
    private Ray downRay;
    //In case we'd like to make a more precise "arc"
    private int anglePrecision = 1;
    private int maxAngle;
    [SerializeField]
    private float maxRange = 5;
    private float minRange = 1;
    private float range = 5;
    private float rangeRateIncrease = 0.6f;

    private float knockBackForce = 5;

    [SerializeField]
    private LineRenderer lineRenderer;
    #endregion

    private void OnEnable()
    {
        Ammo = 100;
        UpdateFillBar();
        bullets = GameObject.Find("BulletsSpawn").GetComponent<ObjectPooling>();
        audioHandler = GetComponent<ObjectAudioHandler>();
    }

    private void Update()
    {
            RicochetShooting();
    }

    private void RicochetShooting()
    {
        if (ManageGame.instance.IsTimingDown == true)
        {
            //Using right Bumper
            if (Input.GetButtonDown("Shoot" + Player.playerNum))
            {
                if (Ammo > 0)
                {
                    if (CanShoot == true)
                    {
                        //if it's equal to false
                        if (IsAxisInUse == false)
                        {

                            audioHandler.SetSFX("RicochetShoot");

                            Ammo -= ricochetAmmoConsumption;
                            UpdateFillBar();
                           
                            if (ManageGame.instance.IsTimingDown == true)
                            {
                                GameObject newBullet = bullets.GetPooledObject();
                                newBullet.transform.position = GunMuzzle.transform.position;
                                newBullet.transform.rotation = GunMuzzle.transform.rotation;
                                newBullet.GetComponent<Bullet>().SetPlayerId(Player);
                                newBullet.gameObject.SetActive(true);
                                
                            }
                            IsAxisInUse = true;
                        }
                    }
                }
                if (Input.GetButton("Shoot" + Player.playerNum))
                {
                    IsAxisInUse = false;
                }
            }
        }
    }

    private void CollideWith(string tag, PlayerBase playerBase)
    {
        switch (tag)
        {
            case "Player":
                //hit.collider.gameObject.transform.Translate(ray.direction * 0.15f);
                hit.transform.gameObject.GetComponent<DazeState>().KnockBackEffect(ray.direction, knockBackForce);
                break;
            case "PaintableEnvironment":
                //Renderer _wallRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                int _id = Player.skinId;
                switch (_id)
                {
                    case (0):
                        {
                            //DrawColor.DrawOnSplatmap(hit, new Color(1, 0, 0, 0));
                            break;
                        }
                    case (1):
                        {
                            //DrawColor.DrawOnSplatmap(hit, new Color(0, 1, 0, 0));
                            break;
                        }
                    case (2):
                        {
                            //DrawColor.DrawOnSplatmap(hit, new Color(0, 0, 1, 0));
                            break;
                        }
                    case (3):
                        {
                            //DrawColor.DrawOnSplatmap(hit, new Color(0, 0, 0, 1));
                            break;
                        }
                }
                // _wallRenderer.sharedMaterial = parent.Player.skins[parent.Player.skinId].GetComponent<Renderer>().sharedMaterial;
                /*if (hit.collider.gameObject.name == "Ground")
                {
                    Instantiate(Decal, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity);
                }
                else if(hit.collider.gameObject.name == "Wall")
                {
                    Instantiate(Decal, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.Euler(90, Vector3.Angle(playerBase.transform.forward, new Vector3(hit.point.x, hit.point.y, hit.point.z)),0));
                }*/
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
