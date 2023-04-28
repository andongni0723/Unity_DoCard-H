using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.Serialization;

public class BasicCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("Children Component")]
    public GameObject card;

    public Image cardBGImage;
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardPayNumText;
    public Image cardImg;
    public Image cardColdDownMask;
    public TextMeshProUGUI cardDescriptionText;

    [Header("Card Setting")]
    public Vector4 halfPadding = new Vector4(0, 0, 90, 0); // half padding can let pointer easy Choose card
    public Vector4 zeroPadding = new Vector4(0, 0, 0, 0);  // zero padding can let pointer easy Drag card

    [Header("Test Data Show")]
    [SerializeField] Image image;
    [SerializeField] Transform originParent; // CardManager
    [SerializeField] Transform playCardParent; // After play the card, will set parent to it

    [Header("Card Data")]
    public CardDetail_SO cardDetail;
    public int id;
    public float yPos;

    float CardLinePosY = Screen.height / 4; // 4KUHD (540f)
    float scale;

    [SerializeField] bool isDrag = false;
    [SerializeField] bool cantUse = false;

    private void Awake()
    {

        // Get children gameObject
        //card = transform.Find("Card").gameObject;
        //cardNameText = card.transform.Find("card_name").GetComponent<TextMeshProUGUI>();
        //cardPayNumText = card.transform.Find("card_pay").GetComponent<TextMeshProUGUI>();
        //cardImg = card.transform.Find("card_img").GetComponent<Image>();
        //cardDescriptionText = card.transform.Find("card_description").GetComponent<TextMeshProUGUI>();
        //cardColdDownMask = card.transform.Find("ColdDown Mask").GetComponent<Image>();

        // Get and Set self var
        id = transform.GetSiblingIndex();
        originParent = GameObject.FindWithTag("CardManager").transform; // CardManager
        playCardParent = GameObject.FindWithTag("PlayCardParent").transform;
        image = GetComponent<Image>();
        scale = card.transform.localScale.x;
        CardLinePosY = Screen.height / 4; // 4KUHD (540f)

        image.raycastPadding = halfPadding;
    }

    private void Update()
    {
        //&& GameManager.Instance.gameStep == GameStep.CommonStep
        if (transform.parent == originParent && isDrag && !cantUse)
        {
            // Fix card move anim problem
            transform.position = Input.mousePosition;

            ScaleAndPositionAtCardOnDrag(transform.position); //FIXME
        }
    }
    
    /// <summary>
    /// This method be call when cardManager instantiate card object
    /// </summary>
    public void CardInit(CardDetail_SO data, Sprite cardBg)
    {
        cardDetail = data;
        cardBGImage.sprite = cardBg;
        
        // After Setting Done
        cardNameText.text = cardDetail.cardName;
        cardPayNumText.text = cardDetail.payCardNum.ToString();
        cardImg.sprite = cardDetail.cardSkillSprite;
        cardDescriptionText.text = cardDetail.cardDestription;

        // Final Skill card setting cold down mask
        if (cardDetail.isFinalSkill)
        {
            if (GameManager.Instance.currentCharacter == Character.Player)
                cardColdDownMask.fillAmount = 
                    GameManager.Instance.playerFinalSkillColdDown == 0 ? 0 : (float)GameManager.Instance.playerFinalSkillColdDown / cardDetail.cardColdDown;
            else
                cardColdDownMask.fillAmount = 
                    GameManager.Instance.enemyFinalSkillColdDown == 0 ? 0 : (float)GameManager.Instance.enemyFinalSkillColdDown / cardDetail.cardColdDown;
        }
    }

    #region Event
    private void OnEnable()
    {
        EventHanlder.CardUpdatePosition += OnCardIDChange;
        EventHanlder.CardUpdatePosition += OnCardUpdatePosition; // Update Position
        EventHanlder.CancelPlayTheCard += OnCancelPlayTheCard; // Back to CardManager, and INIT
        EventHanlder.PayTheCardError += OnPayTheCardError; // Cancel pay card, back to CardManager
        EventHanlder.PayCardComplete += OnPayCardComplete; // Destroy card which paid OR played
        EventHanlder.CommandStepEnd += OnCommandStepEnd; // Card Move to Discard pile
    }
    private void OnDisable()
    {
        EventHanlder.CardUpdatePosition -= OnCardIDChange;
        EventHanlder.CardUpdatePosition -= OnCardUpdatePosition;
        EventHanlder.CancelPlayTheCard -= OnCancelPlayTheCard;
        EventHanlder.PayTheCardError -= OnPayTheCardError;
        EventHanlder.PayCardComplete -= OnPayCardComplete;
        EventHanlder.CommandStepEnd -= OnCommandStepEnd;
    }

    private void OnCardIDChange()
    {
        id = transform.GetSiblingIndex();
    }

    private void OnCardUpdatePosition()
    {
        // The card maybe by the card of played
        if (transform.parent == originParent)
        {
            transform.DOMove(GetComponentInParent<CardManager>().CardPositionList[id], 0.5f).OnComplete
            (
                () =>
                {
                    yPos = transform.position.y;
                }
            );
            transform.rotation = GetComponentInParent<CardManager>().CardRotationList[id];
            
            // Card Object
            card.transform.position = transform.position;
        }
    }

    private void OnCancelPlayTheCard()
    {
        card.transform.DOScale(scale * 1f, 0.3f);
        transform.parent = originParent;
        //EventHanlder.CallCardUpdeatePosition();

        OnCardUpdatePosition();
        image.raycastPadding = halfPadding; //FIXME: padding
    }

    private void OnPayTheCardError(GameObject targetCard)
    {
        // This card haven't pay because the pay cards is enough
        if (gameObject == targetCard)
        {
            transform.parent = originParent;
            Debug.Log("OnPayTheCardError");
            //OnCancelPlayTheCard();
        }

        // if (transform.parent == null)
        // {
        //     transform.parent = originParent;
        // }
        // else
        // {
        //     //OnCancelPlayTheCard();
        //     //OnCardUpdatePosition(); FIXME
        // }
    }

    private void OnPayCardComplete()
    {
        // Destroy card which paid OR played
        if (transform.parent != originParent)
        {
            Destroy(gameObject);

            if (transform.parent == playCardParent)
            {
                Instantiate(CardManager.Instance.discardAnimationPrefabs, transform.position, Quaternion.identity, transform.parent);
            }
        }

    }


    private void OnCommandStepEnd()
    {
        // animation move to discard pile
        cantUse = true;
        Vector3 beforePos = transform.position;
        Vector3 discardPoint = CardManager.Instance.discardPilePoint.transform.position;
        //transform.parent = null; // FIXM

        transform.position = beforePos; // Let position set same after change parent

        Sequence mainSequence = DOTween.Sequence();
        mainSequence.Append(transform.DOMove(discardPoint, 0.5f));
        mainSequence.OnComplete(() => Destroy(gameObject));
    }

    #endregion 

    #region Pointer Event
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (!eventData.fullyExited) return; // Unity bug "IPointerEnter/Exit"

        if (cantUse) return;

        // Let card can't move when paing
        if (GameManager.Instance.gameStep != GameStep.PayCardStep)
            transform.SetAsLastSibling(); // Let layer on first

        card.transform.DOScale(scale * 1.5f, 0.3f);
        //Debug.Log("enter");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDrag || cantUse) return;

        //Show big card detail
        EventHanlder.CallCardOnClick(cardDetail);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //if (!eventData.fullyExited) return; // Unity bug "IPointerEnter/Exit"
        transform.SetSiblingIndex(id); // Let layer become before
        card.transform.DOScale(scale * 1f, 0.3f);

        EventHanlder.CallCardOnClick(null);

        //Debug.Log("exit");
    }

    #endregion

    #region Drag Event
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.parent != originParent || isDrag || cantUse) return; // Card was Paid

        isDrag = true;
        
        image.raycastPadding = zeroPadding;  //FIXME: padding
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent != originParent || cantUse) return; // Card was Paid

        //ScaleAtCardOnDrag(eventData.position); FIXME
        EventHanlder.CallCardOnDrag(cardDetail);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent != originParent || cantUse) return; // Card was Paid

        EventAtCardEndDrag(eventData);
    }
    #endregion

    /// <summary>
    /// Card on drag, if card yPos out a distance, scale will become small (play), or big (canel play)
    /// </summary>
    /// <param name="pos"></param>
    private void ScaleAndPositionAtCardOnDrag(Vector3 pos)
    {
        if (pos.y > CardLinePosY)
        {
            // Play the card
            card.transform.DOScale(scale * 0.5f, 0.2f);
            card.transform.position = transform.position;
        }
        else
        {
            // Back to Hand
            card.transform.DOScale(scale * 2f, 0.2f);
            card.transform.position = transform.position + new Vector3(0, Screen.height / 2.7f, 0);
        }
    }

    public void EventAtCardEndDrag(PointerEventData eventData)
    {
        cantUse = true;

        // Check the card PosY
        if (eventData.position.y < CardLinePosY)
        {
            // Cancel play the card
            card.transform.position = transform.position;

            if (GameManager.Instance.gameStep == GameStep.PayCardStep)
            {
                Debug.Log("noYYYYYYy");
                EventHanlder.CallPayTheCardError(gameObject);
            }
            else
            {
                EventHanlder.CallCancelPlayTheCard();
            }
        }
        else // Play the card
        {
            // Let card Position not be different after change parent
            var lastPos = transform.position;
            transform.parent = null;
            transform.position = lastPos;
            transform.rotation = Quaternion.Euler(0, 0, 0);

            SoundManager.Instance.PlaySound(SoundManager.Instance.playCardSE); // SE

            // Is play card to attack, OR want pay card to let other card attack
            if (GameManager.Instance.gameStep == GameStep.PayCardStep)
            {
                EventHanlder.CallPayTheCard(gameObject);
            }
            else
            {
                // Play card
                transform.parent = playCardParent;
                EventHanlder.CallCardEndDrag();
            }
        }

        isDrag = false;
        cantUse = false;
    }

}
