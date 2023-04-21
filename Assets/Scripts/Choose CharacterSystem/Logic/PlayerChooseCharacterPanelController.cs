using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChooseCharacterPanelController : ChooseCharacterPanelController
{
    private void Awake()
    {
        character = Character.Player;
    }
    
    
    #region Event

    public new void OnEnable()
    {
        base.OnEnable();
        EventHanlder.PlayerChooseCharacterGridButton += OnChooseCharacterGridButton; // After button selected character , display UI
        EventHanlder.PlayerSendCharacterDetails += OnSendCharacterDetails;
    }
    private new void OnDisable()
    {
        base.OnDisable();
        EventHanlder.PlayerChooseCharacterGridButton -= OnChooseCharacterGridButton; // After button selected character , display UI
        EventHanlder.PlayerSendCharacterDetails -= OnSendCharacterDetails;
    }

    #endregion
}
