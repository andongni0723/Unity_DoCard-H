using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
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

    private void OnMoveAction(ConfirmAreaGridData data)
    {
        // The data list first element is the to move grid
        GameObject toMoveGrid = PlayerGridManager.instance.GridPosToFindGrid(data.ConfirmGridsList[0]); 
        Debug.Log(toMoveGrid.name);
        Vector3 toParentPosition = new Vector3(0, 0.8f, -1);

        transform.parent = toMoveGrid.transform;
        transform.DOMove(transform.parent.transform.position + toParentPosition, 1);
        
    }
    #endregion
}
