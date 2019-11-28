/* 
 * Created by:
 * Name: Alexander Watson
 * Sid: 1507490
 * Date Created: 07/10/2019
 * Last Modified: 07/10/2019
 */
using UnityEngine;

public class DisablingMouse : MonoBehaviour
{

    private bool cursorLocked = true;//To see whehter cursor is locked or not
	
	private void Start()
	{
		//Locks cursor and mouse movement
		 Cursor.lockState = CursorLockMode.Locked;
         Cursor.visible = false;
		 cursorLocked = true;
	}

    // Update is called once per frame
    void Update()
    {        
            //For testing purposes...
            //For unlocking and locking cursor to get out of playmode 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
				if(cursorLocked == true)
				{
					cursorLocked = false;
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
				else if(cursorLocked == false)
				{
                    cursorLocked = true;
				    Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
				}
            }

            
    }
}
