/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 01/10/2019
 * Last Modified: 01/10/2019
 */
using UnityEngine;

public class SpinHorizontal : MonoBehaviour
{

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0, 30 * Time.deltaTime,0 );
    }
}
