using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseCharacter : MonoBehaviour
{
    GameObject canvus;

    private void Awake()
    {
        canvus = transform.Find("Canvas").gameObject;
    }

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

        Debug.Log("Hurt text animation");//FIXME
        GameObject obj = Instantiate(GameManager.Instance.attackTextPrefab, canvus.transform) as GameObject;
        obj.GetComponent<HurtText>().SetText(data.attackTypeDetails.cardHurtHP);
        // Character shake animation
        transform.DOPunchPosition(Vector3.right, 0.5f);
        // Camera shake animation
    }
}
