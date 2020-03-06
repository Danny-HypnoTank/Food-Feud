using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ControllerNav : MonoBehaviour
{
    private EventSystem ES;
    [SerializeField]
    private GameObject[] button;
    private int buttonID = 0;

    public int ButtonID { get => buttonID; set => buttonID = value; }

    void Start()
    {
        ES = gameObject.GetComponent<EventSystem>();
        ES.firstSelectedGameObject = null;
    }

    private void Update()
    {
         if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (ES.firstSelectedGameObject != button[buttonID])
            {
                ES.firstSelectedGameObject = button[buttonID];
                ES.SetSelectedGameObject(button[buttonID]);
            }
        }
    }
}
