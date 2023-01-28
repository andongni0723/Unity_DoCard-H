using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridManager : BaseGridManager
{
    public static PlayerGridManager instance;

    protected override void Awake() {
        // Singleton
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        base.Awake();
    }
    protected override void OnEnable()
    {
        EventHanlder.EndDragPlayerGridUpdateData += UpdateData;
    }

    protected override void OnDisable()
    {
        EventHanlder.EndDragPlayerGridUpdateData -= UpdateData;
    }
}
