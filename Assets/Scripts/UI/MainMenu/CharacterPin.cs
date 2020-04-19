/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 21/11/2019
 * Last Modified: 21/11/2019
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPin : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerPinLocations;
    [SerializeField]
    private int pinId;
    [SerializeField]
    private Transform ownedBy;
    [SerializeField]
    private SpriteRenderer characterSprite;
    [SerializeField]
    private Sprite unSelectedImage, selectedImg;
    public Transform[] PlayerPinLocations { get => playerPinLocations; set => playerPinLocations = value; }
    public Transform Right { get => right; set => right = value; }
    public Transform Left { get => left; set => left = value; }
    public Transform Up { get => up; set => up = value; }
    public Transform Down { get => down; set => down = value; }
    public Transform OwnedBy { get => ownedBy; set => ownedBy = value; }
    public int PinId { get => pinId; set => pinId = value; }

    [SerializeField]
    private Transform right, left, up, down;

    private ParticleSystem pSystem;

    private void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        characterSprite.sprite = unSelectedImage;
    }

    public void OwnPin()
    {
        characterSprite.sprite = selectedImg;
        pSystem.Play();
    }
    public void UnOwnPin()
    {
        characterSprite.sprite = unSelectedImage;
        ownedBy = null;
    }

    public Transform TakeDirection(string direction, int playerPos)
    {
        if (direction == "Right" && right != null)
        {
            return right.GetComponent<CharacterPin>().PlayerPinLocations[playerPos];  
        }
        else if (direction == "Left" && left != null)
        {
            return left.GetComponent<CharacterPin>().PlayerPinLocations[playerPos]; ;
        }
        else if (direction == "Up" && up != null)
        {
            return up.GetComponent<CharacterPin>().PlayerPinLocations[playerPos]; ;
        }
        else if (direction == "Down" && down != null)
        {
            return down.GetComponent<CharacterPin>().PlayerPinLocations[playerPos]; ;
        }
        else
        {
            return null;
        }
    }
}
