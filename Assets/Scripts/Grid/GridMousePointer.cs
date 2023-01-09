using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridMousePointer : MonoBehaviour, IPointerClickHandler
{
    BoxCollider2D coll;

    public Vector2 BasicColliderSize = new Vector2(1.3f, 1.3f);

    bool isCofirmArea;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        Init();
    }

    private void Update()
    {
        // Move with Mouse
        if(isCofirmArea)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
    }

    private void OnEnable()
    {
        EventHanlder.PlayTheCard += OnPlayTheCard;
    }
    private void OnDisable()
    {
        EventHanlder.PlayTheCard -= OnPlayTheCard;
    }

    private void OnPlayTheCard(CardDetail_SO cardDetail)
    {
        // setting mouse pointer offset
        coll.size = cardDetail.cardOffset * BasicColliderSize; 
        isCofirmArea = true;
    }
    

    public void OnPointerClick(PointerEventData eventData)
    {
        //FIXME: Click bug
        //TODO: Grid Manager to sent data about confirm grids
        Debug.Log("click");
        Init();
        EventHanlder.CallConfirmTheCard();
    }

    private void Init()
    {
        isCofirmArea = false;
        transform.position = Vector3.zero;
    }

}
