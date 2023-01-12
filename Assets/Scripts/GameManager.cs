using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isPayCard;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

#region Event
    private void OnEnable()
    {
        EventHanlder.EndDragCofirmData += OnEndDragCofirmData;
        EventHanlder.PlayTheCard += OnPlayTheCard;
        EventHanlder.CancelPlayTheCard += OnCancelPlayTheCard;
    }
    private void OnDisable()
    {
        EventHanlder.EndDragCofirmData -= OnEndDragCofirmData;
        EventHanlder.PlayTheCard -= OnPlayTheCard;
        EventHanlder.CancelPlayTheCard -= OnCancelPlayTheCard;
    }

    private void OnEndDragCofirmData(ConfirmAreaGridData data)
    {
        int gridCount = data.ConfirmGridsList.Count;

        if (gridCount == data.cardDetail.cardOffset.x * data.cardDetail.cardOffset.y) // the confirm area count is right
        {
            //TODO: card pay UI
            Debug.Log("The confirm area count is right");
            EventHanlder.CallPlayTheCard(data.cardDetail);
        }
        else // the confirm area count isn't right
        {
            EventHanlder.CallCancelPlayTheCard();
        }
    }

    private void OnPlayTheCard(CardDetail_SO obj)
    {
        isPayCard = true;
    }
    private void OnCancelPlayTheCard()
    {
        isPayCard = false;
    }    
#endregion
}
