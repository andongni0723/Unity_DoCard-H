using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class BasicCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public CardDetail_SO cardDetail;
    public Vector4 halfPadding = new Vector4(0, 0, 90, 0); // half padding can let pointer easy Choose card
    public Vector4 zeroPadding = new Vector4(0, 0, 0, 0);  // zero padding can let pointer easy Drag card
    public Image image;

    public Transform originParent; // CardManager
    [SerializeField] public Transform playCardParent; // After play the card, will set parent to it
    public int id;
    public float yPos;
    float targetCardYPos = 540;
    float scale;

    bool isDrag = false;

    private void Awake()
    {
        id = transform.GetSiblingIndex();
        originParent = GameObject.FindWithTag("CardManager").transform; // CardManager
        playCardParent = GameObject.FindWithTag("PlayCardParent").transform;
        image = GetComponent<Image>();
        scale = transform.localScale.x;
        //transform.DOMove(new Vector2(transform.position.x + 200, transform.position.y), 1);
    }

    #region Event
    private void OnEnable()
    {
        EventHanlder.CardUpdatePosition += OnCardIDChange;
        EventHanlder.CardUpdatePosition += OnCardUpdatePosition;
        EventHanlder.EndDragCardUpdateData += OnEndDragCardUpdateData;
        EventHanlder.CancelPlayTheCard += OnCancelPlayTheCard;
        EventHanlder.PayTheCardError += OnPayTheCardError;
    }
    private void OnDisable()
    {
        EventHanlder.CardUpdatePosition -= OnCardIDChange;
        EventHanlder.CardUpdatePosition -= OnCardUpdatePosition;
        EventHanlder.EndDragCardUpdateData -= OnEndDragCardUpdateData;
        EventHanlder.CancelPlayTheCard -= OnCancelPlayTheCard;
        EventHanlder.PayTheCardError -= OnPayTheCardError;
    }

    private void OnCardIDChange()
    {
        id = transform.GetSiblingIndex();
    }

    private void OnCardUpdatePosition()
    {
        // The card maybe by play the card
        if (transform.parent != null)
        {
            // The card maybe by payCard parent
            if (transform.parent.TryGetComponent(out CardManager cardManager))
            {
                transform.DOMove(cardManager.CardPositionList[id], 0.5f).OnComplete
                (
                    () =>
                    {
                        yPos = transform.position.y;
                    }
                );
            }
        }


    }
    private CardDetail_SO OnEndDragCardUpdateData()
    {
        return cardDetail;
    }

    private void OnCancelPlayTheCard()
    {
        transform.DOScale(scale * 1f, 0.3f);
        transform.parent = GameObject.FindGameObjectWithTag("CardManager").transform;
        OnCardUpdatePosition();
        image.raycastPadding = halfPadding;
    }

    private void OnPayTheCardError(GameObject targetCard)
    {
        // This card haven't pay because the pay cards is enough
        if(transform.parent == null) 
        {
            transform.parent = originParent;
        }
    }

    #endregion 


    #region Pointer Event
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.instance.gameStep != GameStep.PayCardStep)
            transform.SetAsLastSibling(); // Let layer on first

        transform.DOScale(scale * 1.5f, 0.3f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDrag) return;

        //Show big card detail
        //TODO: The image will change to detail sprite "Card Detail Sprite" in future
        EventHanlder.CallCardOnClick(image.sprite);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.SetSiblingIndex(id); // Let layer become before
        transform.DOScale(scale * 1f, 0.3f);

        EventHanlder.CallCardOnClick(null);
    }

    #endregion

    #region Drag Event
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.parent != originParent) return; // Card is Paying

        isDrag = true;
        transform.DOScale(scale * 0.3f, 0.3f);
        image.raycastPadding = zeroPadding;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent != originParent) return; // Card is Paying

        ScaleAtCardOnDrag(eventData);
        EventHanlder.CallCardOnDrag(cardDetail);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent != originParent) return; // Card is Paying

        EventAtCardEndDrag(eventData);
    }


    #endregion

    /// <summary>
    /// Card on drag, if card yPos out a distance, scale will become small (play), or big (canel play)
    /// </summary>
    /// <param name="eventData"></param>
    public void ScaleAtCardOnDrag(PointerEventData eventData)
    {
        if (eventData.position.y > targetCardYPos) // Play the card
        {
            transform.DOScale(scale * 0.3f, 0.3f);
            transform.position = eventData.position;
        }
        else // Canel play the card
        {
            transform.DOScale(scale * 1f, 0.3f);
            transform.position = eventData.position;
        }
    }

    public void EventAtCardEndDrag(PointerEventData eventData)
    {
        // Canel play the card
        if (eventData.position.y < targetCardYPos)
        {
            transform.DOScale(scale * 1f, 0.3f);
            OnCardUpdatePosition();
            image.raycastPadding = halfPadding;
        }
        else
        {
            var lastPos = transform.position; // Let card Position not be different after change parent
            transform.parent = null;
            transform.position = lastPos;

            // Is play card to attack, OR want pay card to let other card attack
            if (GameManager.instance.gameStep == GameStep.PayCardStep)
            {         
                EventHanlder.CallPayTheCard(gameObject);
            }
            else
            {
                transform.parent = playCardParent;
                EventHanlder.CallCardEndDrag();
            }
        }
        isDrag = false;
    }

}
