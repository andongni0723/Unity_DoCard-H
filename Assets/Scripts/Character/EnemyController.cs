using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseCharacter
{
    #region Event
    private void OnEnable()
    {
        EventHanlder.EnemyHurt += Hurt; // The Hurt text animation
    }
    private void OnDisable()
    {
        EventHanlder.EnemyHurt -= Hurt;
    }
    #endregion
}
