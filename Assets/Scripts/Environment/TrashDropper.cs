using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDropper : MonoBehaviour
{

    [SerializeField]
    private GameObject[] trashObjects;

    public void DropTrash()
    {
        int trashToEnable = Random.Range(0, trashObjects.Length);
        trashObjects[trashToEnable].SetActive(true);
    }
}
