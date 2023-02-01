using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : BaseCharacter
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("slash01");
        }
    }

    #region Event
    private void OnEnable()
    {
        EventHanlder.MoveAction += OnMoveAction;
    }
    private void OnDisable() 
    {
        EventHanlder.MoveAction -= OnMoveAction;

    }
    #endregion
}
