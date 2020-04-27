using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDropper : MonoBehaviour
{

    [SerializeField]
    private GameObject[] trashObjects;

    private List<int> usedTrash = new List<int>();
    private bool isValidTrash;

    public void DropTrash()
    {
        isValidTrash = false;

        while(!isValidTrash)
        {
            int trashToEnable = Random.Range(0, trashObjects.Length);
            if (!usedTrash.Contains(trashToEnable))
            {
                usedTrash.Add(trashToEnable);
                trashObjects[trashToEnable].SetActive(true);
                isValidTrash = true;
            }

            if (usedTrash.Count == trashObjects.Length)
                break;
        }
        
    }
}
