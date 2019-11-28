/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 22/10/2019
 */
using UnityEngine;

[CreateAssetMenu(fileName = "NewMap", menuName = "ScriptableObjects/Map", order = 3)]
public class MapDisplay : ScriptableObject
{
    public int mapId;
    public Sprite mapImg;
    public string mapName;
}
