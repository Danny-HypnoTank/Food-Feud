/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 01/10/2019
 * Last Modified: 01/10/2019
 */
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    private float value = 30;
    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(0,0, value * Time.deltaTime);
    }
}
