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
    public Character currentCharacter;
    public List<ConfirmAreaGridData> PlayerSettlementCardActionList = new List<ConfirmAreaGridData>();
    public List<ConfirmAreaGridData> EnemySettlementCardActionList = new List<ConfirmAreaGridData>();
    public List<ConfirmAreaGridData> CommonCardActionList = new List<ConfirmAreaGridData>();
    public List<ConfirmGrid> AllConfirmGridList = new List<ConfirmGrid>();
    public CardDetail_SO playingCard;

    public int playerHealth = 10;
    public int playerArmor = 0;

    public int enemyHealth = 10;
    public int enemyArmor = 0;

    // If area grid count of playing card is corrent, the data will put in 'temporaryData'.
    // If pay card confirm, the data will put to skill arealist
    ConfirmAreaGridData temporaryData;

    [Header("Game Prefab Assets")]
    public Sprite cardAttackSprite;
    public Sprite cardTankSprite;
    public Sprite cardMoveSprite;
    public Sprite cardFinalSkillSprite;

    public GameObject attackTextPrefab;

    [Header("Test")]
    public TextMeshProUGUI gameStepText;

    protected override void Awake()
    {
        base.Awake();

        //FIXME: 未來
        ChangeGameStep(GameStep.StepStart);

        //StartCoroutine(ExecuteCardActionList2(CommonCardActionList));
        StartCoroutine(LoopGameStepAction());

    }
    IEnumerator LoopGameStepAction()
    {
        while (true)
        {
            switch (gameStep)
            {
                case GameStep.StepStart:
                    ChangeGameStep(GameStep.PlayerStep);
                    break;

                case GameStep.PlayerStep:

                    ChangeGameStep(GameStep.CommonStep);
                    EventHanlder.CallPlayerStepAddCard();
                    break;

                case GameStep.CommonStep:
                    if (CommonCardActionList.Count != 0)
                        yield return StartCoroutine(ExecuteCardActionList(CommonCardActionList));
                    break;

                case GameStep.EnemySettlement:
                    //TODO: future to enemy 
                    currentCharacter = Character.Enemy;
                    ChangeGameStep(GameStep.EnemyStep);
                    break;

                case GameStep.EnemyStep:
                    ChangeGameStep(GameStep.AIStep);
                    break;

                case GameStep.AIStep:
                    //TODO: furture to enemy AI
                    yield return new WaitForSeconds(1); // FIXME: furure to do AI
                    ChangeGameStep(GameStep.PlayerSettlement);
                    break;

                case GameStep.PlayerSettlement:
                    currentCharacter = Character.Player;
                    yield return StartCoroutine(ExecuteCardActionList(PlayerSettlementCardActionList));
                    ChangeGameStep(GameStep.StepEnd); //TODO: future
                    break;

                case GameStep.StepEnd:
                    ChangeGameStep(GameStep.StepStart); //TODO: future
                    break;
            }
            yield return null;
        }
    }

    #region Tool Function
    /// <summary>
    /// Change GameStep to args
    /// </summary>
    public void ChangeGameStep(GameStep _toChange)
    {
        gameStep = _toChange;
        gameStepText.text = _toChange.ToString();
    }

    public Sprite CardTypeToCardBackgroud(CardDetail_SO data)
    {
        switch (data.cardType)
        {
            case CardType.Attack:
                if (data.isFinalSkill)
                    return cardFinalSkillSprite;
                else
                    return cardAttackSprite;

            case CardType.Move:
                return cardMoveSprite;

            case CardType.Tank:
                return cardTankSprite;

        }

        return null;
    }
    #endregion

    #region Event
    private void OnEnable()
    {
        EventHanlder.CardOnDrag += OnCardOnDrag; // Set gameData var
        EventHanlder.CardEndDrag += OnCardEndDrag; // Set gameData var
        EventHanlder.EndDragCofirmData += OnEndDragCofirmData; // Check skill confirm area is corrent
        EventHanlder.CancelPlayTheCard += OnCancelPlayTheCard; // change gameStep to CommonStep
        EventHanlder.PayCardComplete += OnPayCardComplete; // change gameStep, and call event to change grid color
        EventHanlder.PlayerHurt += OnPlayerHurt; // change player health
        EventHanlder.EnemyHurt += OnEnemyHurt;   // change enemy health
        EventHanlder.CommandStepEnd += OnCommandStepEnd; // change gameStep
    }
    private void OnDisable()
    {
        EventHanlder.CardOnDrag -= OnCardOnDrag;
        EventHanlder.CardEndDrag -= OnCardEndDrag;
        EventHanlder.EndDragCofirmData -= OnEndDragCofirmData;
        EventHanlder.CancelPlayTheCard -= OnCancelPlayTheCard;
        EventHanlder.PayCardComplete -= OnPayCardComplete;
        EventHanlder.PlayerHurt -= OnPlayerHurt;
        EventHanlder.EnemyHurt -= OnEnemyHurt;
        EventHanlder.CommandStepEnd += OnCommandStepEnd;
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

        // Get checkGridCount
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

        // Check the confirm area count
        if (gridCount == checkGridCount)
        {
            //Is right

            //card pay UI
            Debug.Log("GameManager: The confirm area count is right");
            temporaryData = data;
            EventHanlder.CallPlayTheCard(data.cardDetail);
        }
        else
        {
            EventHanlder.CallSendGameMessage("請確認技能釋放範圍完整");
            EventHanlder.CallCancelPlayTheCard();
        }
    }

    private void OnPlayTheCardChangeGameStep()
    {
        ChangeGameStep(GameStep.PayCardStep);
    }
    private void OnCancelPlayTheCard()
    {
        temporaryData = null; // Init 
        playingCard = null;
        ChangeGameStep(GameStep.CommonStep);
    }

    private void OnPayCardComplete()
    {
        ChangeGameStep(GameStep.CommonStep);
        CardUseToGiveCard(temporaryData.cardDetail); // Give Card about data list

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
        playingCard = null;

        AddAllConfirmGrid();

        EventHanlder.CallSendGameMessage("卡牌使用成功");
    }

    private void OnPlayerHurt(int hurtNum)
    {
        AttackCardHurtCharacter(hurtNum, playerHealth, playerArmor);
    }
    private void OnEnemyHurt(int hurtNum)
    {
        AttackCardHurtCharacter(hurtNum, enemyHealth, enemyArmor);
        //TODO: Give Effect
    }

    private void OnCommandStepEnd()
    {
        ChangeGameStep(GameStep.EnemySettlement);
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


    /// <summary>
    /// Execute CardActionList by step
    /// </summary>
    /// <param name="actionList">CardActionList</param>
    IEnumerator ExecuteCardActionList(List<ConfirmAreaGridData> actionList)
    {
        WaitForSeconds wait;

        // If don't setting wait time, this method will error 
        // (can't add item in list when coroutine not done )
        if (actionList == CommonCardActionList)
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

                        // 1. Call grid to play animation and check character health
                        EventHanlder.CallAttackGrid(grid, skill.cardDetail);


                        // 2. Remove the grid in AllConfirmGridList of now executing
                        foreach (ConfirmGrid confirmGrid in AllConfirmGridList)
                        {
                            if (confirmGrid == grid)
                            {
                                AllConfirmGridList.Remove(confirmGrid);
                                break;
                            }
                        }

                        // 3. To GridManager reload grid color
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

    private void AttackCardHurtCharacter(int hurtNum, int health, int armor)
    {
        //int hurtNum = data.attackTypeDetails.cardHurtHP;

        //Debug.Log("Chageeeeeeeeeeeeeeeeeeeee"); //FIXM
        //  Change Health
        if (armor != 0)
        {
            // Change Armor
            if (hurtNum > armor)
            {
                // Armor broke
                health -= hurtNum - armor;
                armor = 0;
            }
            else
            {
                armor -= hurtNum;
            }
        }
        else
        {
            health -= hurtNum;
        }

        // Set Health with new value
        if (currentCharacter == Character.Player)
        {
            // Enemy Hurt
            enemyHealth = health;
            enemyArmor = armor;
        }
        else
        {
            playerHealth = health;
            playerArmor = armor;
        }

        // Update UI
        EventHanlder.CallHealthChange();
        EventHanlder.CallArmorChange();
    }

    /// <summary>
    /// Give CardInstantiateCardList of cardDetail_SO to Instantiate the cardObj
    /// </summary>
    /// <param name="data"></param>
    private void CardUseToGiveCard(CardDetail_SO data)
    {
        foreach (CardDetail_SO newCardData in data.attackTypeDetails.CardInstantiateCardList)
        {
            CardManager.Instance.AddCard(newCardData);
        }
    }
}
