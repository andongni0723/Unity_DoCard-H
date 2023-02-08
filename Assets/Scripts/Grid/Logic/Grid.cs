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
    public bool isCharacterOn = false;
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
        FindCharacter();
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
        FindCharacter();
        CheckGridColor();
    }

    /// <summary>
    /// Find Character in children
    /// </summary>
    private void FindCharacter()
    {
        isCharacterOn = false;
        Transform[] childList = GetComponentsInChildren<Transform>();

        // Find Player in children witn tag
        foreach (Transform item in childList)
        {
            if (item.CompareTag("Player") || item.CompareTag("Enemy"))
            {
                isCharacterOn = true;
            }
        }

        // Setting grid color
        CheckGridColor();
    }


    /// <summary>
    /// Check Playing Card Type to decide open the confirm area
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayingCardType()
    {
        CardDetail_SO playingCard = GameManager.Instance.playingCard;

        if (playingCard.cardType == CardType.Attack && isEnemyGrid) return true; //Playing a attack card
        if (playingCard.cardType == CardType.Move && !isEnemyGrid) return true;  //Playing a move card

        return false; //Playing a tank card
    }

    public void CheckGridColor()
    {
        if (isMouseOnArea)
        {
            spriteRenderer.color = mouseAreaColor;
        }
        else if (isSkilArea && isCharacterOn) // Player on skill attack area
        {
            spriteRenderer.color = dangerousColor;
        }
        else if (isSkilArea)
        {
            spriteRenderer.color = skillAreaColor;
        }
        else if (isCharacterOn)
        {
            spriteRenderer.color = playerOnColor;
        }
        else
        {
            spriteRenderer.color = basicColor;
        }
    }

    /// <summary>
    /// This method will call when GridManager call, Inst the attack vfx and check player hurt
    /// </summary>
    /// <param name="animPrefabs"></param>
    public void CallAttackGrid(GameObject animPrefabs, CardDetail_SO data)
    {
        // Play the animation object
        var obj = Instantiate(animPrefabs, transform.position, Quaternion.identity, transform) as GameObject;       
        obj.transform.localPosition = Vector3.up; // Set animation object position

        // Check character health
        if(isCharacterOn)
        {
            // Character hurt, need play animation and check hurt
            //Debug.Log("hurtttttttttttt");//FIXM
            EventHanlder.CallCaracterHurt(GameManager.Instance.currentCharacter, data.attackTypeDetails.cardHurtHP);
        }
    }
}
