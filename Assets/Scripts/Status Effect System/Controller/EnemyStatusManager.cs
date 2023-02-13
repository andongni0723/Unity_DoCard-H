using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusManager : BaseStatusManager
{
    private void OnEnable()
    {
        EventHanlder.OnPlayerSettlement += UpdataCurrentHurtStatusToGameManger;
    }
    private void OnDisable()
    {
        EventHanlder.OnPlayerSettlement += UpdataCurrentHurtStatusToGameManger;
    }
}
