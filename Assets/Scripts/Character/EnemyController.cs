using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseCharacter
{
    #region Event
    private void OnEnable()
    {
        EventHanlder.EnemyMoveAction += OnMoveAction; //Move action (animation)
        EventHanlder.EnemyHurt += Hurt; // The Hurt text animation
    }
    private void OnDisable()
    {
        EventHanlder.EnemyMoveAction -= OnMoveAction;
        EventHanlder.EnemyHurt -= Hurt;
    }
    #endregion
}
