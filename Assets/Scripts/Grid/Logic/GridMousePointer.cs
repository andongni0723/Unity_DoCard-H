using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridMousePointer : MonoBehaviour
{
    BoxCollider2D coll;

    public Vector2 BasicColliderSize = new Vector2(1.3f, 1.3f);
    public Vector2 sizeMag;

    [SerializeField] bool isCofirmArea;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        Init();
    }

    private void Update()
    {
        // Move with Mouse
        if (isCofirmArea)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
    }

    #region Event
    private void OnEnable()
    {
        EventHanlder.CardOnDrag += OnCardOnDrag; // Move with mouse
        EventHanlder.CardEndDrag += OnCardEndDrag; // Cancel move with mouse
        EventHanlder.CancelPlayTheCard += OnCancelPlayTheCard; // INIT
        EventHanlder.PayCardComplete += OnPayCardComplete; // INIT
    }



    private void OnDisable()
    {
        EventHanlder.CardOnDrag -= OnCardOnDrag;
        EventHanlder.CardEndDrag -= OnCardEndDrag;
        EventHanlder.CancelPlayTheCard -= OnCancelPlayTheCard;
        EventHanlder.PayCardComplete -= OnPayCardComplete;
    }

    private void OnCardOnDrag(CardDetail_SO cardDetail)
    {
        if (GameManager.Instance.gameStep == GameStep.PayCardStep) return;

        // setting mouse pointer offset
        switch (cardDetail.cardType)
        {
            case CardType.Attack:
                coll.size = ((cardDetail.attackTypeDetails.cardAttackOffset - Vector2.one) * sizeMag) + BasicColliderSize;
                break;

            case CardType.Move:
                coll.size = ((cardDetail.moveTypeDetails.cardMoveOffset - Vector2.one) * sizeMag) + BasicColliderSize;
                break;

            case CardType.Tank:
                coll.size = Vector2.zero;
                break;
        }

        isCofirmArea = true;
    }
    private void OnCardEndDrag()
    {
        isCofirmArea = false;
    }
    private void OnCancelPlayTheCard()
    {
        Init();
    }

    private void OnPayCardComplete()
    {
        Init();
    }
    #endregion

    private void Init()
    {
        isCofirmArea = false;
        transform.position = Vector3.zero;
    }

}
