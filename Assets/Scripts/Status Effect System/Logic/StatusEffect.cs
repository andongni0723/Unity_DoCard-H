using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StatusEffect : MonoBehaviour
{
    public EffectDetail_SO effectDetail;

    Image image;
    TextMeshProUGUI text;


    private void Awake()
    {
        // Component
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        // Setting
        StartCoroutine(IconSetting());
    }

    IEnumerator IconSetting()
    {
        yield return new WaitWhile(() => effectDetail == null);

        // Setting
        image.sprite = effectDetail.effectIcon;
    }

    /// <summary>
    /// Set Child Text with args
    /// </summary>
    /// <param name="newStatusCount">text</param>
    public void ReloadStatusCountText(int newStatusCount)
    {
        if(newStatusCount == 1)
            text.text = null;
        else
            text.text = newStatusCount.ToString();
    }
}
