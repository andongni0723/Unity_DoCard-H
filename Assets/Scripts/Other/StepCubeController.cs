using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepCubeController : MonoBehaviour
{
    public GameObject playerCube;
    public GameObject enemyCube;

    #region Event
    private void OnEnable() {
        EventHanlder.OnPlayerStep += OnPlayerStep; // player cube open
        EventHanlder.OnEnemyStep += OnEnemyStep; // enemy cube open
    }
    private void OnDisable() {
        EventHanlder.OnPlayerStep -= OnPlayerStep;
        EventHanlder.OnEnemyStep -= OnEnemyStep;
    }

    private void OnPlayerStep()
    {
        playerCube.SetActive(true);
        enemyCube.SetActive(false);
    }
    private void OnEnemyStep()
    {
        playerCube.SetActive(false);
        enemyCube.SetActive(true);
    }

    #endregion

}
