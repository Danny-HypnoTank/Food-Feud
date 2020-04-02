using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewButtonLogic", menuName = "Special Button/Object Dropper")]
public class ObjectDropper : SpecialButtonLogic
{
    [SerializeField]
    private string objectTag;

    private TrashDropper dropper;
    public override void DoAction()
    {
        dropper.DropTrash();
    }

    public override void Initialisation()
    {
        dropper = GameObject.FindGameObjectWithTag(objectTag).GetComponent<TrashDropper>();
    }
}
