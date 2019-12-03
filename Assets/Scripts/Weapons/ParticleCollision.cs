using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem particle;
    private bool canRipple;

    void Start()
    {

        canRipple = true;

    }

    private void OnParticleCollision(GameObject other)
    {

        particle.Emit(1);
        canRipple = false;
        StartCoroutine(Cooldown());

    }

    private IEnumerator Cooldown()
    {

        yield return new WaitForSeconds(1);

        canRipple = true;

    }

}
