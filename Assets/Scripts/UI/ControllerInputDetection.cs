/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 29/09/2019
 * Last Modified: 07/10/2019
 * Modified by: Danny Pym-Hember, Dominik Waldowski
 */
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerInputDetection : MonoBehaviour
{

    private EventSystem eS;
    [SerializeField]
    private GameObject storeSelected;

    // Start is called before the first frame update
    private void Start()
    {
        eS = this.gameObject.GetComponent<EventSystem>();
        //   storeSelected = eS.firstSelectedGameObject;
        eS.firstSelectedGameObject = null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (eS.firstSelectedGameObject == null)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                eS.firstSelectedGameObject = storeSelected;
                eS.SetSelectedGameObject(storeSelected);
            }
        }
        else if(eS.currentSelectedGameObject != storeSelected)
        {
            if (eS.currentSelectedGameObject == null)
            {
                eS.SetSelectedGameObject(storeSelected);
            }
            else
            {
                storeSelected = eS.currentSelectedGameObject;
            }
        }
    }

    public void SetMainButton(GameObject btn)
    {
        storeSelected = btn;
        eS.firstSelectedGameObject = storeSelected;
        eS.SetSelectedGameObject(storeSelected);
    }

}