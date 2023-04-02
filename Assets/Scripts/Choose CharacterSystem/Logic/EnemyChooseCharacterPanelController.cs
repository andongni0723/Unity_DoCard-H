using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChooseCharacterPanelController : ChooseCharacterPanelController
{
    #region Event

    private void OnEnable()
    {
        EventHanlder.EnemySendCharacterDetails += OnSendCharacterDetails;
        EventHanlder.ChooseCharacterChangeStep += OnChooseCharacterChangeStep;

    }
    private void OnDisable()
    {
        EventHanlder.EnemySendCharacterDetails -= OnSendCharacterDetails;
        EventHanlder.ChooseCharacterChangeStep -= OnChooseCharacterChangeStep;

    }

    #endregion
}
