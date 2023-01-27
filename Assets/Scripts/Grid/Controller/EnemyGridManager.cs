using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGridManager : BaseGridManager
{
    protected override void OnEnable()
    {
        EventHanlder.EndDragEnemyGridUpdateData += UpdateData;
    }

    protected override void OnDisable()
    {
        EventHanlder.EndDragEnemyGridUpdateData -= UpdateData;
    }
}