using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChild : TestBase
{
    public void ButtonTest(CardDetail_SO data)
    {
        GameManager.Instance.ValueListToInt(data.attackTypeDetails.cardHurtHPCalc);
    }
}
