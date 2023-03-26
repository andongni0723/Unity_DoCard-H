using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Serialization;

public class ChooseCharacterPanelController : MonoBehaviour
{
    public ChooseCharacterDetails_SO currentShowCharacter;
    
    [Header("Setting")]
    public TextMeshProUGUI characterName;
    public Image characterImage;
    public TextMeshProUGUI characterDescription;
    
    
    #region Button Method

    public void ChooseCharacter(ChooseCharacterDetails_SO data)
    {
        currentShowCharacter = data;
        ReloadCharacterShow();
    }

    #endregion

    /// <summary>
    /// According to 'character_SO' to show different character details on UI
    /// </summary>
    private void ReloadCharacterShow()
    {
        characterName.text = currentShowCharacter.characterName;
        characterImage.sprite = currentShowCharacter.characterSprite;
        characterDescription.text = currentShowCharacter.characterDescription;
    }
}
