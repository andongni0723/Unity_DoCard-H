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

    [Header("Object Setting")] 
    public Character character;
    
    [Space(5)]
    public GameObject setActiveParent;
    public TextMeshProUGUI characterName;
    public Image characterImage;
    public TextMeshProUGUI characterDescription;
    public GameObject chooseBarObj;
    public GameObject barObj;
    public GameObject disappearPoint;

    [Space(10f)] 
    public GameObject characterBarGrid;
    
    [Header("Scripts Setting")]
    public List<ChooseCharacterDetails_SO> ChooseCharacterDetailsList = new List<ChooseCharacterDetails_SO>();

    

    protected virtual void Start()
    {
        OnChooseCharacterChangeStep();
        LoadChooseCharacterBarElements(ChooseCharacterDetailsList);
    }

    #region Button Method

    public void ChooseCharacter(ChooseCharacterDetails_SO data)
    {
        currentShowCharacter = data;
        ReloadCharacterShow();
    }

    #endregion

    #region Event

    protected void OnEnable()
    {
        //EventHanlder.ChooseCharacterGridButton += OnChooseCharacterGridButton; // After button selected character , display UI
        EventHanlder.ChooseCharacterChangeStep += OnChooseCharacterChangeStep; // Play move animation and change display
    }

    protected void OnDisable()
    {
        //EventHanlder.ChooseCharacterGridButton -= OnChooseCharacterGridButton;
        EventHanlder.ChooseCharacterChangeStep -= OnChooseCharacterChangeStep;
    }

    protected void OnChooseCharacterGridButton(ChooseCharacterDetails_SO data)
    {
        currentShowCharacter = data;
        ReloadCharacterShow();
        Debug.Log("UI");
    }

    protected void OnChooseCharacterChangeStep()
    {
        //Debug.Log("set active");
        switch (ChooseCharacterManager.Instance.chooseCharacterStep)
        {
            case ChooseCharacterStep.P1:
                ObjectSetEnable(character == Character.Player);
                MoveAnimation(character == Character.Enemy || character == Character.AI);
                break;
            case ChooseCharacterStep.P2:
                ObjectSetEnable(character == Character.Enemy || character == Character.AI);
                MoveAnimation(character == Character.Player);
                break;
            case ChooseCharacterStep.Ready:
                ObjectSetEnable(false);
                MoveAnimation(true);
                break;
        }
    }

    protected void LoadChooseCharacterBarElements(List<ChooseCharacterDetails_SO> list)
    {
        foreach (ChooseCharacterDetails_SO data in list)
        {
            GameObject obj = Instantiate(characterBarGrid, barObj.transform);
            obj.GetComponent<ChooseBarElement>().SetDetails(character, data);
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
    private void ReloadCharacterShow()
    {
        characterName.text = currentShowCharacter.characterName;
        characterImage.sprite = currentShowCharacter.characterSprite;
        characterDescription.text = currentShowCharacter.characterDescription;
    }

    private void ObjectSetEnable(bool isEnable)
    {
        characterName.gameObject.SetActive(isEnable);
        chooseBarObj.SetActive(isEnable);
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


    }
}
