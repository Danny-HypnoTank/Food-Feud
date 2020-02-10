using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPointsManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] deliveryPoints;

    public Transform[] DeliveryPoints { get => deliveryPoints; set => deliveryPoints = value; }

    public void SetDeliveryPoints()
    {
        for (int i = 0; i < deliveryPoints.Length; i++)
        {
            deliveryPoints[i].gameObject.SetActive(false);
        }
    }
}
