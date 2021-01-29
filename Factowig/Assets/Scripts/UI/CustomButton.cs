using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CustomButton : Button
{
    public GameObject selectionBox = null;
    
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

    protected void Update()
    {
        if (currentSelectionState != SelectionState.Selected)
            return;

        //InputController.PlayerInput playerInput = InputController.Instance.playerInput[0];
        //switch (playerInput.controlMode)
        //{
        //    case InputController.ControlsMode.KeyboardMouse:
        //        break;
        //    case InputController.ControlsMode.Controller:
                if(Gamepad.current != null && Gamepad.current/*gamepad*/.buttonSouth.wasPressedThisFrame)
                {
                    if (anim)
                    {
                        anim.SetTrigger("OnClick");
                        StartCoroutine(PlayClickAnimation());
                    }
                }
        //        break;
        //}
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (anim)
            anim.SetTrigger("OnClick");
        base.OnPointerDown(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (anim)
            StartCoroutine(PlayClickAnimation());
        else
            base.OnPointerClick(eventData);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        StartCoroutine(PlayClickAnimation(eventData));
    }

    private IEnumerator PlayClickAnimation(BaseEventData eventData = null)
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.FX.CLICK);

        if (anim)
            yield return new WaitUntil(() => { AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0); if (info.IsName("Unfold") && info.normalizedTime >= 1f) return true; else return false; });
        
        if (eventData != null)
            base.OnSubmit(eventData);
        else
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

        if (selectionBox)
            selectionBox.SetActive(true);

        if(img && hoverSprite)
            img.sprite = hoverSprite;  

        if(anim)
            anim.SetBool("selected", true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        if (selectionBox)
            selectionBox.SetActive(false);

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

#if UNITY_EDITOR

[CustomEditor(typeof(CustomButton))]
public class CustomButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CustomButton button = (CustomButton)target;

        button.hoverSprite = (Sprite)EditorGUILayout.ObjectField("Sprite", null, typeof(Sprite), allowSceneObjects: true);
        button.clickSprite = (Sprite)EditorGUILayout.ObjectField("Sprite", null, typeof(Sprite), allowSceneObjects: true);

        DrawDefaultInspector();
    }
}

#endif