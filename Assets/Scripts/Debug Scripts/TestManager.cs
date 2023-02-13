using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public GameObject mainTestObj;

    public void ButtonAction()
    {
        mainTestObj.GetComponentInChildren<TestBase>().SayHi();
    }
}
