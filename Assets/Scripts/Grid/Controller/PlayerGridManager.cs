using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridManager : BaseGridManager
{
    
    protected override void OnEnable()
    {
        EventHanlder.EndDragPlayerGridUpdateData += UpdateData; // update data
        EventHanlder.EnemyAttackGrid += OnAttackGridToCall; // Find the target grid and call the event 
    }

    protected override void OnDisable()
    {
        EventHanlder.EndDragPlayerGridUpdateData -= UpdateData;
        EventHanlder.EnemyAttackGrid -= OnAttackGridToCall;
    }
}
