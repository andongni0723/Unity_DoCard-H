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

    private void Update()
    {
        //Check button can use
        if(GameManager.Instance.gameStep != GameStep.CommonStep ||
           GameManager.Instance.playingCard != null)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void PlayerDonePlayButton()
    {
        EventHanlder.CallCommandStepEnd();
    }
}
