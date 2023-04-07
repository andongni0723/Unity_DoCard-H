using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [SceneName]public string formScene;
    [SceneName] public string toScene;
    public void LeaveScene()
    {
        TeleportManager.Instance.Transition(SceneManager.GetActiveScene().name, toScene);
    }
}
