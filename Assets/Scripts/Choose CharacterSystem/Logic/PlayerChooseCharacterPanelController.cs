using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChooseCharacterPanelController : ChooseCharacterPanelController
{
    #region Event

    private void OnEnable()
    {
        EventHanlder.PlayerSendCharacterDetails += OnSendCharacterDetails;
        EventHanlder.ChooseCharacterChangeStep += OnChooseCharacterChangeStep;

    }
    private void OnDisable()
    {
        EventHanlder.PlayerSendCharacterDetails -= OnSendCharacterDetails;
        EventHanlder.ChooseCharacterChangeStep -= OnChooseCharacterChangeStep;

    }

    #endregion
}
