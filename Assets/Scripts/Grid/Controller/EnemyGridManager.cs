using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGridManager : BaseGridManager
{
    protected override void OnEnable()
    {
        EventHanlder.EndDragEnemyGridUpdateData += UpdateData;  // update data
        EventHanlder.PlayerAttackGrid += OnAttackGridToCall;  // Find the target grid and call the event 
    }

    protected override void OnDisable()
    {
        EventHanlder.EndDragEnemyGridUpdateData -= UpdateData;
        EventHanlder.PlayerAttackGrid -= OnAttackGridToCall;
    }
}