using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChooseBarElement : MonoBehaviour
{
    public Character character;
    public ChooseCharacterDetails_SO chooseCharacterDetails;

    public Image iconImage;
    public Button button;

    private void Awake()
    {
        //iconImage = transform.GetChild(0).GetComponent<Image>();
        //button = GetComponent<Button>();
    }

    /// <summary>
    /// Instantiate this object , call this method to setting data
    /// </summary>
    /// <param name="forCharacter"></param>
    /// <param name="details"></param>
    public void SetDetails(Character forCharacter, ChooseCharacterDetails_SO details)
    {
        character = forCharacter;
        chooseCharacterDetails = details;
        
        DisplayUI();
    }

    private void DisplayUI()
    {
        //Debug.Log(iconImage);
        iconImage.sprite = chooseCharacterDetails.characterSprite;
        
        button.onClick.AddListener(() =>
        {
            EventHanlder.CallChooseCharacterGridButton(character, chooseCharacterDetails);
            Debug.Log("C");
        });
    }
}
