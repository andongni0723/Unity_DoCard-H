using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DiscardAnimation : MonoBehaviour
{
    private void Start()
    {
        GetComponent<RectTransform>().localScale = new(0.5f, 0.5f, 0.5f);

        Sequence mainSequence = DOTween.Sequence();
        mainSequence.Append(transform.DOScale(0.3f, 0.3f));
        mainSequence.Append(transform.DOMove(CardManager.Instance.discardPilePoint.transform.position, 1f));
        mainSequence.OnComplete(() => Destroy(gameObject));
    }
}
