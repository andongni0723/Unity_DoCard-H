using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardManager : Singleton<CardManager>
{
    public List<Vector2> CardPositionList = new List<Vector2>();
    //public List<CardDetail_SO> CardDetailPrefabList = new List<CardDetail_SO>();
    public CharacterCards_SO Cards;
    public GameObject cardPrefabs;
    public GameObject cardInstPoint;
    public GameObject discardPilePoint;

    public int stepStartPlayCardNum = 8;

    private float cardMoveX;
    private int instCardNameNum;

    [Header("Card Move Setting")]
    public float cardWidth = Screen.height / 7.55f; // 4KUHD  (286f)
    public float moveX;

    [Header("Card Soft Setting")]
    public int maxCardNum = 5;


    protected override void Awake()
    {
        base.Awake();
        cardWidth = Screen.height / 7.55f; // 4KUHD  (286f)
    }


    #region Event
    private void OnEnable()
    {
        EventHanlder.ChangeCardsOnStepStart += ChangeCardsOnStepStart; // Change Cards with arg
        EventHanlder.PlayerStepAddCard += OnPlayerStepAddCard; // Init the some card
        EventHanlder.PayCardComplete += OnPayCardComplete; // Init the card position
    }
    private void OnDisable()
    {
        EventHanlder.ChangeCardsOnStepStart += ChangeCardsOnStepStart;
        EventHanlder.PlayerStepAddCard -= OnPlayerStepAddCard;
        EventHanlder.PayCardComplete -= OnPayCardComplete;
    }

    private void ChangeCardsOnStepStart(CharacterCards_SO data)
    {
        Cards = data;
    }

    private void OnPlayerStepAddCard()
    {
        for (int i = 0; i < stepStartPlayCardNum; i++)
        {
            AddCardButton();
        }
    }

    private void OnPayCardComplete()
    {
        ChangeCardPosition(transform.childCount);
    }
    #endregion

    private void OnTransformChildrenChanged()
    {
        // If Children Change (add or remove)
        if (CardPositionList.Count != transform.childCount)
            ChangeCardPosition(transform.childCount);
    }

    /// <summary>
    /// Change card position 
    /// </summary>
    /// <param name="_childNum"></param>
    private void ChangeCardPosition(int _childNum)
    {
        // Init
        CardPositionList.Clear();

        cardMoveX = cardWidth + moveX;

        if (_childNum > maxCardNum)
        {
            cardMoveX = cardMoveX / (1f + (_childNum - maxCardNum) / (maxCardNum - 1));
        }

        // If children count is even number, 
        // the card needs to move some right to keep cards is on center
        int odd = 1;
        odd = (_childNum % 2 == 0) ? 1 : 0;

        // the xPos of the leftest card
        float leftX = -(cardMoveX * (int)(_childNum / 2)) + cardMoveX / 2 * odd;


        for (int i = 0; i < _childNum; i++)
        {
            // Add Position to List
            CardPositionList.Add(new Vector2(transform.position.x + leftX + cardMoveX * i, transform.position.y));
        }

        EventHanlder.CallCardUpdeatePosition();
    }


    /// <summary>
    /// Give the CardDetail to inst the card
    /// </summary>
    /// <param name="data">CardDetail_SO</param>
    public void AddCard(CardDetail_SO data)
    {
        // Check the cardType of detail to change the card baclgroud image
        Sprite cardSprite = GameManager.Instance.CardTypeToCardBackgroud(data);

        // Instantiate the card prefabs
        var cardObj = Instantiate(cardPrefabs, cardInstPoint.transform.position, Quaternion.identity, this.transform) as GameObject;

        // Set new object var
        cardObj.name = $"Card{instCardNameNum}";
        cardObj.GetComponent<BasicCard>().cardDetail = data;
        cardObj.GetComponent<Image>().sprite = cardSprite;
        instCardNameNum++;

        // Call the method let card update data
        cardObj.GetComponent<BasicCard>().AfterInit();
    }


    public void AddCardButton()
    {
        // Random the cardDetail
        CardDetail_SO cardDetail = Cards.Cards[Random.Range(0, Cards.Cards.Count)];

        AddCard(cardDetail);
    }
}
