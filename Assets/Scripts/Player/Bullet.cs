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

    public DrawColor DrawColor { get => drawColor; set => drawColor = value; }
    [SerializeField]
    private DrawColor drawColor;
    // Handles bullet logic
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Time.deltaTime * speed + 0.1f, bounceSurface))
        {
            CollideWith(hit.collider.gameObject.tag, hit);
            Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
            float rot = 90 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, rot, 0);
            currentBounce++;
            if(currentBounce > 3)
            {
                DestroyBullet();
            }
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
                Debug.Log("paint");
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
                int _id = player.skinId;
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

        }
    }
}
