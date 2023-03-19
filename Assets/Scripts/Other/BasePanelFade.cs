using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BasePanelFade : MonoBehaviour, IPointerClickHandler
{
    public GameObject mainPanel;
    public float maxValueScale = 1.2f;
    public float minValueScale = 0.8f;
    public float shortTime = 0.3f;
    public float longTime = 0.6f;
    private IPointerClickHandler _pointerClickHandlerImplementation;

    private void OnEnable()
    {
        StartCoroutine(StartFadeAnimation());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(EndFadeAnimation());
    }

    IEnumerator StartFadeAnimation()
    {
        mainPanel.transform.localScale = new Vector3(minValueScale, minValueScale, minValueScale);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(mainPanel.transform.DOScale(new Vector3(maxValueScale, maxValueScale, maxValueScale), shortTime));
        sequence.Append(mainPanel.transform.DOScale(Vector3.one, longTime));
        
        yield return null;
    }
    IEnumerator EndFadeAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(mainPanel.transform.DOScale(new Vector3(minValueScale, minValueScale, minValueScale), shortTime)).OnComplete(
            () =>
            {
                gameObject.SetActive(false);
            });
        
        
        yield return null;
    }
}
