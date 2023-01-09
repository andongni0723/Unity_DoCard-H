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

    public static event Action<Sprite> CardOnClick;
    public static void CallCardOnClick(Sprite sprite)
    {
        CardOnClick?.Invoke(sprite);
    }

    public static event Action PlayTheCard;
    public static void CallPlayTheCard()
    {
        PlayTheCard?.Invoke();
    }
}
