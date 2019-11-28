/*
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 22/10/2019
 */
using UnityEngine;

public class ParticlePlay : MonoBehaviour
{
    private ParticleSystem particleSys;
    private void OnEnable()
    {
        particleSys = this.gameObject.GetComponent<ParticleSystem>();
        particleSys.Play();
        Invoke("DisableParticle", particleSys.main.duration);
    }

    private void DisableParticle()
    {
        this.gameObject.SetActive(false);
    }

}
