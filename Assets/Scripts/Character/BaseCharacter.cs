using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseCharacter : MonoBehaviour
{
    public CharacterCards_SO CardsDetails;
    GameObject canvus;

    private void Awake()
    {
        canvus = transform.Find("Canvas").gameObject;
    }

    protected virtual void OnMoveAction(ConfirmAreaGridData data)
    {
        // The ConfirmAreaGridData list first element is the move target grid
        GameObject toMoveGrid = transform.parent.parent.GetComponent<BaseGridManager>().GridPosToFindGrid(data.ConfirmGridsList[0]);
        Debug.Log(toMoveGrid.name); //FIXME
        Vector3 toParentPosition = new Vector3(0, 0.8f, -1);

        // DOTween animation
        transform.parent = toMoveGrid.transform;
        transform.DOMove(transform.parent.transform.position + toParentPosition, 1);

    }

    protected virtual void Hurt(CardDetail_SO data)
    {
        // The Hurt text animation

        //Debug.Log("Hurt text animation");//FIXM

        // If damage is 0, don't play animation
        if (GameManager.Instance.ValueListToInt(data.attackTypeDetails.cardHurtHPCalc) != 0)
        {
            GameObject obj = Instantiate(GameManager.Instance.attackTextPrefab, canvus.transform) as GameObject;
            obj.GetComponent<HurtText>().SetText(GameManager.Instance.ValueListToInt(data.attackTypeDetails.cardHurtHPCalc));

            // Character shake animation
            transform.DOPunchPosition(Vector3.right, 0.5f);
        }
    }

    // GameManger Call
    public void StatusHurtText(int hurtNum)
    {
        if (hurtNum != 0)
        {
            GameObject obj = Instantiate(GameManager.Instance.attackTextPrefab, canvus.transform) as GameObject;
            obj.GetComponent<HurtText>().SetText(hurtNum);
            // Character shake animation
            transform.DOPunchPosition(Vector3.right, 0.2f);
            // Camera shake animation
        }
    }
}
