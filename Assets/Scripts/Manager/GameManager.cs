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
    [HideInInspector] public GameObject PlayerGameObject;
    [HideInInspector] public GameObject EnemyGameObject;
    public List<ConfirmAreaGridData> PlayerSettlementCardActionList = new List<ConfirmAreaGridData>();
    public List<ConfirmAreaGridData> EnemySettlementCardActionList = new List<ConfirmAreaGridData>();
    public List<ConfirmAreaGridData> CommonCardActionList = new List<ConfirmAreaGridData>();
    public List<Effect> SettlementHurtStatusEffectActionList = new List<Effect>();
    [SerializeField]protected List<ConfirmGrid> AllConfirmGridList = new List<ConfirmGrid>();
    public CardDetail_SO playingCard;
    public int playerHurtSumCurrent;
    public int enemyHurtSumCurrent;

    public int playerLastHurtNum;
    public int enemyLastHurtNum;

    public int playerHealth = 10;
    public int playerArmor = 0;
    public int lastHurtNum = 0;
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

        // Setting
        PlayerGameObject = GameObject.FindWithTag("Player");
        EnemyGameObject = GameObject.FindWithTag("Enemy");

        //FIXME: 未來
        ChangeGameStep(GameStep.StepStart);

        // Game Step Action 
        StartCoroutine(LoopGameStepAction());

    }
    protected virtual IEnumerator LoopGameStepAction()
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
                    ChangeCardsOnStepStart(currentCharacter);
                    EventHanlder.CallPlayerStepAddCard();
                    break;

                case GameStep.CommonStep:
                    break;

                case GameStep.EnemySettlement:
                    //TODO: future to enemy 
                    currentCharacter = Character.Enemy;
                    yield return StartCoroutine(ExecuteCardActionList(EnemySettlementCardActionList));
                    yield return StartCoroutine(ExecuteStatusEffectActionList(SettlementHurtStatusEffectActionList));
                    
                    //Clear List
                    AddAllConfirmGrid();

                    enemyLastHurtNum = enemyHurtSumCurrent;
                    enemyHurtSumCurrent = 0;
                    ChangeGameStep(GameStep.EnemyStep);
                    break;

                case GameStep.EnemyStep:
                    ChangeGameStep(GameStep.AIStep);
                    ChangeCardsOnStepStart(currentCharacter);
                    break;

                case GameStep.AIStep:
                    //TODO: furture to enemy AI
                    yield return new WaitForSeconds(1); // FIXME: furure to do AI
                    ChangeGameStep(GameStep.PlayerSettlement);
                    break;

                case GameStep.PlayerSettlement:
                    currentCharacter = Character.Player;
                    yield return StartCoroutine(ExecuteCardActionList(PlayerSettlementCardActionList));
                    yield return StartCoroutine(ExecuteStatusEffectActionList(SettlementHurtStatusEffectActionList));
                    
                    //Clear List
                    AddAllConfirmGrid();

                    playerLastHurtNum = playerHurtSumCurrent;
                    playerHurtSumCurrent = 0;
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
    public virtual void ChangeGameStep(GameStep _toChange)
    {
        gameStep = _toChange;
        gameStepText.text = _toChange.ToString();
        EventHanlder.CallChangeGameStep(_toChange);
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

    public GameObject GameStepToGetCurrentAttackCharacter()
    {
        switch (currentCharacter)
        {
            case Character.Player:
                return PlayerGameObject;

            case Character.Enemy:
                return EnemyGameObject;
        }

        return null;
    }

    public Character RelativeCharacterToAbsolueCharacter(Character relativeCharacter)
    {
        switch (relativeCharacter)
        {
            case Character.Self:
                return currentCharacter == Character.Player ? Character.Player : Character.Enemy;
            case Character.Enemy:
                return currentCharacter == Character.Player ? Character.Enemy : Character.Player;
            default:
                return relativeCharacter;
        }
    }

    public int ValueListToInt(List<Value> valueList)
    {
        float waitCalcNum = 0;
        float waitCalcNum2 = 0;
        CalcSymbol calcSymbol = CalcSymbol.Null;
        float respond = 0;

        foreach (Value value in valueList)
        {
            if (waitCalcNum == 0)
            {
                waitCalcNum = ValueToFloat(value);
            }
            else if (calcSymbol == CalcSymbol.Null && value.isCalcSymbol)
            {
                calcSymbol = value.calcSymbol;
            }
            else if (waitCalcNum2 == 0)
            {
                waitCalcNum2 = ValueToFloat(value);
            }
            else
            {
                Debug.LogError("GameManager: Card or Status ValueList Data Error (Value List switch Error)");

            }

            if (waitCalcNum != 0 && calcSymbol != CalcSymbol.Null && waitCalcNum2 != 0)
            {
                // Calc
                respond = FloatAndCalcSymbolToCalc(waitCalcNum, calcSymbol, waitCalcNum2);
            }
        }

        // Is dont need calc
        if (respond == 0)
        {
            respond = waitCalcNum;
        }

        //Debug.Log($"{waitCalcNum} {calcSymbol} {waitCalcNum2} = {respond}"); //FIXM
        return (int)respond;
    }

    public float ValueToFloat(Value value)
    {
        // Need calcNum
        if (value.isGameData)
        {
            // GameData
            return GameDataEnumToValue(value.gameDataVar);
        }
        else if (value.isGetStatusNum)
        {
            // Effect 
            if (value.getEffect.target == Character.Self && currentCharacter == Character.Player ||
               value.getEffect.target == Character.Enemy && currentCharacter == Character.Enemy)
            {
                return PlayerGameObject.GetComponentInChildren<BaseStatusManager>().GetStatusLevel(value.getEffect.effectData.effectType);
            }
            else if (value.getEffect.target == Character.Self && currentCharacter == Character.Enemy ||
                     value.getEffect.target == Character.Enemy && currentCharacter == Character.Player)
            {
                return EnemyGameObject.GetComponentInChildren<BaseStatusManager>().GetStatusLevel(value.getEffect.effectData.effectType);
            }
        }
        else if (value.isInt)
        {
            // Int
            return value.interger;
        }

        return 0;
    }

    public float GameDataEnumToValue(GameData targetGamedata)
    {
        switch (targetGamedata)
        {
            case GameData.selfHealth:
                return currentCharacter == Character.Player ? playerHealth : enemyHealth;

            case GameData.selfArmor:
                return currentCharacter == Character.Player ? playerArmor : enemyArmor;

            case GameData.enemyHealth:
                return currentCharacter == Character.Enemy ? playerHealth : enemyHealth;

            case GameData.enemyArmor:
                return currentCharacter == Character.Enemy ? playerArmor : enemyArmor;

            case GameData.LastStepHurt:
                if (currentCharacter == Character.Player)
                    return playerLastHurtNum;
                else
                    return enemyLastHurtNum;

            default:
                return 0;

        }
    }

    public float FloatAndCalcSymbolToCalc(float a, CalcSymbol calcSymbol, float b)
    {
        switch (calcSymbol)
        {
            case CalcSymbol.plus:
                return a + b;
            case CalcSymbol.minus:
                return a - b;
            case CalcSymbol.times:
                return a * b;
            case CalcSymbol.dividedBy:
                return a / b;
            default:
                return 0;
        }
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

        // Check the confirm area count and other status effect
        if (data.cardDetail.cardType == CardType.Move && GameStepToGetCurrentAttackCharacter().GetComponentInChildren<BaseStatusManager>()
                .HaveStatus(EffectType.Imprison))
        {
            // Character have imprison status effect
            EventHanlder.CallSendGameMessage("身上有禁錮效果，禁止移動");
            EventHanlder.CallCancelPlayTheCard();
        }
        else if (gridCount != checkGridCount)
        {
            EventHanlder.CallSendGameMessage("請確認技能釋放範圍完整");
            EventHanlder.CallCancelPlayTheCard();
        }
        else if (gridCount == checkGridCount)
        {
            //Is right

            //card pay UI
            Debug.Log("GameManager: The confirm area count is right");
            temporaryData = data;
            EventHanlder.CallPlayTheCard(data.cardDetail);
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
            case CardUseStep.CommondStep:
                //Debug.Log("add temporay"); //FIXM
                CommonCardActionList.Add(temporaryData);
                break;

            case CardUseStep.SettlementStep:
                if (currentCharacter == Character.Player)
                    PlayerSettlementCardActionList.Add(temporaryData);
                else
                    EnemySettlementCardActionList.Add(temporaryData);//TODO: enemy
                break;
        }

        //Debug.Log("Before");
        playingCard = null;

        AddAllConfirmGrid();

        EventHanlder.CallSendGameMessage("卡牌使用成功");
    }

    private void OnPlayerHurt(CardDetail_SO data)
    {
        int damage = ValueListToInt(data.attackTypeDetails.cardHurtHPCalc);
        AttackCardHurtCharacter(damage, playerHealth, playerArmor);
        CardUseToGiveStatusEffect(data.attackTypeDetails.CardEffectList);
        CardUseToRemoveStatusEffect(data.attackTypeDetails.RemoveEffectList);

        playerHurtSumCurrent += damage;
    }
    private void OnEnemyHurt(CardDetail_SO data)
    {
        int damage = ValueListToInt(data.attackTypeDetails.cardHurtHPCalc);
        AttackCardHurtCharacter(damage, enemyHealth, enemyArmor);
        CardUseToGiveStatusEffect(data.attackTypeDetails.CardEffectList);
        CardUseToRemoveStatusEffect(data.attackTypeDetails.RemoveEffectList);

        enemyHurtSumCurrent += damage;
    }

    protected virtual void OnCommandStepEnd()
    {
        ChangeGameStep(GameStep.EnemySettlement);
    }
    #endregion

    protected void AddAllConfirmGrid()
    {
        //SkillHurtGridListPlayerSettlementCardAction
        // |- ConfirmAreaGridData
        //      |- ...
        //      |- ConfirmGridList
        //          |- ConfirmGrid <- Need
        //          |- ...

        AllConfirmGridList.Clear();


        foreach (ConfirmAreaGridData data in EnemySettlementCardActionList)
        {
            foreach (ConfirmGrid grid in data.ConfirmGridsList)
            {
                //Debug.Log("AAAAA");//FIXM
                AllConfirmGridList.Add(grid);
            }
        }

        foreach (ConfirmAreaGridData data in PlayerSettlementCardActionList)
        {
            foreach (ConfirmGrid grid in data.ConfirmGridsList)
            {
                //Debug.Log("AAAAA");//FIXM
                AllConfirmGridList.Add(grid);
            }
        }

        foreach (ConfirmAreaGridData data in CommonCardActionList)
        {
            foreach (ConfirmGrid grid in data.ConfirmGridsList)
            {
                //Debug.Log("AAAAA");//FIXM
                AllConfirmGridList.Add(grid);
            }
        }

        #region Del
        // if (currentCharacter == Character.Player)
        // {
        //     foreach (ConfirmAreaGridData data in EnemySettlementCardActionList)
        //     {
        //         foreach (ConfirmGrid grid in data.ConfirmGridsList)
        //         {
        //             Debug.Log("AAAAA");//FIXM

        //             AllConfirmGridList.Add(grid);
        //         }
        //     }

        //     foreach (ConfirmAreaGridData data in PlayerSettlementCardActionList)
        //     {
        //         foreach (ConfirmGrid grid in data.ConfirmGridsList)
        //         {
        //             Debug.Log("AAAAA");//FIXM

        //             AllConfirmGridList.Add(grid);
        //         }
        //     }
        // }
        // else
        // {
        //     //Enemy
        //     foreach (ConfirmAreaGridData data in PlayerSettlementCardActionList)
        //     {
        //         foreach (ConfirmGrid grid in data.ConfirmGridsList)
        //         {
        //             Debug.Log("AAAAA");//FIXM

        //             AllConfirmGridList.Add(grid);
        //         }
        //     }

        //     foreach (ConfirmAreaGridData data in EnemySettlementCardActionList)
        //     {
        //         foreach (ConfirmGrid grid in data.ConfirmGridsList)
        //         {
        //             Debug.Log("AAAAA");//FIXM

        //             AllConfirmGridList.Add(grid);
        //         }
        //     }
        // }
        #endregion

        // Reload when function done
        EventHanlder.CallReloadGridColor(AllConfirmGridList); // To GridManager reload grid color

        StartCoroutine(ExecuteCardActionList(CommonCardActionList));
    }

    protected void ChangeCardsOnStepStart(Character character)
    {
        switch (character)
        {
            case Character.Player:
                EventHanlder.CallChangeCardsOnStepStart(PlayerGameObject.GetComponent<BaseCharacter>().CardsDetails);
                break;

            case Character.Enemy:
                EventHanlder.CallChangeCardsOnStepStart(EnemyGameObject.GetComponent<BaseCharacter>().CardsDetails);
                break;
        }
    }

    /// <summary>
    /// Execute CardActionList by step
    /// </summary>
    /// <param name="actionList">CardActionList</param>
    protected IEnumerator ExecuteCardActionList(List<ConfirmAreaGridData> actionList)
    {
        WaitForSeconds wait;
        Debug.Log("ExecuteCardActionList"); //FIXME

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

                        // // Commond don't need remove grid , because the grid of commond card didn't add in
                        // foreach (ConfirmGrid confirmGrid in AllConfirmGridList)
                        // {
                        //     if (confirmGrid == grid)
                        //     {
                        //         AllConfirmGridList.Remove(confirmGrid);
                        //         Debug.Log("grid find to destroy"); //FIXME
                        //         break;
                        //     }
                        // }


                        // // 3. To GridManager reload grid color
                        // EventHanlder.CallReloadGridColor(AllConfirmGridList);
                    }

                    yield return wait;
                    break;

                case CardType.Move:
                    EventHanlder.CallMoveAction(skill);
                    break;

                case CardType.Tank:
                    if (currentCharacter == Character.Player)
                    {
                        playerArmor += skill.cardDetail.tankTypeDetails.addArmor;
                        playerHealth += skill.cardDetail.tankTypeDetails.addHealth;
                    }
                    else
                    {
                        //Enemy
                        enemyArmor += skill.cardDetail.tankTypeDetails.addArmor;
                        enemyHealth += skill.cardDetail.tankTypeDetails.addHealth;
                    }

                    EventHanlder.CallArmorChange();
                    EventHanlder.CallHealthChange();
                    break;
            }

        }


        actionList.Clear();
        
        yield return null;
    }

    protected IEnumerator ExecuteStatusEffectActionList(List<Effect> effectList)
    {
        WaitForSeconds wait = new WaitForSeconds(2);

        // Hurt Status effect
        foreach (Effect status in effectList)
        {

            if (currentCharacter == Character.Enemy) // Player Hurt
            {
                AttackCardHurtCharacter(status.effectData.logicTrigger.hurtLogic.hurt, playerHealth, playerArmor);
                PlayerGameObject.GetComponent<BaseCharacter>().StatusHurtText(status.effectData.logicTrigger.hurtLogic.hurt); // Play hurt Text animation
                PlayerGameObject.GetComponentInChildren<BaseStatusManager>().RemoveStatusEffect(status.effectData, 1);
            }
            else if (currentCharacter == Character.Player) // Enemy Hurt
            {
                AttackCardHurtCharacter(status.effectData.logicTrigger.hurtLogic.hurt, enemyHealth, enemyArmor);
                EnemyGameObject.GetComponent<BaseCharacter>().StatusHurtText(status.effectData.logicTrigger.hurtLogic.hurt); // Play hurt Text animation
                EnemyGameObject.GetComponentInChildren<BaseStatusManager>().RemoveStatusEffect(status.effectData, 1);
            }

            yield return wait;
        }

        effectList.Clear();
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

        foreach (CardDetail_SO newCardData in data.tankTypeDetails.CardInstantiateCardList)
        {
            CardManager.Instance.AddCard(newCardData);
        }
    }

    private void CardUseToGiveStatusEffect(List<Effect> effectList)
    {
        GameObject target = null;

        foreach (Effect effect in effectList)
        {
            // Check target of status effect to give
            if ((effect.target == Character.Self && currentCharacter == Character.Player) ||
                effect.target == Character.Enemy && currentCharacter == Character.Enemy)
            {
                target = PlayerGameObject;
            }
            else if ((effect.target == Character.Self && currentCharacter == Character.Enemy) ||
                     effect.target == Character.Enemy && currentCharacter == Character.Player)
            {
                target = EnemyGameObject;
            }

            // Add status effect to target
            target.GetComponentInChildren<BaseStatusManager>().AddStatusEffect(effect.effectData, effect.effectCount);
        }
    }

    private void CardUseToRemoveStatusEffect(List<Effect> effectList)
    {
        GameObject target = null;

        foreach (Effect effect in effectList)
        {
            // Check target of status effect to give
            if ((effect.target == Character.Self && currentCharacter == Character.Player) ||
                effect.target == Character.Enemy && currentCharacter == Character.Enemy)
            {
                target = PlayerGameObject;
            }
            else if ((effect.target == Character.Self && currentCharacter == Character.Enemy) ||
                     effect.target == Character.Enemy && currentCharacter == Character.Player)
            {
                target = EnemyGameObject;
            }

            // Remove status effect to target
            target.GetComponentInChildren<BaseStatusManager>().RemoveStatusEffect(effect.effectData, -1);
        }
    }
}
