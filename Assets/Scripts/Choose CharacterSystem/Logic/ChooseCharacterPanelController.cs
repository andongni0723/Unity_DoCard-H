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
    public Character character;

    public GameObject setActiveParent;
    public TextMeshProUGUI characterName;
    public Image characterImage;
    public TextMeshProUGUI characterDescription;
    public GameObject chooseBar;

    public GameObject disappearPoint;

    private void Start()
    {
        OnChooseCharacterChangeStep();
    }

    #region Button Method

    public void ChooseCharacter(ChooseCharacterDetails_SO data)
    {
        currentShowCharacter = data;
        ReloadCharacterShow();
    }

    #endregion

    #region Event

    private void OnEnable()
    {
        EventHanlder.ChooseCharacterChangeStep += OnChooseCharacterChangeStep;
    }

    private void OnDisable()
    {
        EventHanlder.ChooseCharacterChangeStep -= OnChooseCharacterChangeStep;
    }

    protected void OnChooseCharacterChangeStep()
    {
        //Debug.Log("set active");
        switch (ChooseCharacterManager.Instance.chooseCharacterStep)
        {
            case ChooseCharacterStep.P1:
                ObjectSetEnable(character == Character.Player);
                MoveAnimation(character == Character.Enemy);
                break;
            case ChooseCharacterStep.P2:
                ObjectSetEnable(character == Character.Enemy);
                MoveAnimation(character == Character.Player);
                break;
            case ChooseCharacterStep.Ready:
                ObjectSetEnable(false);
                MoveAnimation(true);
                break;
        }
    }


    protected ChooseCharacterDetails_SO OnSendCharacterDetails()
    {
        //Debug.Log(gameObject.name);
        return currentShowCharacter;
    }

    #endregion
    
    /// <summary>
    /// According to 'character_SO' to show different character details on UI
    /// </summary>
    protected void ReloadCharacterShow()
    {
        characterName.text = currentShowCharacter.characterName;
        characterImage.sprite = currentShowCharacter.characterSprite;
        characterDescription.text = currentShowCharacter.characterDescription;
        
    }

    private void ObjectSetEnable(bool isEnable)
    {
        characterName.gameObject.SetActive(isEnable);
        chooseBar.SetActive(isEnable);
        characterDescription.gameObject.SetActive(isEnable);
        //Debug.Log(gameObject.name);
    }
    private void MoveAnimation(bool isDisappear)
    {
        Sequence sequence = DOTween.Sequence();
        float toX = disappearPoint.transform.localPosition.x;
        //float origin = GetComponent<RectTransform>().
        float time = 0.4f;

        sequence.Append(setActiveParent.transform.DOLocalMoveX(isDisappear ? toX : 0, time));
        //sequence.Append(characterName.transform.DOMoveX(toX, time));
        //sequence.Join(characterImage.transform.DOMoveX(toX, time));
        //sequence.Join(chooseBar.transform.DOMoveX(toX, time));
        
        
    }
}
