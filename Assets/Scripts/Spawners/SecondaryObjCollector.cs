using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryObjCollector : MonoBehaviour
{
    private SecondaryObjectiveSpawner spawner;
    [SerializeField]
    private GameObject secondaryObj;
    [SerializeField]
    private bool hasSecondaryObj;
    [SerializeField]
    private float basePointValue;
    private float currentPoints;
    [SerializeField]
    private Transform pointAboveHead;

    public GameObject SecondaryObj { get => secondaryObj; set => secondaryObj = value; }
    public bool HasSecondaryObj { get => hasSecondaryObj; set => this.hasSecondaryObj = value; }

    public void SetSpawner(SecondaryObjectiveSpawner _sapwner)
    {
        spawner = _sapwner;
    }

    public void Delivery()
    {
        //currentPoints adds to total points
        hasSecondaryObj = false;
        secondaryObj.transform.SetParent(spawner.SpawnLoc);
        secondaryObj.transform.position = spawner.SpawnLoc.transform.position;
        secondaryObj.SetActive(false);
        secondaryObj = null;
        spawner.ResetSecondaryObj();
    }

    public void DropSecondaryObj()
    {
        hasSecondaryObj = false;
        secondaryObj.transform.SetParent(spawner.SpawnLoc);
        secondaryObj.GetComponent<SecondaryObjective>().BoxCollision.isTrigger = false;
        secondaryObj.GetComponent<SecondaryObjective>().Dropped(this.transform);
        secondaryObj = null;
    }

    private void Update()
    {
        if(hasSecondaryObj == true && secondaryObj != null)
        {
            secondaryObj.transform.position = pointAboveHead.transform.position;
        }
    }

    public void TransferObj(GameObject obj)
    {
        secondaryObj = obj;
        secondaryObj.transform.SetParent(pointAboveHead);
        secondaryObj.transform.position = pointAboveHead.position;
        hasSecondaryObj = true;
        obj.GetComponent<SecondaryObjective>().BoxCollision.enabled = false;
        obj.GetComponentInChildren<Renderer>().material.SetColor("_BaseColor",this.GetComponent<PlayerBase>().Player.SkinColours[this.GetComponent<PlayerBase>().Player.skinId]);
    }
}
