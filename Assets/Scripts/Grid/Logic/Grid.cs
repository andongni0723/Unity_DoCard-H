using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public ConfirmGrid gridID;

    [Header("Grid Setting")]
    [SerializeField] bool isEnemyGrid = false;
    [SerializeField] bool isPlayerOn = false;
    public bool isMouseOnArea = false;

    public Color basicColor = new Color(255, 255, 255, 76);
    public Color playerOnColor = new Color(0, 255, 40, 76);

    public Color mouseAreaColor = new Color(255, 255, 255, 255);

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        FindPlayer();
    }

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
        if (isPlayerOn)
            spriteRenderer.color = playerOnColor;
        else
            spriteRenderer.color = basicColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!isEnemyGrid) return;

        // When Mouse Cofirm the area, the grid color will change
        if (other.CompareTag("mousePointer"))
        {
            spriteRenderer.color = mouseAreaColor;
            isMouseOnArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!isEnemyGrid) return;
        
        if (other.CompareTag("mousePointer"))
        {
            // Init the color
            spriteRenderer.color = basicColor;
            isMouseOnArea = false;
        }
    }
}