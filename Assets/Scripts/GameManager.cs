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
    public List<ConfirmGrid> AllConfirmGridList = new List<ConfirmGrid>();
    public CardDetail_SO playingCard;

    // If area grid of card is corrent, the data will put in 'temporaryData'.
    // If pay card confirm, the data will put to skill arealist
    ConfirmAreaGridData temporaryData;

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

    /// <summary>
    /// Change GameStep to args
    /// </summary>
    public void ChangeGameStep(GameStep _toChange)
    {
        gameStep = _toChange;
    }

    #region Event
    private void OnEnable()
    {
        EventHanlder.CardOnDrag += OnCardOnDrag; // Set gameData var
        EventHanlder.CardEndDrag += OnCardEndDrag; // Set gameData var
        EventHanlder.EndDragCofirmData += OnEndDragCofirmData; // Check skill confirm area is corrent
        //EventHanlder.GameStepChange += OnPlayTheCardChangeGameStep; // change gameStep to PayCardStep
        EventHanlder.CancelPlayTheCard += OnCancelPlayTheCard; // change gameStep to CommonStep
        EventHanlder.PayCardComplete += OnPayCardComplete; // change gameStep, and call event to change grid color

    }
    private void OnDisable()
    {
        EventHanlder.CardOnDrag -= OnCardOnDrag;
        EventHanlder.CardEndDrag -= OnCardEndDrag;
        EventHanlder.EndDragCofirmData -= OnEndDragCofirmData;
        //EventHanlder.GameStepChange -= OnPlayTheCardChangeGameStep;
        EventHanlder.CancelPlayTheCard -= OnCancelPlayTheCard;
        EventHanlder.PayCardComplete -= OnPayCardComplete;
    }

    private void OnCardOnDrag(CardDetail_SO data)
    {
        playingCard = data;
    }
    private void OnCardEndDrag()
    {
        playingCard = null;
    }


    private void OnEndDragCofirmData(ConfirmAreaGridData data)
    {
        int gridCount = data.ConfirmGridsList.Count;
        int checkGridCount = 0;

        switch(data.cardDetail.cardType)
        {
            case CardType.Attack:
                checkGridCount = (int)(data.cardDetail.attackTypeDetails.cardAttackOffset.x * data.cardDetail.attackTypeDetails.cardAttackOffset.y);
                break;

            case CardType.Move:
                checkGridCount = (int)(data.cardDetail.moveTypeDetails.cardMoveOffset.x * data.cardDetail.moveTypeDetails.cardMoveOffset.y);
                break;

            case CardType.Tank:
                checkGridCount = 0;
                break;        
        }

        if (gridCount == checkGridCount) // the confirm area count is right
        {
            //card pay UI
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
        gameStep = GameStep.CommonStep;
        gameStepText.text = "CommonStep";

        SkillHurtGridList.Add(temporaryData); //TODO: enemy
        AddAllConfirmGrid();
        EventHanlder.CallReloadGridColor(AllConfirmGridList); // To GridManager reload grid color
    }
    #endregion

    private void AddAllConfirmGrid()
    {
        //SkillHurtGridList
        // |- ConfirmAreaGridData
        //      |- ConfirmGridList
        //          |- ConfirmGrid <= Need
        //          |- ...
        
        foreach (ConfirmAreaGridData data in SkillHurtGridList)
        {
            foreach (ConfirmGrid grid in data.ConfirmGridsList)
            {
                AllConfirmGridList.Add(grid);
            }
        }
    }
}
