using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementControllerSprite : UIElementController
{
    [Header("Graphics")]
    [SerializeField]
    private Sprite inactiveSprite;
    [SerializeField]
    private Sprite activeSprite;

    private SpriteRenderer spriteRender;

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
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
        base.SetHover();

        ChangeSprite(activeSprite);
    }

    public override void SetInactive()
    {
        base.SetInactive();

        ChangeSprite(inactiveSprite);
    }
}
