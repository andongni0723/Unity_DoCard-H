using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurnPoint : MonoBehaviour
{
    [SerializeField] private Character parentCharacter;

    [Header("Component")] 
    public GameObject pointObject;

    private void Start()
    {
        parentCharacter = transform.parent.GetComponent<BaseCharacter>().character;
        
        pointObject.SetActive(parentCharacter == GameManager.Instance.currentCharacter);
    }

    #region Event

    private void OnEnable()
    {
        EventHanlder.OnPlayerStep += OnOnPlayerStep; // Check self active
        EventHanlder.OnEnemyStep += OnOnEnemyStep; // Check self active
    }

    private void OnDisable()
    {
        EventHanlder.OnPlayerStep -= OnOnPlayerStep;
        EventHanlder.OnEnemyStep -= OnOnEnemyStep;
    }

    private void OnOnEnemyStep()
    {
        pointObject.SetActive(parentCharacter == GameManager.Instance.currentCharacter);
    }
    private void OnOnPlayerStep()
    {
        pointObject.SetActive(parentCharacter == GameManager.Instance.currentCharacter);
    }
    #endregion
    
    
}
