using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    [SerializeField]
    private Powerup powerHeld;

    public Powerup PowerHeld { get => powerHeld; set => powerHeld = value; }
}
