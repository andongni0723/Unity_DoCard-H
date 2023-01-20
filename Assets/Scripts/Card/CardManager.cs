using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    public List<Vector2> CardPositionList = new List<Vector2>();
    public List<CardDetail_SO> CardDetailPrefabList = new List<CardDetail_SO>();
    public List<GameObject> CardPrefabList = new List<GameObject>();
    public GameObject cardPrefabs;
    public GameObject cardInstPoint;

    [Header("Card Prefab Assets")]
    public Sprite cardAttackSprite;
    public Sprite cardTankSprite;
    public Sprite cardMoveSprite;

    private float cardMoveX;

    [Header("Card Move Setting")]
    public float cardWidth;
    public float moveX;

    [Header("Card Soft Setting")]
    public int maxCardNum = 5;

    #region Event
    private void OnEnable()
    {
        EventHanlder.PayCardComplete += OnPayCardComplete;
    }
    private void OnDisable()
    {
        EventHanlder.PayCardComplete -= OnPayCardComplete;

    }

    private void OnPayCardComplete()
    {
        ChangeCardPosition(transform.childCount);
    }
    #endregion

    public void ChangeCardPosition(int _childNum)
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



    public void AddCard()
    {
        // Random the cardDetail
        CardDetail_SO cardDetail = CardDetailPrefabList[Random.Range(0, CardDetailPrefabList.Count)];
        Sprite cardSprite = null;

        // Check the cardType of detail to change the card image
        switch (cardDetail.cardType)
        {
            case CardType.Attack:
                cardSprite = cardAttackSprite;
                break;

            case CardType.Move:
                cardSprite = cardMoveSprite;
                break;

            case CardType.Tank:
                cardSprite = cardTankSprite;
                break;
        }

        // Inst. the card prefabs
        var cardObj = Instantiate(cardPrefabs, cardInstPoint.transform.position, Quaternion.identity, this.transform) as GameObject;

        // Set new object var
        cardObj.GetComponent<BasicCard>().cardDetail = cardDetail;
        cardObj.GetComponent<Image>().sprite = cardSprite;
    }

    private void OnTransformChildrenChanged()
    {
        // If Children Change (add or remove)
        if (CardPositionList.Count != transform.childCount)
            ChangeCardPosition(transform.childCount);
    }

}
