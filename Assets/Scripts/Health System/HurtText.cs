using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class HurtText : MonoBehaviour
{
    TextMeshProUGUI text;
    CanvasGroup canvasGroup;

    Vector3 startPosition;
    Vector3 startScale;

    [Header("Script Setting")]
    public Vector3 BigScale = Vector3.one;
    public float BigAnimTime = 0.01f;
    public float SmallAnimTime = 0.5f;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        startScale = transform.localScale;
        startPosition = transform.position;
        TextAnimation(); 
    }

    /// <summary>
    /// The method will call when 'HealthManager' Init this gameObject , to setting hurt text
    /// </summary>
    /// <param name="hurtNum">text</param>
    public void SetText(int hurtNum)
    {
        text.text = hurtNum.ToString();
    }

    private void TextAnimation()
    {
        /// Animation Time Line ///
        /// 
        /// MoveUp -------------------> UP
        /// Small--> Big -------------> Small
        /// White--> Red --> White----> 
        ///          FadeOut----------> Zero
        
        
        // INIT
        transform.position = startPosition;
        transform.localScale = startScale;
        canvasGroup.alpha = 1;
        transform.localScale = Vector3.zero;

        Sequence moveSequence = DOTween.Sequence();
        Sequence scaleSequence = DOTween.Sequence();
        Sequence colorSequence = DOTween.Sequence();
        
        // Move
        moveSequence.Append(transform.DOMove(startPosition + Vector3.up * 1, 1.5f).SetEase(Ease.OutSine));

        // Scale
        scaleSequence.Append(transform.DOScale(BigScale, BigAnimTime));
        scaleSequence.Append(transform.DOScale(Vector3.zero, 1).SetEase(Ease.OutSine));
        scaleSequence.Join(canvasGroup.DOFade(0, 0.5f));

        // Color
        colorSequence.Append(text.DOColor(Color.red, BigAnimTime));
        colorSequence.Append(text.DOColor(Color.white, SmallAnimTime));


    }
}
