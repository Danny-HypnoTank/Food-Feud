using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlowDown : MonoBehaviour
{
    private float originalSpeed;
    private List<PlayerController> playersAffected = new List<PlayerController>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            playersAffected.Add(player);
            originalSpeed = player.MoveSpeedModifier;
            player.SetProperty<float>(nameof(player.MoveSpeedModifier),originalSpeed/2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            playersAffected.Add(player);
            originalSpeed = player.MoveSpeedModifier;
            player.SetProperty<float>(nameof(player.MoveSpeedModifier), originalSpeed);
        }
    }
}
