using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCardDetail : MonoBehaviour
{
    Image image;


    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
        image.sprite = null;
    }

    private void OnEnable()
    {
        EventHanlder.CardOnClick += OnCardOnClick;
    }

    private void OnDisable()
    {
        EventHanlder.CardOnClick -= OnCardOnClick;
    }

    private void OnCardOnClick(Sprite cardSprite)
    {
        // Show big card Details
        if(cardSprite != null)
        {
            image.sprite = cardSprite;
            image.enabled = true;
        }
        else
        {
            image.sprite = null;
            image.enabled = false;
        }
    }
}
