using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Serialization;

public class ChooseCharacterManager : MonoBehaviour
{
    public TextMeshProUGUI characterText;
    public Image characterImage;
    
    
    public ChooseCharacterStep chooseCharacterStep;

    [Header("Setting")] 
    public GameObject bg;
    public GameObject bigPoint;
    public GameObject middlePoint;
    public GameObject smallPoint;

    public float moveDuration = 0.4f;

    private void Start()
    {
        ChangeStep(ChooseCharacterStep.P1);
    }
    
    #region Button Method
    
    public void ChangeNext()
    {
        ChangeStep(1);
    }
    
    public void ChangeLast()
    {
        ChangeStep(-1);
    }
    #endregion
    
    private void ChangeStep(ChooseCharacterStep to)
    {
        chooseCharacterStep = to;
        ReloadStepAction();
    }
    private void ChangeStep(int value)
    {
        chooseCharacterStep += value;

        // Check if the enum index out
        if ((int)chooseCharacterStep > 3)
            chooseCharacterStep = ChooseCharacterStep.Go;
        else if ((int)chooseCharacterStep < 0)
            chooseCharacterStep = ChooseCharacterStep.P1;
        
        ReloadStepAction();
    }
    
    /// <summary>
    /// Execute Action about different step
    /// </summary>
    private void ReloadStepAction()
    {
        switch (chooseCharacterStep)
        {
            case ChooseCharacterStep.P1:
                bg.transform.DOMoveX(bigPoint.transform.position.x, moveDuration);
                break;
            case ChooseCharacterStep.P2:
                bg.transform.DOMoveX(smallPoint.transform.position.x, moveDuration);
                Debug.Log("p2");
                break;
            case ChooseCharacterStep.Ready:
                bg.transform.DOMoveX(middlePoint.transform.position.x, moveDuration);
                break;
            case ChooseCharacterStep.Go:
                break;
        }
    }
}
