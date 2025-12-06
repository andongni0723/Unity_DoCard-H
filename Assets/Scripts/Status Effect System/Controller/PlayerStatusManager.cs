using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : BaseStatusManager
{
    protected override void OnEnable()
    {
        base.OnEnable();
        EventHanlder.OnEnemySettlement += UpdataCurrentHurtStatusToGameManger;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHanlder.OnEnemySettlement -= UpdataCurrentHurtStatusToGameManger;
    }
}
