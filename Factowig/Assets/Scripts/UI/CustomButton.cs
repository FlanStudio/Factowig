using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Events;

public class CustomButton : Button
{
    public Sprite normalSprite = null;
    public Sprite hoverSprite = null;
    public Sprite clickSprite = null;

    private Image img = null;

    private Animator anim = null;

    protected override void Awake()
    {
        img = GetComponent<Image>();
        anim = GetComponent<Animator>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(anim)
            anim.SetTrigger("OnClick");
        base.OnPointerDown(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (anim)
            StartCoroutine(PlayClickAnimation(eventData));
        else
            base.OnPointerClick(eventData);
    }

    private IEnumerator PlayClickAnimation(PointerEventData eventData)
    {
        yield return new WaitUntil(() => { AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0); if (info.IsName("Unfold") && info.normalizedTime >= 1f) return true; else return false; });
        onClick.Invoke();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Select();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        if(hoverSprite)
            img.sprite = hoverSprite;

        if(anim)
            anim.SetBool("selected", true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        if(normalSprite)
            img.sprite = normalSprite;

        if(anim)
            anim.SetBool("selected", false);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
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