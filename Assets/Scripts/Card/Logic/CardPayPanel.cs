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


    private void Update()
    {
        // Reload 'payCards' horizontal layout group children position
        if(payCards.transform.childCount != 0)
        {
            payCards.transform.GetChild(0).gameObject.SetActive(false);
            payCards.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void OnPlayTheCard(CardDetail_SO data)
    {
        // If The card don't pay any card to play
        if (data.payCardNum == 0)
        {
            PayCardComplete();
        }
        else
        {
            panel.SetActive(true);
            cardData = data;

            PayCardTextCheck();
        }
    }

    private void OnPayTheCard(GameObject card)
    {
        PayCardAddParentCheck(cardData, card);
        PayCardIsDoneCheck(cardData, card);
        PayCardTextCheck();
    }
    #endregion


    /// <summary>
    /// If pay cards num enough, to execute this function
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cardObj"></param>
    private void PayCardIsDoneCheck(CardDetail_SO data, GameObject cardObj)
    {
        if (GameManager.Instance.gameStep == GameStep.PayCardStep)
        {
            if (payCards.transform.childCount == data.payCardNum)
            {
                // PayCards is Enough
                confirmButton.SetActive(true);
                //Debug.Log($"PayCardIsDoneCheck , {payCards.transform.childCount} == {data.payCardNum}"); //FIXM
            }
        }
    }

    /// <summary>
    /// If pay card num enough AND player want pay card, let card put to player hand 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="cardObj"></param>
    private void PayCardAddParentCheck(CardDetail_SO data, GameObject cardObj)
    {
        // Timeline
        /// Check ----->| = |---> PayTheCardError
        ///       |---->| < |---> Add Parent

        if (GameManager.Instance.gameStep == GameStep.PayCardStep)
        {
            if (payCards.transform.childCount == data.payCardNum) //pay cards enough
            {
                //payCards is more than data payCardNum
                EventHanlder.CallPayTheCardError(cardObj);
            }
            else
            {
                // no complete
                cardObj.transform.parent = payCards.transform;
                cardObj.transform.position = payCards.transform.position;
            }
        }
    }
    private void PayCardTextCheck()
    {
        int needCardNum = cardData.payCardNum - payCards.transform.childCount;
        mainText.text = $"卡牌召喚還需要{needCardNum}張卡";
    }

    #region Button Event
    public void CancelPlayTheCard() // Cancel Button Event
    {
        confirmButton.SetActive(false);
        EventHanlder.CallCancelPlayTheCard();
    }

    public void PayCardComplete()
    {
        EventHanlder.CallPayCardComplete();
    }
    #endregion
}
