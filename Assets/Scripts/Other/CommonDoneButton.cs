using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonDoneButton : MonoBehaviour
{
    public void PlayerDonePlayButton()
    {
        EventHanlder.CallCommandStepEnd();
        //TODO: let card can del when step over
    }
}
