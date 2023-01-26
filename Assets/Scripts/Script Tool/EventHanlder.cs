using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class EventHanlder
{
    // Softed Card Position
    public static event Action CardUpdatePosition;
    public static void CallCardUpdeatePosition()
    {
        CardUpdatePosition?.Invoke();
    }


    // Card Event
    public static event Action<CardDetail_SO> CardOnDrag;
    public static void CallCardOnDrag(CardDetail_SO cardDetail)
    {
        CardOnDrag?.Invoke(cardDetail);
    }

    public static event Action<CardDetail_SO> CardOnClick;
    public static void CallCardOnClick(CardDetail_SO data)
    {
        CardOnClick?.Invoke(data);
    }

    // End Drag
    public static event Action CardEndDrag;
    public static Func<List<ConfirmGrid>> EndDragGridUpdateData; // "GridManager" confirm the grid of mouse choose 
    public static event Action<ConfirmAreaGridData> EndDragCofirmData; //GameManager: EndDragCofirmData

    public static void CallCardEndDrag()
    {
        // cardDetail: "BasicCard" will sent CardDetail_SO
        // ConfirmGridsList: "GridManager" confirm the grid of mouse choose 
        // Then sent data to "GameManager" to check data
        ConfirmAreaGridData data = new ConfirmAreaGridData();

        data.cardDetail = GameManager.Instance.playingCard;
        data.ConfirmGridsList = EndDragGridUpdateData?.Invoke();
        
        CardEndDrag?.Invoke();

        EndDragCofirmData?.Invoke(data);
    }


    // After End Drag
    //public static event Action GameStepChange; // GameManager //FIXME: if not use , del.
    public static event Action<CardDetail_SO> PlayTheCard;
    public static void CallPlayTheCard(CardDetail_SO cardDetail)
    {
        //GameStepChange?.Invoke();
        GameManager.Instance.ChangeGameStep(GameStep.PayCardStep);
        PlayTheCard?.Invoke(cardDetail);
    }

    public static event Action<GameObject> PayTheCard;
    public static void CallPayTheCard(GameObject card)
    {
        PayTheCard?.Invoke(card);
    }

    public static event Action<GameObject> PayTheCardError;
    public static void CallPayTheCardError(GameObject card)
    {
        PayTheCardError?.Invoke(card);
    }

    public static event Action CancelPlayTheCard;
    public static void CallCancelPlayTheCard()
    {
        GameManager.Instance.ChangeGameStep(GameStep.CommonStep);
        CancelPlayTheCard?.Invoke();
    }

    public static event Action PayCardComplete;
    public static void CallPayCardComplete()
    {
        GameManager.Instance.ChangeGameStep(GameStep.CommonStep);
        PayCardComplete?.Invoke();
        EventHanlder.CallCardUpdeatePosition();
    }


    // // GameManager Reload confirm data 
    // public static event Action<List<ConfirmGrid>> ReloadGridData;
    // public static void CallReloadGridData(List<ConfirmGrid> grids)
    // {
    //     ReloadGridData?.Invoke(grids);
    // }

    // GridManager -> Grid
    public static event Action<List<ConfirmGrid>> ReloadGridColor;
    public static void CallReloadGridColor(List<ConfirmGrid> grids)
    {
        ReloadGridColor?.Invoke(grids);
    }

    public static event Action<string> SendGameMessage;
    public static void CallSendGameMessage(string message)
    {
        SendGameMessage?.Invoke(message);
    }
}
