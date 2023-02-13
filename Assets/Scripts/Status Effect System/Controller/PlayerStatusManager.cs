using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : BaseStatusManager
{
    private void OnEnable()
    {
        EventHanlder.OnEnemySettlement += UpdataCurrentHurtStatusToGameManger;
    }
    private void OnDisable()
    {
        EventHanlder.OnEnemySettlement += UpdataCurrentHurtStatusToGameManger;
    }
}
