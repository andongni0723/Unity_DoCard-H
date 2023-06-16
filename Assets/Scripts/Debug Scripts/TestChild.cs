using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChild : TestBase
{
    public int testInt = 7;
    
    public void ButtonTest(CardDetail_SO data)
    {
        GameManager.Instance.ValueListToInt(data.attackTypeDetails.cardHurtHPCalc);
    }

    #region Event

    private void OnEnable()
    {
        EventHanlder.CardUpdatePosition += OnCardUpdatePosition;   
    }

    private void OnDisable()
    {
        EventHanlder.CardUpdatePosition -= OnCardUpdatePosition;
    }

    private void OnCardUpdatePosition()
    {
        Debug.Log("card");
    }

    #endregion 
    
    public static event Action Test;

    public static void CallTest()
    {
        Test?.Invoke();
    }
}


