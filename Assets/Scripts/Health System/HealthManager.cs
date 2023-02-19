using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HealthManager : MonoBehaviour
{
    [Header("Script Setting")]
    public bool isPlayer;
    public Color hpBarBasicColor = Color.red;
    public Color hpBarHaveArmorColor = new Color(255, 139, 0, 255);

    [Header("Children")]
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI armorText;

    private void Start()
    {
        // Check parent is player or enemy
        isPlayer = transform.parent.parent.CompareTag("Player") ? true : false;

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
        float characterHealth = isPlayer ? GameManager.Instance.playerHealth : GameManager.Instance.enemyHealth;

        slider.value = characterHealth / 15;   //UI
        hpText.text = $"{characterHealth}/15"; //UI

        // Armor
        float characterArmor = isPlayer ? GameManager.Instance.playerArmor : GameManager.Instance.enemyArmor;

        armorText.text = characterArmor.ToString(); //UI

        // According to armor value to change hpBar fill color
        if (characterArmor != 0) 
        {
            // Have armor
            slider.fillRect.GetComponent<Image>().color = hpBarHaveArmorColor;
        }
        else
        {
            slider.fillRect.GetComponent<Image>().color = hpBarBasicColor;
        }
    }
}
