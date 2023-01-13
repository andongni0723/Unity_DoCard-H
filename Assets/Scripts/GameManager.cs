using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameStep gameStep;

    [Header("Game Data")]
    public List<ConfirmAreaGridData> SkillHurtGridList = new List<ConfirmAreaGridData>();

    // If area grid of card is corrent, the data will put in 'temporaryData'.
    // If pay card confirm, the data will put to skill arealist
    public ConfirmAreaGridData temporaryData;

    [Header("Test")]
    public TextMeshProUGUI gameStepText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        //FIXME: 未來
        gameStep = GameStep.CommonStep;
        gameStepText.text = "CommonStep";
    }

    #region Event
    private void OnEnable()
    {
        EventHanlder.EndDragCofirmData += OnEndDragCofirmData;
        EventHanlder.GameStepChange += OnPlayTheCardChangeGameStep;
        EventHanlder.CancelPlayTheCard += OnCancelPlayTheCard;
        EventHanlder.PayCardComplete += OnPayCardComplete;
    }
    private void OnDisable()
    {
        EventHanlder.EndDragCofirmData -= OnEndDragCofirmData;
        EventHanlder.GameStepChange -= OnPlayTheCardChangeGameStep;
        EventHanlder.CancelPlayTheCard -= OnCancelPlayTheCard;
        EventHanlder.PayCardComplete -= OnPayCardComplete;

    }



    private void OnEndDragCofirmData(ConfirmAreaGridData data)
    {
        int gridCount = data.ConfirmGridsList.Count;

        if (gridCount == data.cardDetail.cardOffset.x * data.cardDetail.cardOffset.y) // the confirm area count is right
        {
            //TODO: card pay UI
            Debug.Log("The confirm area count is right");
            temporaryData = data;
            EventHanlder.CallPlayTheCard(data.cardDetail);
        }
        else // the confirm area count isn't right
        {
            EventHanlder.CallCancelPlayTheCard();
        }
    }

    private void OnPlayTheCardChangeGameStep()
    {
        gameStep = GameStep.PayCardStep;
        gameStepText.text = "PayCardStep";
    }
    private void OnCancelPlayTheCard()
    {
        temporaryData = null; // Init 
        gameStep = GameStep.CommonStep;
        gameStepText.text = "CommonStep";
    }

    private void OnPayCardComplete()
    {
        Debug.Log("did");
        gameStep = GameStep.CommonStep;
        gameStepText.text = "CommonStep";

        SkillHurtGridList.Add(temporaryData); //TODO: enemy
    }
    #endregion
}
