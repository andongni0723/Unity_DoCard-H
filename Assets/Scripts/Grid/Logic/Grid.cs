using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [Header("Script Setting")]
    public bool isEnemyGrid = false;
    public ConfirmGrid gridID;

    [Header("Grid Setting")]
    public bool isPlayerOn = false;
    public bool isMouseOnArea = false;
    public bool isSkilArea = false;

    public Color basicColor = new Color(255, 255, 255, 76);
    public Color playerOnColor = new Color(0, 255, 40, 76);

    public Color mouseAreaColor = new Color(255, 255, 255, 255);
    public Color skillAreaColor = new Color(255, 255, 0, 255);

    public Color dangerousColor = Color.black;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        FindPlayer();
    }


    #region Event
    private void OnEnable()
    {
        EventHanlder.ReloadGridColor += OnReloadGridColor;
    }
    private void OnDisable()
    {
        EventHanlder.ReloadGridColor -= OnReloadGridColor;

    }

    private void OnReloadGridColor(List<ConfirmGrid> grids)
    {
        isSkilArea = false;

        foreach (ConfirmGrid grid in grids)
        {
            if (grid == gridID)
            {
                isSkilArea = true;
                CheckGridColor();
            }
        }
    }
    #endregion


    private void OnTransformChildrenChanged()
    {
        FindPlayer();
    }

    /// <summary>
    /// Find Player in children
    /// </summary>
    private void FindPlayer()
    {
        isPlayerOn = false;
        Transform[] childList = GetComponentsInChildren<Transform>();

        // Find Player in children witn tag
        foreach (Transform item in childList)
        {
            if (item.CompareTag("Player"))
            {
                isPlayerOn = true;
            }
        }

        // Setting grid color
        CheckGridColor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CheckPlayingCardType())
        {
            // When Mouse Cofirm the area, the grid color will change
            if (other.CompareTag("mousePointer"))
            {
                isMouseOnArea = true;
                CheckGridColor();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("mousePointer"))
            {
                // Init the color
                isMouseOnArea = false;
                CheckGridColor();
            }
    }

    /// <summary>
    /// Check Playing Card Type to decid open the confirm area
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayingCardType()
    {
        CardDetail_SO playingCard = GameManager.Instance.playingCard;

        if(playingCard.cardType == CardType.Attack && isEnemyGrid) return true; //Playing a attack card
        if(playingCard.cardType == CardType.Move && !isEnemyGrid) return true;  //Playing a move card
        
        return false; //Playing a tank card
    }

    public void CheckGridColor()
    {
        if (isMouseOnArea)
        {
            spriteRenderer.color = mouseAreaColor;
        }
        else if (isSkilArea && isPlayerOn) // Player on skill attack area
        {
            spriteRenderer.color = dangerousColor;
        }
        else if (isSkilArea)
        {
            spriteRenderer.color = skillAreaColor;
        }
        else if (isPlayerOn)
        {
            spriteRenderer.color = playerOnColor;
        }
        else
        {
            spriteRenderer.color = basicColor;
        }
    }
}
