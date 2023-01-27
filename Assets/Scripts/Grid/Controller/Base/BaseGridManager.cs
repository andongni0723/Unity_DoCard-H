using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGridManager : MonoBehaviour
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

    protected virtual void OnEnable()
    { 
        //EventHanlder.ReloadGridData += OnReloadGridData;
    }

    protected virtual void OnDisable()
    {
        //EventHanlder.ReloadGridData -= OnReloadGridData;
    }

    protected virtual List<ConfirmGrid> UpdateData()
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
                // |-
                // |- ConfirmGrid
                //          |- gridX, gridY  => can get form script

                dataList.Add(gridCS.gridID);
            }
        }

        return dataList;
    }

    // private void OnReloadGridData(ConfirmAreaGridData data)
    // {
    //     //isConfirmGridsList.Add(data);
    //     EventHanlder.CallReloadGridColor(data);
    // }
}
