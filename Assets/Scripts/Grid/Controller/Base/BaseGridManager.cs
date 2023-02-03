using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGridManager : MonoBehaviour
{
    public List<GameObject> GridsList;
    public List<ConfirmAreaGridData> isConfirmGridsList;

    public GameObject attackVFXPrefabs;

    protected virtual void Awake() 
    {
        foreach (var item in GetComponentsInChildren<Grid>())
        {
            GridsList.Add(item.gameObject);
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
                // |- ...
                // |- ConfirmGrid
                //          |- gridX, gridY  => can get form script

                dataList.Add(gridCS.gridID);
            }
        }

        return dataList;
    }


    /// <summary>
    /// Find the target grid and call the event 
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="data"></param>
    protected virtual void OnAttackGridToCall(ConfirmGrid grid, CardDetail_SO data)
    {
        foreach (GameObject gridObj in GridsList)
        {
            Grid gridCS = gridObj.GetComponent<Grid>();
            if(gridCS.gridID == grid)
            {
                gridCS.CallAttackGrid(attackVFXPrefabs, data);
            }
        }
    }
    
    /// <summary>
    /// Use ConfirmGrid position to Get Grid GameObject
    /// </summary>
    /// <param name="toGrid">input grid position</param>
    /// <returns></returns>
    public virtual GameObject GridPosToFindGrid(ConfirmGrid toGrid)
    {
        foreach (Grid grid in GetComponentsInChildren<Grid>())
        {
            if(grid.gridID == toGrid)
                return grid.gameObject;
        }

        return null;
    }

    // private void OnReloadGridData(ConfirmAreaGridData data)
    // {
    //     //isConfirmGridsList.Add(data);
    //     EventHanlder.CallReloadGridColor(data);
    // }
}
