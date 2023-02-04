using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowCardDetail : MonoBehaviour
{
    Image image;
    
    [Header("Children")]
    public GameObject cardNameObj;
    public GameObject cardPayNumTextObj;
    public GameObject cardImgObj;
    public GameObject cardDescriptionObj;


    void Start()
    {
        image = GetComponent<Image>();
        
        INIT();
    }

    private void INIT()
    {
        image.enabled = false;
        image.sprite = null;

        cardNameObj.SetActive(false);
        cardPayNumTextObj.SetActive(false);
        cardImgObj.SetActive(false);
        cardDescriptionObj.SetActive(false);

    }
    
    /// <summary>
    /// Set gameObject and children active visible, and input CardDetail_SO to children to show
    /// </summary>
    /// <param name="data">CardDetail_SO</param>
    private void ShowDetail(CardDetail_SO data)
    {
        // Set Active
        image.enabled = true;
        cardNameObj.SetActive(true);
        cardPayNumTextObj.SetActive(true);
        cardImgObj.SetActive(true);
        cardDescriptionObj.SetActive(true);

        // Updata card details
        image.sprite = GameManager.Instance.CardTypeToCardBackgroud(data);
        cardNameObj.GetComponent<TextMeshProUGUI>().text = data.cardName;
        cardPayNumTextObj.GetComponent<TextMeshProUGUI>().text = data.payCardNum.ToString();
        cardImgObj.GetComponent<Image>().sprite = data.cardSkillSprite;
        cardDescriptionObj.GetComponent<TextMeshProUGUI>().text = data.cardDestription;
    }

    #region Event
    private void OnEnable()
    {
        EventHanlder.CardOnClick += OnCardOnClick;
    }

    private void OnDisable()
    {
        EventHanlder.CardOnClick -= OnCardOnClick;
    }

    private void OnCardOnClick(CardDetail_SO data)
    {
        // Show big card Details
        if(data != null)
        {
            ShowDetail(data);
        }
        else
        {
            INIT();
        }
    }
    #endregion
}
