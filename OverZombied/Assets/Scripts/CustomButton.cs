using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

public class CustomButton : Button
{
    public Sprite normalSprite = null;
    public Sprite hoverSprite = null;
    public Sprite clickSprite = null;

    private Image img = null;

    protected override void Awake()
    {
        img = GetComponent<Image>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        img.sprite = hoverSprite;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        img.sprite = normalSprite;
    }
}

[CustomEditor(typeof(CustomButton))]
public class CustomButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //CustomButton button = (CustomButton)target;

        //button.hoverSprite = (Sprite)EditorGUILayout.ObjectField("Sprite", null, typeof(Sprite), allowSceneObjects: true);
        //button.clickSprite = (Sprite)EditorGUILayout.ObjectField("Sprite", null, typeof(Sprite), allowSceneObjects: true);

        //DrawDefaultInspector();
    }
}