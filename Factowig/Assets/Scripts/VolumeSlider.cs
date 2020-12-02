using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

[CustomEditor(typeof(VolumeSlider))]
public class VolumeSliderEditor : Editor
{
    private SerializedProperty mode;
    private SerializedProperty progressBar;
    private SerializedProperty selectionSquare;
    private SerializedProperty text;

    private void OnEnable()
    {
        mode = serializedObject.FindProperty("mode");
        progressBar = serializedObject.FindProperty("progressBar");
        selectionSquare = serializedObject.FindProperty("selectionSquare");
        text = serializedObject.FindProperty("text");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(mode);
        EditorGUILayout.PropertyField(progressBar);
        EditorGUILayout.PropertyField(selectionSquare);
        EditorGUILayout.PropertyField(text);
        serializedObject.ApplyModifiedProperties();

        //base.OnInspectorGUI();
    }
}

public class VolumeSlider : Slider
{
    public enum AUDIOMODE { Music, FX }
    public AUDIOMODE mode = AUDIOMODE.Music;

    public RectTransform progressBar = null;
    public GameObject selectionSquare = null;
    public TextMeshProUGUI text;

    private RectTransform rectTransform = null;

    private bool isClicked = false;

    protected override void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        base.Awake();
    }

    protected override void Update()
    {
        if(isClicked)
        {
            float percent = (Mouse.current.position.ReadValue().x - rectTransform.position.x) / progressBar.rect.width;
            percent = Mathf.Floor(percent * 10) / 10;
            percent = Mathf.Clamp(percent, 0, 1f);

            progressBar.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - (1 - percent) * progressBar.rect.width, progressBar.anchoredPosition.y);

            text.text = (percent * 10).ToString();
        }
        
        base.Update();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Select();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
    }

    public override void OnSelect(BaseEventData eventData)
    {
        selectionSquare.SetActive(true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        selectionSquare.SetActive(false);
    }
}
