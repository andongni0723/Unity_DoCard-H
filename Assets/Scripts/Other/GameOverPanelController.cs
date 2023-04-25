using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class GameOverPanelSetting
{
    public Color panelMainColor;
    public Color textColor;
    public string text;
}

public class GameOverPanelController : MonoBehaviour
{
    [Header("UI Object")]
    public GameObject background;
    public GameObject panelUI;
    public GameObject quitButton;
    public TextMeshProUGUI tittleText;

    [Header("Script Setting")] 
    [SceneName] public string from;
    [SceneName] public string to;

    [Space(10f)] 
    public GameOverPanelSetting p1Setting;
    public GameOverPanelSetting p2Setting;


    GameOverPanelSetting currentSetting;
    
    #region Event
    private void OnEnable()
    {
        EventHanlder.CharacterDead += OnCharacterDead;
    }
    private void OnDisable()
    {
        EventHanlder.CharacterDead -= OnCharacterDead;
    }

    private void OnCharacterDead(Character character)
    {
        // Set Display UI
        currentSetting = character == Character.Player ? p2Setting : p1Setting;

        panelUI.GetComponent<Image>().color = currentSetting.panelMainColor;
        tittleText.color = currentSetting.textColor;
        tittleText.text = currentSetting.text;
        
        // Animation
        background.SetActive(true);
        PanelInAnimation();
    }
    #endregion
    

    private void PanelInAnimation()
    {
        float panelEndY = Screen.height * 0.175f; //Full HD(1080p) (190f)
        //Debug.Log(Screen.height);
        panelUI.GetComponent<RectTransform>().DOAnchorPosY(panelEndY, 0.5f);
    }


    #region Button Event

    public void QuitButton()
    {
        TeleportManager.Instance.Transition(from, to);
    }
    
    #endregion
}
