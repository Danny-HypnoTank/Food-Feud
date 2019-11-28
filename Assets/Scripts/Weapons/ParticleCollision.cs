using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem particle;
    private List<ParticleCollisionEvent> collisions;

    void Start()
    {
        collisions = new List<ParticleCollisionEvent>();

    }

    private void OnParticleCollision(GameObject other)
    {

        ParticlePhysicsExtensions.GetCollisionEvents(particle, other, collisions);

        PlayerBase player = GetComponent<PlayerBase>();

        for (int i = 0; i < collisions.Count; i++)
        {

            //TODO: Implement painting
            Debug.Log($"{collisions[i]}");

        }

    }

}
