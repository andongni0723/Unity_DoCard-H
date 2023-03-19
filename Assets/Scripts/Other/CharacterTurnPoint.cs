using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurnPoint : MonoBehaviour
{
    private Character parentCharacter;

    private void Start()
    {
        parentCharacter = transform.parent.GetComponent<BaseCharacter>().character;
        
        gameObject.SetActive(parentCharacter == GameManager.Instance.currentCharacter);
    }

    #region Event

    private void OnEnable()
    {
        EventHanlder.OnPlayerStep += OnOnPlayerStep; // Check self active
        EventHanlder.OnEnemyStep += OnOnEnemyStep; // Check self active
    }

    private void OnDisable()
    {
        EventHanlder.OnPlayerStep += OnOnPlayerStep;
        EventHanlder.OnEnemyStep += OnOnEnemyStep;
    }

    private void OnOnEnemyStep()
    {
        gameObject.SetActive(parentCharacter == GameManager.Instance.currentCharacter);
    }
    private void OnOnPlayerStep()
    {
        gameObject.SetActive(parentCharacter == GameManager.Instance.currentCharacter);
    }
    #endregion
    
    
}
