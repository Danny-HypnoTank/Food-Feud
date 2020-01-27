/*
 * Created by:
 * Name: James Sturdgess
 * Sid: 1314371
 * Date Created: 04/10/2019
 * Last Modified 06/10/2019
 * Modified By:
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class DazeState : MonoBehaviour
{
    private DefaultShooting dShooting;
    private CharacterController character;
    private float speedUpDuration = 2;
    public float StunProgress;
    private bool stunned;
    private bool canShoot;
    private Vector3 impact = Vector3.zero;
    private float mass = 3.0f;
    private PlayerBase playerbase;
    private StunBehavior stunBehavior;
    [SerializeField]
    private GameObject[] stunStars;
    private PlayerController playerController;
    public bool Recovering { get; private set; }

    public bool Stunned { get => stunned; set => stunned = value; }
    public bool CanShoot { get => canShoot; set => canShoot = value; }

    private void Start()
    {
        playerbase = GetComponent<PlayerBase>();
        character = this.gameObject.GetComponent<CharacterController>();
        dShooting = this.GetComponent<DefaultShooting>();
        stunBehavior = this.GetComponent<StunBehavior>();
        playerController = this.GetComponent<PlayerController>();
        stunned = false;
        canShoot = true;
    }
    public IEnumerator SpeedUp(Player player, float increasedSpeed)
    {
        player.Speed = increasedSpeed;
        yield return new WaitForSeconds(speedUpDuration);
        player.Speed = player.DefaultSpeed;
    }

    public void KnockBackEffect(Vector3 direction, float force)
    {
        direction.Normalize();
        if (direction.y < 0) direction.y = -direction.y;
        {
                impact += direction.normalized * force / mass;
        }
    }

    public IEnumerator RemoveStun()
    {

        //Initial Delay
        yield return new WaitForSeconds(1);

        Recovering = true;

        //Recover Stun Bar in increments
        for (float i = StunProgress; i > 0; i -= 0.1f)
        {
            StunProgress = i;
            //  bar.fillAmount = StunProgress;
            yield return new WaitForSeconds(0.5f);
        }

        StunProgress = 0;
        // bar.fillAmount = StunProgress;

        Recovering = false;

        stunned = false;

    }

    public IEnumerator Stun(Player player)
    {
        stunStars[0].SetActive(true);
        stunStars[1].SetActive(true);
        stunStars[2].SetActive(true);
        stunStars[3].SetActive(true);
        player.Speed = 0f;
        this.gameObject.GetComponent<CharacterController>().Move(new Vector3(0, 0, 0));
        playerController.MoveSpeedModifier = 0f;
        stunned = true;
        canShoot = false;
        yield return new WaitForSeconds(3.0f);
        stunStars[0].SetActive(false);
        stunStars[1].SetActive(false);
        stunStars[2].SetActive(false);
        stunStars[3].SetActive(false);
        player.Speed = player.DefaultSpeed;
        playerController.MoveSpeedModifier = 5f;
        stunned = false;
        canShoot = true;
    }

    public void AddStun(float value = 0.35f)
    {
        StopCoroutine("RemoveStun");
        StartCoroutine("RemoveStun");
        if (StunProgress < 1)
        {
            StunProgress += value;
            StartCoroutine("Stun");
            // bar.fillAmount = StunProgress;

        }
    }

    private void Update()
    {
        if (impact.magnitude > 0.2f) character.Move(impact * Time.deltaTime);
        {
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
        }
            if (StunProgress >= 0.3f && StunProgress <= 0.69f)
            {
                stunStars[0].SetActive(true);
                stunStars[1].SetActive(false);
                stunStars[2].SetActive(false);
                stunStars[3].SetActive(false);
            }
            else if (StunProgress >= 0.7f && StunProgress <= 0.99f)
            {
                stunStars[0].SetActive(true);
                stunStars[1].SetActive(true);
                stunStars[2].SetActive(false);
                stunStars[3].SetActive(false);
            }
            else if (StunProgress > 1)
            {
                stunStars[0].SetActive(true);
                stunStars[1].SetActive(true);
                stunStars[2].SetActive(true);
                stunStars[3].SetActive(true);
            }
            if (StunProgress <= 0)
            {
                stunStars[0].SetActive(false);
                stunStars[1].SetActive(false);
                stunStars[2].SetActive(false);
                stunStars[3].SetActive(false);
            }
        
        //if (Stunned)
        //transform.Rotate(0, 2, 0);
    }


}


/*public class DazeState : State<PlayerBase>
{

    private static DazeState _instance;

    public static DazeState Instance
    {

        get
        {

            if (_instance == null)
                _instance = new DazeState();

            return _instance;

        }

    }

    public override void EnterState(PlayerBase parent)
    {
        parent.StartCoroutine(DazeTimer(parent));
    }

    public override void ExitState(PlayerBase parent)
    {

    }

    public override void UpdateState(PlayerBase parent)
    {

        parent.transform.Rotate(0, parent.MoveSpeed, 0);

    }

    private IEnumerator DazeTimer(PlayerBase parent)
    {

        //TODO: implement break free control

        yield return new WaitForSeconds(5.0f);

        parent.StateMachine.ChangeState(IdleState.Instance);

    }*/
