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
    
    
    /* Card System */
    #region Card System

    // Softed Card Position
    public static event Action CardUpdatePosition;
    public static void CallCardUpdeatePosition()
    {
        CardUpdatePosition?.Invoke();
    }


    // Card Event
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
        ConfirmAreaGridData data = new ConfirmAreaGridData();

        // cardDetail: "GameManager" will sent CardDetail_SO
        data.cardDetail = GameManager.Instance.playingCard;

        // ConfirmGridsList: "GridManager" confirm the grid of mouse choose 
        switch (data.cardDetail.cardType)
        {
            case CardType.Attack: // Attack card will aim the Enemy's grid
                data.ConfirmGridsList = EndDragEnemyGridUpdateData?.Invoke();
                break;

            case CardType.Move: // Move card will aim the Player's grid
                data.ConfirmGridsList = EndDragPlayerGridUpdateData?.Invoke();
                break;
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
    public static event Action<ConfirmAreaGridData> MoveAction;
    public static void CallMoveAction(ConfirmAreaGridData data)
    {
        MoveAction?.Invoke(data);
    }

    //GameManager execute the attack action, call the grid of skill area
    public static event Action<ConfirmGrid, CardDetail_SO> PlayerAttackGrid;
    public static event Action<ConfirmGrid, CardDetail_SO> EnemyAttackGrid;
    public static void CallAttackGrid(ConfirmGrid grid, CardDetail_SO data)
    {
        if (GameManager.Instance.currentCharacter == Character.Player)
        {
            // Player Attack
            PlayerAttackGrid(grid, data);
        }
        else
        {
            //EnemyAttackGrid(grid, data); //TODO: Future AI
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
