using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class EventHanlder
{
    public static event Action CardUpdatePosition;
    public static void CallCardUpdeatePosition()
    {
        CardUpdatePosition?.Invoke();
    }

    public static event Action<CardDetail_SO> CardOnDrag;
    public static void CallCardOnDrag(CardDetail_SO cardDetail)
    {
        CardOnDrag?.Invoke(cardDetail);
    }


    // End Drag
    public static event Action CardEndDrag;
    public static Func<ConfirmAreaGridData> EndDragGridUpdateData;
    public static event Action<ConfirmAreaGridData> EndDragCofirmData; //TODO: GameManager: EndDragCofirmData

    public static void CallCardEndDrag()
    {
        CardEndDrag?.Invoke();

        // "GridManager" confirm the grid of mouse choose , then sent data to "GameManager" to check data
        ConfirmAreaGridData data = EndDragGridUpdateData?.Invoke();
        EndDragCofirmData?.Invoke(data);
    }

    public static event Action<Sprite> CardOnClick;
    public static void CallCardOnClick(Sprite sprite)
    {
        CardOnClick?.Invoke(sprite);
    }

    public static event Action<CardDetail_SO> PlayTheCard;
    public static void CallPlayTheCard(CardDetail_SO cardDetail)
    {
        PlayTheCard?.Invoke(cardDetail);
    }

    public static event Action ConfirmTheCard;
    public static void CallConfirmTheCard( )
    {
        ConfirmTheCard?.Invoke();
    }
}
