/*
 * Created by:
 * Name: James Sturdgess
 * Sid: 1314371
 * Date Created: 06/10/2019
 * Last Modified 22/10/2019
 * Modified By: Dominik Waldowski, Antoni Gudejko
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private ObjectPooling explosionEffect;
    [SerializeField]
    private float blastRadius = 3f;
    public GameObject decal;
    private PlayerBase parent;
    private float bombSpeed = 3;
    private float deathTime = 15;                                   //time until bomb disappears automatically
    private float bombThrowDistance = 10;                           //how far the bomb will travel
    private float bombTopHeight = 2.2f;                                //how high the arc of parabola will be
    private float startTime;                                        //Currently time when bomb activates
    private float journeyLength;                                   //total length of journey travelled by bomb
    private Vector3 endPos;                                        //endPosition of the bomb(where it will land)
    private Vector3 startPos;                                      //start position of the bomb(where it becomes enabled)
    public PlayerBase Parent { get => parent; set => parent = value; }


    [SerializeField]
    private List<Material> bombMaterials;

    [SerializeField]
    private float weaponSplashMultiplier = 1;

    RaycastHit hit;
    Ray ray;
    private Renderer rend;
    
    public DrawColor DrawColor { get => drawColor; set => drawColor = value; }
    [SerializeField]
    private DrawColor drawColor;
    private void OnEnable()
    {
        startTime = 0;
        startTime = Time.time;
        startPos = transform.position;                                //used for calculating time
        endPos = this.transform.position + (transform.forward * bombThrowDistance);            //sets destination to which the bomb will move
        startPos = this.transform.position;                              //sets start position which is the current position of the bomb
        journeyLength = Vector3.Distance(startPos, endPos);         //calculates total distance by taking start and end point
        explosionEffect = GameObject.Find("EffectsPool").GetComponent<ObjectPooling>();
        Invoke("DestroyBomb", deathTime);

        drawColor = GameObject.Find("GameManager").GetComponent<DrawColor>();

        rend = gameObject.GetComponent<Renderer>();

        
    }

    //sets bomb to inactive
    private void DestroyBomb()
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        Parent = null;
        CancelInvoke("DisableCollision");
    }

    //sets the object that thrown bomb used for calculating whose colour bomb should paint in
    public void SetParent(PlayerBase p)
    {

        Parent = p;
        switch (Parent.Player.skinId)
        {
            case (0):
                {
                    rend.material = bombMaterials[0];
                    foreach(Transform child in transform)
                    {
                        child.GetComponent<Renderer>().material = bombMaterials[0];
                        if(child.GetComponent<ParticleSystem>())
                        {
                           ParticleSystem _ps = child.GetComponent<ParticleSystem>();
                            var main = _ps.main;
                            main.startColor = parent.Player.SkinColours[parent.Player.skinId];
                        }
                    }
                    break;
                }
            case (1):
                {
                    rend.material = bombMaterials[1];
                    foreach (Transform child in transform)
                    {
                        child.GetComponent<Renderer>().material = bombMaterials[1];
                        if (child.GetComponent<ParticleSystem>())
                        {
                            ParticleSystem _ps = child.GetComponent<ParticleSystem>();
                            var main = _ps.main;
                            main.startColor = parent.Player.SkinColours[parent.Player.skinId];
                        }
                    }
                    break;
                }
            case (2):
                {
                    rend.material = bombMaterials[2];
                    foreach (Transform child in transform)
                    {
                        child.GetComponent<Renderer>().material = bombMaterials[2];
                        if (child.GetComponent<ParticleSystem>())
                        {
                            ParticleSystem _ps = child.GetComponent<ParticleSystem>();
                            var main = _ps.main;
                            main.startColor = parent.Player.SkinColours[parent.Player.skinId];
                        }
                    }
                    break;
                }
            case (3):
                {
                    rend.material = bombMaterials[3];
                    foreach (Transform child in transform)
                    {
                        child.GetComponent<Renderer>().material = bombMaterials[3];
                        if (child.GetComponent<ParticleSystem>())
                        {
                            ParticleSystem _ps = child.GetComponent<ParticleSystem>();
                            var main = _ps.main;
                            main.startColor = parent.Player.SkinColours[parent.Player.skinId];
                        }
                    }
                    break;
                }
        }
    }

    //Moves the bomb using parabola class. 
    private void Update()
    {
        float distCovered = (Time.time - startTime) * bombSpeed;
        journeyLength = Vector3.Distance(startPos, endPos);
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = MathParabola.Parabola(startPos, endPos, bombTopHeight, fractionOfJourney * bombSpeed);
    }

    //Triggers explosion on collision
    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject != parent.gameObject)
            {
            ExplosionEffect();
            Explode();
            }
    }

    private void ExplosionEffect()
    {
        GameObject newExplosion = explosionEffect.GetPooledObject();
        newExplosion.transform.position = this.transform.position;
        newExplosion.transform.rotation = this.transform.rotation;

        ParticleSystem ps = newExplosion.GetComponent<ParticleSystem>();
        var mainPs = ps.main;
        mainPs.startColor = parent.Player.SkinColours[parent.Player.skinId];

        newExplosion.SetActive(true);
    }
    //calculates explosion radius
    private void Explode()
    {
        Vector3 dwn = transform.TransformDirection(-Vector3.up);
        if (Physics.Raycast(transform.position, dwn, out hit, Mathf.Infinity))
        {
            CollideWith(hit.collider.gameObject.tag);
        }
        //TODO: implement bomb laying paint

        DestroyBomb();

    }
    private void CollideWith(string tag)
    {
        switch (tag)
        {
            case "Player":



                break;
            case "PaintableEnvironment":
                //Renderer _wallRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                int _id = parent.Player.playerNum;
                float _smult;
                if (hit.collider.GetComponent<PaintSizeMultiplier>())
                {
                    _smult = (1 * hit.collider.GetComponent<PaintSizeMultiplier>().multiplier) * weaponSplashMultiplier;
                }
                else
                {
                    _smult = 1f * weaponSplashMultiplier;
                }

                DrawColor.DrawOnSplatmap(hit, _id, parent.Player, _smult);
                break;

        }
    }
}
