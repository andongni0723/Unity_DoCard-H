using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseCharacter : MonoBehaviour
{
    

    protected virtual void OnMoveAction(ConfirmAreaGridData data)
    {
        // The ConfirmAreaGridData list first element is the move target grid
        GameObject toMoveGrid = PlayerGridManager.instance.GridPosToFindGrid(data.ConfirmGridsList[0]); 
        Debug.Log(toMoveGrid.name); //FIXME
        Vector3 toParentPosition = new Vector3(0, 0.8f, -1);

        // DOTween animation
        transform.parent = toMoveGrid.transform;
        transform.DOMove(transform.parent.transform.position + toParentPosition, 1);
        
    }

    protected virtual void Hurt(CardDetail_SO data)
    {
        // The Hurt text animation
        // Character shake animation
        // Camera shake animation
    }
}
