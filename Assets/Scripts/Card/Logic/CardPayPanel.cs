using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPayPanel : MonoBehaviour
{
    public GameObject panel;
    public GameObject payCards;

    CardDetail_SO cardData;
    private void Awake()
    {
        panel = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        PayCardCheck(cardData);
    }

    #region Event
    private void OnEnable()
    {
        EventHanlder.PlayTheCard += OnPlayTheCard;
        EventHanlder.PayTheCard += OnPayTheCard;
    }
    private void OnDisable()
    {
        EventHanlder.PlayTheCard -= OnPlayTheCard;
        EventHanlder.PayTheCard -= OnPayTheCard;
    }


    private void OnPlayTheCard(CardDetail_SO data)
    {
        panel.SetActive(true);
        cardData = data;
        if (payCards.transform.childCount >= data.payCardNum)
        {
            //TODO: Pay card complete
        }
    }
    private void OnPayTheCard(GameObject card)
    {
        card.transform.parent = payCards.transform;
    }

    public void CancelPlayTheCard()
    {
        EventHanlder.CallCancelPlayTheCard();
    }
    #endregion


    private void PayCardCheck(CardDetail_SO data)
    {
        if (GameManager.instance.isPayCard)
        {
            if (payCards.transform.childCount >= data.payCardNum)
            {
                //TODO: Pay card complete
            }
        }
    }
}
