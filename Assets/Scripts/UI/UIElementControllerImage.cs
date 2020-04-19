using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElementControllerImage : UIElementController
{
    [Header("Graphics")]
    [SerializeField]
    private Sprite inactiveSprite;
    [SerializeField]
    private Sprite activeSprite;

    private Image spriteRender;

    void Start()
    {
        spriteRender = GetComponent<Image>();
    }

    private void ChangeSprite(Sprite sprite)
    {
        if (spriteRender != null)
        {
            if (spriteRender.sprite != sprite)
                spriteRender.sprite = sprite;
        }
    }

    public override void SetHover()
    {
        ChangeSprite(activeSprite);
        base.SetHover();
    }

    public override void SetInactive()
    {
        ChangeSprite(inactiveSprite);
        base.SetInactive();
    }
}
