using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SwitchToggle : MonoBehaviour
{
    [Header("UI element")]
    public GameObject background;
    public GameObject handle;
    
    [Header("Color Setting")]
    public Color32 normalColor;
    public Color32 openColor;
    
    protected Toggle toggle;
    private RectTransform handleRectTransform;
    private Image backgroundImage;
    private Vector3 handlePosition;
    protected bool isOn;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handleRectTransform = handle.GetComponent<RectTransform>();
        handlePosition = handleRectTransform.anchoredPosition;

        backgroundImage = background.GetComponent<Image>();

        // INIT
        OnSwitch();
    }
    
    
    /// <summary>
    /// Toggle Event
    /// </summary>
    public void OnSwitch()
    {
        //toggle.isOn = !toggle.isOn;
        isOn = toggle.isOn;
        
        // Handle Move
        handleRectTransform.DOAnchorPos(isOn ? handlePosition * -1 : handlePosition, 0.4f);
        
        // Background color change
        backgroundImage.DOColor(isOn ? openColor : normalColor, 0.4f);
        
        // Action override by child
        SwitchAction();
    }
    
    /// <summary>
    /// Child override action about after switch toggle 
    /// </summary>
    protected virtual void SwitchAction() { }
}
