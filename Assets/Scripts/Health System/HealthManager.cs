using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HealthManager : MonoBehaviour
{
    [Header("Script Setting")]
    public Color hpBarBasicColor = Color.red;
    public Color hpBarHaveArmorColor = new Color(255, 139, 0, 255);

    [Header("Children")]
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI armorText;

    private void Start()
    {
        UpdataUIStatue();
    }

    #region Event
    private void OnEnable()
    {
        EventHanlder.ArmorChange += UpdataUIStatue; // Update UI Statue
        EventHanlder.HealthChange += UpdataUIStatue; // Update UI Statue
    }
    private void OnDisable()
    {
        EventHanlder.ArmorChange -= UpdataUIStatue;
        EventHanlder.HealthChange -= UpdataUIStatue; 
    }
    #endregion

    private void UpdataUIStatue()
    {
        // HP
        slider.value = GameManager.Instance.playerHealth / 10;
        hpText.text = $"{GameManager.Instance.playerHealth}/10";

        // Armor
        armorText.text = GameManager.Instance.playerArmor.ToString();

        // According to armor value to change hpBar fill color
        if (GameManager.Instance.playerArmor != 0) // Have armor
        {
            slider.fillRect.GetComponent<Image>().color = hpBarHaveArmorColor;
        }
        else
        {
            slider.fillRect.GetComponent<Image>().color = hpBarBasicColor;
        }
    }
}
