/*
 * Created by:
 * Name: Akexander Watson
 * Sid: 1507490
 * Date Created: 29/09/2019
 * Last Modified: 06/10/2019
 * Modified By: Dominik Waldowski, Antoni Gudejko
 */
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Player player;                          //reference to player that owns the bullet for calculating scores.
    [SerializeField]
    private float speed = 25f;                  //how fast the object travels
    private float timeToDie = 6;                //time left to disable the object
    [SerializeField]
    private LayerMask bounceSurface;
    private int bounceLimit = 3;
    private int currentBounce = 0;

    [SerializeField]
    private float weaponSplashMultiplier = 1;

    public DrawColor DrawColor { get => drawColor; set => drawColor = value; }
    [SerializeField]
    private DrawColor drawColor;
    private ObjectAudioHandler audioHandler;

    private void Start()
    {
        audioHandler = GetComponent<ObjectAudioHandler>();
    }

    // Handles bullet logic

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Time.deltaTime * speed + 0.1f, bounceSurface))
        {
            audioHandler.SetSFX("Bounce");
            CollideWith(hit.collider.gameObject.tag, hit);
            Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
            float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rot, 0);
            currentBounce++;
            if (currentBounce > 3)
            {
                DestroyBullet();
            }
        }
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            CollideWith(hit.collider.gameObject.tag, hit);
        }
    }

    //when object becomes active it invokes function that disables it after specified time
    private void OnEnable()
    {
        Invoke("DestroyBullet", timeToDie);
        currentBounce = 0;
        drawColor = GameObject.Find("GameManager").GetComponent<DrawColor>();
    }

    //disables the bullet
    private void DestroyBullet()
    {
        this.gameObject.SetActive(false);
    }

    //detects collision with other players and environment
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    //grabs ID of player so that score can be added to appropriate player
    public void SetPlayerId(Player owner)
    {
        player = owner;
    }

    //resets all bullet variables to default
    private void OnDisable()
    {
        player = null;
    }

    private void CollideWith(string tag, RaycastHit hit)
    {
        switch (tag)
        {
            case "Player":



                break;
            case "PaintableEnvironment":
                //Renderer _wallRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                float _smult;
                if (hit.collider.GetComponent<PaintSizeMultiplier>())
                {
                    _smult = (1 * hit.collider.GetComponent<PaintSizeMultiplier>().multiplier) * weaponSplashMultiplier;
                    
                }
                else
                {
                    _smult = 1f * weaponSplashMultiplier;
                }
                if (player != null)
                {
                    int _id = player.playerNum;
                    DrawColor.DrawOnSplatmap(hit, _id, player, _smult);
                }
                break;
        }
    }
}
