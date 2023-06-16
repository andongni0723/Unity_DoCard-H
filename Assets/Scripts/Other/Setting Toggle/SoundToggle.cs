using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundToggle : SwitchToggle
{
    protected override void SwitchAction()
    {
        EventHanlder.CallChangeVolume(toggle.isOn? 1 : 0);
    }
}
