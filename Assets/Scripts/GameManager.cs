using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public GameStep gameStep;

    [Header("Game Data")]
    public List<ConfirmAreaGridData> PlayerSettlementCardActionList = new List<ConfirmAreaGridData>();
    public List<ConfirmAreaGridData> CommonCardActionList = new List<ConfirmAreaGridData>();
    public List<ConfirmGrid> AllConfirmGridList = new List<ConfirmGrid>();
    public CardDetail_SO playingCard;

    // If area grid of card is corrent, the data will put in 'temporaryData'.
    // If pay card confirm, the data will put to skill arealist
    ConfirmAreaGridData temporaryData;

    public int playerHealth = 10;
    public int playerArmor = 0;

    public int enemyHealth = 10;
    public int enemyArmor = 0;

    [Header("Card Prefab Assets")]
    public Sprite cardAttackSprite;
    public Sprite cardTankSprite;
    public Sprite cardMoveSprite;
    public Sprite cardFinalSkillSprite;

    [Header("Test")]
    public TextMeshProUGUI gameStepText;

    protected override void Awake()
    {
        base.Awake();

        //FIXME: 未來
        gameStep = GameStep.CommonStep;
        gameStepText.text = "CommonStep";
    }

    private void Update()
    {
        switch (gameStep)
        {
            case GameStep.CommonStep:
                ExecuteCardActionList(CommonCardActionList);
                break;

            case GameStep.PlayerSettlement:
                ExecuteCardActionList(PlayerSettlementCardActionList);
                break;
        }
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
        EventHanlder.CancelPlayTheCard += OnCancelPlayTheCard; // change gameStep to CommonStep
        EventHanlder.PayCardComplete += OnPayCardComplete; // change gameStep, and call event to change grid color

    }
    private void OnDisable()
    {
        EventHanlder.CardOnDrag -= OnCardOnDrag;
        EventHanlder.CardEndDrag -= OnCardEndDrag;
        EventHanlder.EndDragCofirmData -= OnEndDragCofirmData;
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

        switch (data.cardDetail.cardType)
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
            EventHanlder.CallSendGameMessage("請確認技能釋放範圍完整");
            EventHanlder.CallCancelPlayTheCard();
        }
    }

    private void OnPlayTheCardChangeGameStep()
    {
        ChangeGameStep(GameStep.PayCardStep);
        gameStepText.text = "PayCardStep";
    }
    private void OnCancelPlayTheCard()
    {
        temporaryData = null; // Init 
        ChangeGameStep(GameStep.CommonStep);
        gameStepText.text = "CommonStep";
    }

    private void OnPayCardComplete()
    {
        ChangeGameStep(GameStep.CommonStep);
        gameStepText.text = "CommonStep";

        switch (temporaryData.cardDetail.cardUseGameStep)
        {
            case GameStep.CommonStep:
                CommonCardActionList.Add(temporaryData);
                break;

            case GameStep.PlayerSettlement:               
                PlayerSettlementCardActionList.Add(temporaryData);//TODO: enemy
                break;
        }
        AddAllConfirmGrid();
        EventHanlder.CallReloadGridColor(AllConfirmGridList); // To GridManager reload grid color
        EventHanlder.CallSendGameMessage("卡牌使用成功");
    }
    #endregion

    private void AddAllConfirmGrid()
    {
        //SkillHurtGridListPlayerSettlementCardAction
        // |- ConfirmAreaGridData
        //      |- ConfirmGridList
        //          |- ConfirmGrid <= Need
        //          |- ...

        foreach (ConfirmAreaGridData data in PlayerSettlementCardActionList)
        {
            foreach (ConfirmGrid grid in data.ConfirmGridsList)
            {
                AllConfirmGridList.Add(grid);
            }
        }
    }

    public Sprite CardTypeToCardBackgroud(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.Attack:
                return cardAttackSprite;

            case CardType.Move:
                return cardMoveSprite;

            case CardType.Tank:
                return cardTankSprite;

        }

        return null;
    }

    private void ExecuteCardActionList(List<ConfirmAreaGridData> actionList)
    {
        foreach (ConfirmAreaGridData skill in actionList)
        {
            //TODO:
            switch (skill.cardDetail.cardType)
            {
                case CardType.Attack:

                    break;

                case CardType.Move:
                    EventHanlder.CallMoveAction(skill);
                    break;

                case CardType.Tank:
                    playerArmor += skill.cardDetail.tankTypeDetails.addArmor;
                    playerHealth += skill.cardDetail.tankTypeDetails.addHealth;
                    EventHanlder.CallHealthChange();
                    EventHanlder.CallArmorChange();
                    break;
            }
        }

        actionList.Clear();
    }
}
