using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonDoneButton : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    #region Event

    private void OnEnable()
    {
        EventHanlder.AddCardDone += OnAddCardDone; // Set Button can use
    }

    private void OnDisable()
    {
        EventHanlder.AddCardDone -= OnAddCardDone;
    }

    private void OnAddCardDone()
    {
        button.interactable = true;
    }

    #endregion 

    private void Update()
    {
        //Check button can use
        // if(GameManager.Instance.gameStep != GameStep.CommonStep ||
        //    GameManager.Instance.playingCard != null)
        // {
        //     button.interactable = false;
        // }
        // else
        // {
        //     button.interactable = true;
        // }
    }

    public void PlayerDonePlayButton()
    {
        button.interactable = false;
        EventHanlder.CallCommandStepEnd();
    }
}
