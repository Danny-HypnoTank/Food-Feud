/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 15/10/2019
 * Last Modified: 15/10/2019
 */
using UnityEngine;

[CreateAssetMenu(fileName ="NewPowerUp",menuName = "ScriptableObjects/PowerUp", order = 1)]
public class Powerup : ScriptableObject
{
    public GameObject powerupObject;
    public Sprite powerUpSprite;
    public int powerUpId = 999;
    public int weaponID = 999;
    public enum powerUps
    {
        powerups,
        weapons,
        godpowerup
    }
    public powerUps powerUpPower;
}
/*
        Weapons
        Default Weapon 0
        bomblauncher 1
        twinspray 2
        ricochet 3

        Mass PowerUps
        emptyammo 0
        resetweapon 1
        massfreeze 2
*/
