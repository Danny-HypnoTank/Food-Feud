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

    [Header("Stun Attributes")]
    [SerializeField]
    private float cooldownTimeMin;
    [SerializeField]
    private float cooldownTimeMax;
    [SerializeField]
    private float stunDurationMin;
    [SerializeField]
    private float stunDurationMax;

    private float cooldownTime;
    private float stunDuration;
    private bool canBeStunned;

    [Header("Graphical Objects")]
    [SerializeField]
    private GameObject[] stunStars;

    private float mass = 3.0f;
    private Vector3 impact = Vector3.zero;

    private PlayerController playerController;
    private DefaultShooting dShooting;
    private CharacterController character;
    private PlayerBase playerbase;
    private StunBehavior stunBehavior;

    public bool Stunned { get; private set; }
    public bool CanShoot { get; private set; }

    private void Start()
    {
        playerbase = GetComponent<PlayerBase>();
        character = GetComponent<CharacterController>();
        dShooting = GetComponent<DefaultShooting>();
        stunBehavior = GetComponent<StunBehavior>();
        playerController = GetComponent<PlayerController>();

        canBeStunned = true;
    }

    public void KnockBackEffect(Vector3 direction, float force)
    {
        direction.Normalize();
        if (direction.y < 0) direction.y = -direction.y;
        {
            impact += direction.normalized * force / mass;
        }
    }

    public IEnumerator Stun(float dashAmount)
    {
        if (playerController.StunImmunityPowerup != null)
        {
            playerController.StunImmunityPowerup.End();
        }
        else
        {
            if (canBeStunned)
            {
                stunStars.ToggleGameObjects(true);

                Stunned = true;
                CanShoot = false;
                stunDuration.CalculateFromPercentage(stunDurationMin, stunDurationMax, dashAmount);

                yield return new WaitForSeconds(stunDuration);

                stunStars.ToggleGameObjects(false);

                Stunned = false;
                CanShoot = true;
                cooldownTime.CalculateFromPercentage(cooldownTimeMin, cooldownTimeMax, dashAmount);

                StartCoroutine(StunCooldown(cooldownTime));
            }
        }
    }

    private IEnumerator StunCooldown(float time)
    {

        canBeStunned = false;

        yield return new WaitForSeconds(time);

        canBeStunned = true;

    }

}