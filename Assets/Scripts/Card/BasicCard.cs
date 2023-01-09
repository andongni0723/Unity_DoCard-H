using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BasicCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public CardDetail_SO cardDetail;
    public Vector4 halfPadding = new Vector4(0, 0, 90, 0); // half padding can let pointer easy Choose card
    public Vector4 zeroPadding = new Vector4(0, 0, 0, 0);  // zero padding can let pointer easy Drag card
    public Image image;
    public int id;
    public float yPos;
    float targetCardYPos = 540;
    float scale;

    bool isDrag = false;

    private void Awake()
    {
        id = transform.GetSiblingIndex();
        image = GetComponent<Image>();
        scale = transform.localScale.x;
        //transform.DOMove(new Vector2(transform.position.x + 200, transform.position.y), 1);
    }

    private void OnEnable()
    {
        EventHanlder.CardUpdatePosition += CardIDChange;
        EventHanlder.CardUpdatePosition += OnCardUpdatePosition;
    }
    private void OnDisable()
    {
        EventHanlder.CardUpdatePosition -= CardIDChange;
        EventHanlder.CardUpdatePosition -= OnCardUpdatePosition;

    }

    private void CardIDChange()
    {
        id = transform.GetSiblingIndex();
    }

    private void OnCardUpdatePosition()
    {
        var CardManager = transform.parent.GetComponent<CardManager>();

        transform.DOMove(CardManager.CardPositionList[id], 0.5f).OnComplete
        (
            () =>
            {
                yPos = transform.position.y;
            }
        );
        //Debug.Log("set" + id);
    }



    #region Pointer Event
    public void OnPointerEnter(PointerEventData eventData)
    {
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
        isDrag = true;
        transform.DOScale(scale * 0.3f, 0.3f);
        image.raycastPadding = zeroPadding;
    }
    public void OnDrag(PointerEventData eventData)
    {
        ScaleAtCardOnDrag(eventData);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        //Event of Play a card or Canel play a card
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
        if (eventData.position.y > targetCardYPos) // Play the card
        {
            // Play the card
            transform.DOScale(scale * 0f, 0.3f);
            Destroy(gameObject);

            EventHanlder.CallPlayTheCard(cardDetail);
        }
        else // Canel play the card
        {
            transform.DOScale(scale * 1f, 0.3f);
            OnCardUpdatePosition();
            image.raycastPadding = halfPadding;
        }

        isDrag = false;
    }

}
