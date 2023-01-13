using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPayPanel : MonoBehaviour
{
    [Header("UI Object")]
    public GameObject panel;
    public GameObject payCards;
    public GameObject confirmButton;
    public TextMeshProUGUI mainText;

    CardDetail_SO cardData;

    bool isPayEnough; //FIXME:
    private void Awake()
    {
        panel = transform.GetChild(0).gameObject;
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

        PayCardTextCheck();
    }

    private void OnPayTheCard(GameObject card)
    {
        PayCardCheck(cardData, card);
        PayCardIsDoneCheck(cardData, card);
        PayCardTextCheck();
    }

    public void CancelPlayTheCard() // Cancel Button Event
    {
        confirmButton.SetActive(false);
        EventHanlder.CallCancelPlayTheCard();
    }
    #endregion

    
    /// <summary>
    /// If pay cards num NOT enough, to let card parent add in 'payCardsTable'
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cardObj"></param>
    private void PayCardCheck(CardDetail_SO data, GameObject cardObj)
    {
        if (GameManager.instance.gameStep == GameStep.PayCardStep)
        {
            if(payCards.transform.childCount != data.payCardNum) // not complete
            {
                cardObj.transform.parent = payCards.transform;
            }
        }
    }

    /// <summary>
    /// If pay card num enough AND player want pay card, let card put to player hand 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cardObj"></param>
    private void PayCardIsDoneCheck(CardDetail_SO data, GameObject cardObj)
    {
        if (GameManager.instance.gameStep == GameStep.PayCardStep)
        {
            if (payCards.transform.childCount == data.payCardNum) //pay cards enough
            {
                //Pay card complete
                confirmButton.SetActive(true);
                EventHanlder.CallPayTheCardError(cardObj);
            }
        }
    }

    private void PayCardTextCheck()
    {
        int needCardNum = cardData.payCardNum - payCards.transform.childCount;
        mainText.text = $"卡牌召喚還需要{needCardNum}張卡";
    }
}
