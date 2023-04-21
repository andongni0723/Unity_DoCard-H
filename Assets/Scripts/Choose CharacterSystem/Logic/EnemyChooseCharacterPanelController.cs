using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChooseCharacterPanelController : ChooseCharacterPanelController
{
    public List<ChooseCharacterDetails_SO> AIChooseCharacterDetailsList = new List<ChooseCharacterDetails_SO>();

    private void Awake()
    {
        character = ModeManager.Instance.gameMode == GameMode.PVP ? Character.Enemy : Character.AI;
    }
    
    protected override void Start()
    {
        OnChooseCharacterChangeStep();
        
        if(character == Character.Enemy)
            LoadChooseCharacterBarElements(ChooseCharacterDetailsList);
        else if(character == Character.AI)
            LoadChooseCharacterBarElements(AIChooseCharacterDetailsList);
    }
    #region Event

    public new void OnEnable()
    {
        base.OnEnable();
        EventHanlder.EnemyChooseCharacterGridButton += OnChooseCharacterGridButton; // After button selected character , display UI
        EventHanlder.EnemySendCharacterDetails += OnSendCharacterDetails;
    }
    private new void OnDisable()
    {
        base.OnDisable();
        EventHanlder.EnemyChooseCharacterGridButton -= OnChooseCharacterGridButton; // After button selected character , display UI
        EventHanlder.EnemySendCharacterDetails -= OnSendCharacterDetails;

    }

    #endregion
}
