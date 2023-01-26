using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameMessage : MonoBehaviour
{
    TextMeshProUGUI text;
    CanvasGroup canvasGroup;
    GameObject moveDonePoint;
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        moveDonePoint = GameObject.FindGameObjectWithTag("messageDonePoint");

        
    }

    public void PlayMessage(string message)
    {
        Sequence sequence = DOTween.Sequence();
        text.text = message;

        sequence.Append(transform.DOMoveY(moveDonePoint.transform.position.y, 1f));
        sequence.Append(canvasGroup.DOFade(0, 1));
        sequence.OnComplete(() => Destroy(gameObject));
    }
}
