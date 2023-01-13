using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public List<GameObject> GridsList;
    public List<ConfirmAreaGridData> isConfirmGridsList;

    private void Awake() 
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GridsList.Add(transform.GetChild(i).gameObject);
        }
    }
    private void OnEnable()
    {
        EventHanlder.EndDragGridUpdateData += UpdateData;
    }

    private void OnDisable()
    {
        EventHanlder.EndDragGridUpdateData -= UpdateData;
    }

    private List<ConfirmGrid> UpdateData()
    {
        List<ConfirmGrid> dataList = new List<ConfirmGrid>();

        // Find object which "isMouseOnArea" is true to return data 
        foreach (var grid in GridsList)
        {
            // Get grid object script
            Grid gridCS = grid.GetComponent<Grid>();

            //isMouseOnArea
            if(gridCS.isMouseOnArea)
            {
                //ConfirmAreaGridData
                // |- ...
                // |- ConfirmGrid
                //          |- gridX, gridY  => can get form script

                dataList.Add(gridCS.gridID);
            }
        }

        return dataList;
    }
}