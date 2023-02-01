using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void OnMoveAction(ConfirmAreaGridData data)
    {
        // The data list first element is the to move grid
        GameObject toMoveGrid = PlayerGridManager.instance.GridPosToFindGrid(data.ConfirmGridsList[0]); 
        Debug.Log(toMoveGrid.name);
        Vector3 toParentPosition = new Vector3(0, 0.8f, -1);

        transform.parent = toMoveGrid.transform;
        transform.DOMove(transform.parent.transform.position + toParentPosition, 1);
        
    }

    protected virtual void Hurt(ConfirmAreaGridData data)
    {
        
    }
}
