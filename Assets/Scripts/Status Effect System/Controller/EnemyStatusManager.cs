using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusManager : BaseStatusManager
{
    protected override void OnEnable()
    {
        base.OnEnable();
        EventHanlder.OnPlayerSettlement += UpdataCurrentHurtStatusToGameManger;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EventHanlder.OnPlayerSettlement -= UpdataCurrentHurtStatusToGameManger;
    }
}
