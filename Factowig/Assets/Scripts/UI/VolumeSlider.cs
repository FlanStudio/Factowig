using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.InputSystem.Controls;

#if UNITY_EDITOR

[CustomEditor(typeof(VolumeSlider))]
public class VolumeSliderEditor : Editor
{
    private SerializedProperty mode;
    private SerializedProperty progressBar;
    private SerializedProperty selectionSquare;
    private SerializedProperty text;
    private SerializedProperty useGenericInputs;

    private void OnEnable()
    {
        mode = serializedObject.FindProperty("mode");
        progressBar = serializedObject.FindProperty("progressBar");
        selectionSquare = serializedObject.FindProperty("selectionSquare");
        text = serializedObject.FindProperty("text");
        useGenericInputs = serializedObject.FindProperty("useGenericInputs");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(mode);
        EditorGUILayout.PropertyField(progressBar);
        EditorGUILayout.PropertyField(selectionSquare);
        EditorGUILayout.PropertyField(text);
        EditorGUILayout.PropertyField(useGenericInputs);

        serializedObject.ApplyModifiedProperties();

        //base.OnInspectorGUI();
    }
}

#endif

public class VolumeSlider : Slider
{
    public enum AUDIOMODE { Music, FX }
    public AUDIOMODE mode = AUDIOMODE.Music;

    public RectTransform progressBar = null;
    public GameObject selectionSquare = null;
    public TextMeshProUGUI text;

    public bool useGenericInputs = false;

    private RectTransform rectTransform = null;

    private bool isClicked = false;

    private float percent = 1f;

    private float volumeCD = 0.2f;
    private float volumeSmallCD = 0.1f;
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
        float prevPercent = percent;

        if (isClicked)
        {
            percent = (Mouse.current.position.ReadValue().x - rectTransform.position.x) / progressBar.rect.width;
            percent = Mathf.Floor(percent * 10) / 10;
            percent = Mathf.Clamp(percent, 0, 1f);

            if (prevPercent != percent)
                OnVolumeChanged();
        }
        
        if(currentSelectionState == SelectionState.Selected)
        {
            if (volumeTimer > 0f) volumeTimer -= Time.unscaledDeltaTime;
            if (volumeTimer < 0f) volumeTimer = 0f;

            InputController.PlayerInput player1Input = InputController.Instance.playerInput[0];

            if(useGenericInputs)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.delta.IsActuated(0.2f))
                {
                    player1Input.controlMode = InputController.ControlsMode.KeyboardMouse;
                }

                if (Gamepad.current != null && (Gamepad.current.allControls.Any(x => x is ButtonControl && x.IsPressed() && !x.synthetic) || Gamepad.current.leftStick.ReadValue().magnitude >= InputController.idleStickThreshold))
                {
                    player1Input.controlMode = InputController.ControlsMode.Controller;
                    player1Input.gamepad = Gamepad.current;
                }
            }

            switch (player1Input.controlMode)
            {
                case InputController.ControlsMode.KeyboardMouse:
                    break;
                case InputController.ControlsMode.Controller:
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

                            if(prevPercent != percent)
                                OnVolumeChanged();

                            volumeTimer = volumeSmallCD;
                        }
                    }
                    else
                    {
                        value = Vector2.zero;

                        if (player1Input.gamepad.dpad.left.isPressed)
                            value -= Vector2.right;
                        else if (player1Input.gamepad.dpad.right.isPressed)
                            value += Vector2.right;

                        if (value.x != 0)
                        {
                            if (player1Input.gamepad.dpad.right.wasPressedThisFrame || player1Input.gamepad.dpad.left.wasPressedThisFrame)
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

                                if (prevPercent != percent)
                                    OnVolumeChanged();

                                volumeTimer = volumeSmallCD;
                            }

                        }
                        else
                            volumeTimer = 0f;
                    }
                    
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

        AudioManager.Instance.ApplyVolumesToSources();
        AudioManager.Instance.PlaySoundEffect(AudioManager.FX.TICK);
    }
}