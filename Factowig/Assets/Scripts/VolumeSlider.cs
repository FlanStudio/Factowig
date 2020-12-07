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

    private float percent = 1f;

    private float volumeCD = 0.4f;
    private float volumeSmallCD = 0.3f;
    private float volumeTimer = 0f;

    protected override void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        switch(mode)
        {
            case AUDIOMODE.Music:
                percent = AudioManager.musicVolume;
                break;
            case AUDIOMODE.FX:
                percent = AudioManager.fxVolume;
                break;
        }
        OnVolumeChanged();

        base.Awake();
    }

    protected override void Update()
    {
        if(isClicked)
        {
            percent = (Mouse.current.position.ReadValue().x - rectTransform.position.x) / progressBar.rect.width;
            percent = Mathf.Floor(percent * 10) / 10;
            percent = Mathf.Clamp(percent, 0, 1f);

            OnVolumeChanged();
        }
        
        if(currentSelectionState == SelectionState.Selected)
        {
            InputController.PlayerInput player1Input = InputController.Instance.playerInput[0];
            switch (player1Input.controlMode)
            {
                case InputController.ControlsMode.KeyboardMouse:
                    break;
                case InputController.ControlsMode.Controller:
                    if(volumeTimer > 0f) volumeTimer -= Time.unscaledDeltaTime;
                    if (volumeTimer < 0f) volumeTimer = 0f;
                    Vector2 value = player1Input.gamepad.leftStick.ReadValue();
                    if (Mathf.Abs(value.x) >= InputController.idleStickThreshold)
                    {
                        if (Mathf.Abs(player1Input.gamepad.leftStick.ReadValueFromPreviousFrame().x) < InputController.idleStickThreshold)
                            volumeTimer = volumeCD;
                        if (volumeTimer == 0f)
                        {
                            if (value.x < 0)
                            {
                                percent = (Mathf.Round(percent * 10) - 1) / 10;
                                percent = Mathf.Clamp(percent, 0f, 1f);
                            }
                            else
                            {
                                percent = (Mathf.Round(percent * 10) + 1) / 10;
                                percent = Mathf.Clamp(percent, 0f, 1f);
                            }

                            OnVolumeChanged();

                            volumeTimer = volumeSmallCD;
                        }

                    }
                    else
                        volumeTimer = 0f;
                    break;
            }
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
        base.OnSelect(eventData);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        selectionSquare.SetActive(false);
        base.OnDeselect(eventData);
    }

    private void OnVolumeChanged()
    {
        progressBar.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - (1 - percent) * progressBar.rect.width, progressBar.anchoredPosition.y);
        text.text = (percent * 10).ToString();

        switch (mode)
        {
            case AUDIOMODE.Music:
                AudioManager.musicVolume = percent;
                break;
            case AUDIOMODE.FX:
                AudioManager.fxVolume = percent;
                break;
        }
    }
}
