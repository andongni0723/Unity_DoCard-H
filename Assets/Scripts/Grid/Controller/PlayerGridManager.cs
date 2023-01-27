using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridManager : BaseGridManager
{
    protected override void OnEnable()
    {
        EventHanlder.EndDragPlayerGridUpdateData += UpdateData;
    }

    protected override void OnDisable()
    {
        EventHanlder.EndDragPlayerGridUpdateData -= UpdateData;
    }
}
