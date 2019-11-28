using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpIcons : MonoBehaviour
{
    [SerializeField]
    private Image pUpIcon;

    public void ApplyIcon(Sprite powerUpSprite)
    {
        pUpIcon.sprite = powerUpSprite;
    }
}
