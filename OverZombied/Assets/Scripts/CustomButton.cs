using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    public Sprite normalSprite;
    public Sprite hoverSprite;
    public Sprite clickSprite;

    private Image image = null;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        image.sprite = hoverSprite;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        image.sprite = normalSprite;
    }
}
