using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class EventHanlder
{
    /* Message System */
    #region Message System
    // GameManager => MessageManager
    public static event Action<string> SendGameMessage;
    public static void CallSendGameMessage(string message)
    {
        SendGameMessage?.Invoke(message);
    }
    #endregion


    /*Game Mangager*/
    #region GameManger
    // GameManager => Other Scripts
    public static event Action OnPlayerSettlement;
    public static event Action OnEnemySettlement;
    public static event Action OnPlayerStep;
    public static event Action OnEnemyStep;

    public static void CallChangeGameStep(GameStep targetGameStep)
    {
        switch (targetGameStep)
        {
            case GameStep.PlayerStep:
                OnPlayerStep?.Invoke();
                break;
            case GameStep.PlayerSettlement:
                OnPlayerSettlement?.Invoke();
                break;
            case GameStep.EnemyStep:
                OnEnemyStep?.Invoke();
                break;
            case GameStep.EnemySettlement:
                OnEnemySettlement?.Invoke();
                break;
        }
    }
    #endregion

    /* Card System */
    #region Card System

    // Softed Card Position
    public static event Action CardUpdatePosition;
    public static void CallCardUpdeatePosition()
    {
        CardUpdatePosition?.Invoke();
    }


    // Card Event
    public static event Action<CharacterCards_SO> ChangeCardsOnStepStart;
    public static void CallChangeCardsOnStepStart(CharacterCards_SO cards)
    {
        ChangeCardsOnStepStart?.Invoke(cards);
    }
    public static event Action PlayerStepAddCard;
    public static void CallPlayerStepAddCard()
    {
        PlayerStepAddCard?.Invoke();
    }

    public static event Action<CardDetail_SO> CardOnDrag;
    public static void CallCardOnDrag(CardDetail_SO cardDetail)
    {
        CardOnDrag?.Invoke(cardDetail);
    }

    public static event Action<CardDetail_SO> CardOnClick;
    public static void CallCardOnClick(CardDetail_SO data)
    {
        CardOnClick?.Invoke(data);
    }

    // End Drag
    public static Func<List<ConfirmGrid>> EndDragPlayerGridUpdateData; // Player's Grid "GridManager" confirm the grid of mouse choose 

    public static Func<List<ConfirmGrid>> EndDragEnemyGridUpdateData; // Enemy's Grid "GridManager" confirm the grid of mouse choose 
    public static event Action CardEndDrag;
    public static event Action<ConfirmAreaGridData> EndDragCofirmData; //GameManager: EndDragCofirmData

    public static void CallCardEndDrag()
    {
        /* ConfirmAreaGridData */
        /// data
        /// |- cardDetail
        /// |- targetGridForCharacter
        /// |- ConfirmGridsList
        ///     |- GridForCharacter
        ///     |- ...

        ConfirmAreaGridData data = new ConfirmAreaGridData();

        // cardDetail: "GameManager" will sent CardDetail_SO
        data.cardDetail = GameManager.Instance.playingCard;

        // Setting data target(Relative) for grid
        data.targetGridForCharacter =data.cardDetail.cardType == CardType.Move?  Character.Self : Character.Enemy;
    

        // ConfirmGridsList: "GridManager" confirm the grid of mouse choose 
        if (GameManager.Instance.currentCharacter == Character.Player && data.cardDetail.cardType == CardType.Move ||
            GameManager.Instance.currentCharacter == Character.Enemy && data.cardDetail.cardType == CardType.Attack)
        {
            // Move card will aim the Player's grid
            data.ConfirmGridsList = EndDragPlayerGridUpdateData?.Invoke();
        }
        else if (GameManager.Instance.currentCharacter == Character.Player && data.cardDetail.cardType == CardType.Attack ||
                 GameManager.Instance.currentCharacter == Character.Enemy && data.cardDetail.cardType == CardType.Move)
        {
            // Attack card will aim the Enemy's grid
            data.ConfirmGridsList = EndDragEnemyGridUpdateData?.Invoke();
        }

        // Setting data grid target(Absoulue)
        foreach (ConfirmGrid grid in data.ConfirmGridsList)
        {
            grid.GridForCharacter = GameManager.Instance.RelativeCharacterToAbsolueCharacter(data.targetGridForCharacter);
        }

        CardEndDrag?.Invoke();

        // Then sent data to "GameManager" to check data
        EndDragCofirmData?.Invoke(data);
    }


    // After End Drag
    public static event Action<CardDetail_SO> PlayTheCard;
    public static void CallPlayTheCard(CardDetail_SO cardDetail)
    {
        //GameStepChange?.Invoke();
        GameManager.Instance.ChangeGameStep(GameStep.PayCardStep);
        PlayTheCard?.Invoke(cardDetail);
    }

    public static event Action<GameObject> PayTheCard;
    public static void CallPayTheCard(GameObject card)
    {
        PayTheCard?.Invoke(card);
    }

    public static event Action<GameObject> PayTheCardError;
    public static void CallPayTheCardError(GameObject card)
    {
        PayTheCardError?.Invoke(card);
    }

    public static event Action CancelPlayTheCard;
    public static void CallCancelPlayTheCard()
    {
        GameManager.Instance.ChangeGameStep(GameStep.CommonStep);
        CancelPlayTheCard?.Invoke();
    }

    public static event Action PayCardComplete;
    public static void CallPayCardComplete()
    {
        GameManager.Instance.ChangeGameStep(GameStep.CommonStep);
        PayCardComplete?.Invoke();
        EventHanlder.CallCardUpdeatePosition();
    }


    // // GameManager Reload confirm data 
    // public static event Action<List<ConfirmGrid>> ReloadGridData;
    // public static void CallReloadGridData(List<ConfirmGrid> grids)
    // {
    //     ReloadGridData?.Invoke(grids);
    // }

    // GridManager => Grid
    public static event Action<List<ConfirmGrid>> ReloadGridColor;
    public static void CallReloadGridColor(List<ConfirmGrid> grids)
    {
        ReloadGridColor?.Invoke(grids);
    }

    public static event Action CommandStepEnd;
    public static void CallCommandStepEnd()
    {
        CommandStepEnd?.Invoke();
    }
    #endregion


    /* Health System */
    #region Health System

    // Game Data Change
    // GameManager => HealthManager
    public static event Action HealthChange;
    public static void CallHealthChange()
    {
        HealthChange?.Invoke();
    }
    public static event Action ArmorChange;
    public static void CallArmorChange()
    {
        ArmorChange?.Invoke();
    }

    // GameManager execute the card action
    // GameManager => Grid && => Player
    public static event Action<ConfirmAreaGridData> PlayerMoveAction;
    public static event Action<ConfirmAreaGridData> EnemyMoveAction;
    public static void CallMoveAction(ConfirmAreaGridData data)
    {
        if(GameManager.Instance.currentCharacter == Character.Player)
            PlayerMoveAction?.Invoke(data);
        else
            EnemyMoveAction.Invoke(data);
    }

    //GameManager execute the attack action, call the grid of skill area
    public static event Action<ConfirmGrid, CardDetail_SO> PlayerAttackGrid;
    public static event Action<ConfirmGrid, CardDetail_SO> EnemyAttackGrid;
    public static void CallAttackGrid(ConfirmGrid grid, CardDetail_SO data)
    {
        if (GameManager.Instance.currentCharacter == Character.Player)
        {
            // Player Attack
            PlayerAttackGrid?.Invoke(grid, data);
        }
        else
        {
            EnemyAttackGrid?.Invoke(grid, data); //TODO: Future AI
        }
    }

    public static event Action<CardDetail_SO> PlayerHurt;
    public static event Action<CardDetail_SO> EnemyHurt;
    public static void CallCaracterHurt(Character hurtCharacter, CardDetail_SO data)
    {
        if (hurtCharacter == Character.Player)
        {
            // Player Attack
            EnemyHurt(data);
        }
        else
        {
            PlayerHurt(data);
        }
    }
    #endregion
}
