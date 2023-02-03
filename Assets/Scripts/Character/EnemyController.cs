using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseCharacter
{
    #region Event
    private void OnEnable()
    {
        EventHanlder.EnemyHurt += Hurt;
    }
    private void OnDisable()
    {
        EventHanlder.EnemyHurt -= Hurt;
    }
    #endregion
}
