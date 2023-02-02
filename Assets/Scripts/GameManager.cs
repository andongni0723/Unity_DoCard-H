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

        //StartCoroutine(ExecuteCardActionList2(CommonCardActionList));
        StartCoroutine(LoopActionList());

    }
    IEnumerator LoopActionList()
    {
        while (true)
        {
            switch (gameStep)
            {
                case GameStep.CommonStep:
                    if (CommonCardActionList.Count != 0)
                        //ExecuteCardActionList(CommonCardActionList);

                        //FIXME: the action will be infinally loop
                        yield return StartCoroutine(ExecuteCardActionList(CommonCardActionList));
                    break;

                case GameStep.PlayerSettlement:
                    if (PlayerSettlementCardActionList.Count != 0)
                    {
                        yield return StartCoroutine(ExecuteCardActionList(PlayerSettlementCardActionList));
                    }
                    else
                    {
                        ChangeGameStep(GameStep.CommonStep); //TODO: future
                    }
                    break;

                case GameStep.EnemySettlement:
                    //TODO: future to enemy 
                    ChangeGameStep(GameStep.PlayerSettlement);
                    break;
            }
            yield return null;
        }
    }

    private void Update()
    {

    }



    /// <summary>
    /// Change GameStep to args
    /// </summary>
    public void ChangeGameStep(GameStep _toChange)
    {
        gameStep = _toChange;
        gameStepText.text = _toChange.ToString();
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
        //Debug.Log("Before");
        AddAllConfirmGrid();

        EventHanlder.CallSendGameMessage("卡牌使用成功");
    }
    #endregion

    private void AddAllConfirmGrid()
    {
        //SkillHurtGridListPlayerSettlementCardAction
        // |- ConfirmAreaGridData
        //      |- ConfirmGridList
        //          |- ConfirmGrid <- Need
        //          |- ...

        AllConfirmGridList.Clear();
        foreach (ConfirmAreaGridData data in PlayerSettlementCardActionList)
        {
            foreach (ConfirmGrid grid in data.ConfirmGridsList)
            {
                //Debug.Log("AAAAA");//FIXM

                AllConfirmGridList.Add(grid);
            }
        }

        // Reload when function done
        EventHanlder.CallReloadGridColor(AllConfirmGridList); // To GridManager reload grid color
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

    public void PlayerDonePlayButton()
    {
        ChangeGameStep(GameStep.EnemySettlement);
    }

    /// <summary>
    /// Execute CardActionList by step
    /// </summary>
    /// <param name="actionList">CardActionList</param>
    IEnumerator ExecuteCardActionList(List<ConfirmAreaGridData> actionList)
    {
        WaitForSeconds wait;

        // If don't setting wait time, this method will error 
        // (can't add item in list when coroutine not done )
        if(actionList == CommonCardActionList)
            wait = new WaitForSeconds(0);
        else
            wait = new WaitForSeconds(2);


        // MAIN
        foreach (ConfirmAreaGridData skill in actionList)
        {
            // Different cardType to do different thing
            switch (skill.cardDetail.cardType)
            {
                case CardType.Attack:
                    // Call Grid of in skill area
                    foreach (ConfirmGrid grid in skill.ConfirmGridsList)
                    {
                        //Debug.Log("BBBBB");// FIXM

                        // Call grid to play animation and check character health
                        EventHanlder.CallAttackGrid(gameStep, grid);


                        // Remove the grid of now executing
                        foreach (ConfirmGrid confirmGrid in AllConfirmGridList)
                        {
                            if (confirmGrid == grid)
                            {
                                AllConfirmGridList.Remove(confirmGrid);
                                break;
                            }
                        }

                        // To GridManager reload grid color
                        EventHanlder.CallReloadGridColor(AllConfirmGridList);
                    }

                    yield return wait;
                    break;

                case CardType.Move:
                    EventHanlder.CallMoveAction(skill);
                    break;

                case CardType.Tank:
                    playerArmor += skill.cardDetail.tankTypeDetails.addArmor;
                    playerHealth += skill.cardDetail.tankTypeDetails.addHealth;
                    EventHanlder.CallArmorChange();
                    EventHanlder.CallHealthChange();
                    break;
            }

        }

        actionList.Clear();
        //yield return null;
    }
}
