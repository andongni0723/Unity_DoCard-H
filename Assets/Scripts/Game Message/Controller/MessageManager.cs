using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public GameObject messagePrefabs;

    #region Event
    private void OnEnable()
    {
        EventHanlder.SendGameMessage += OnSendGameMessage;
    }
    private void OnDisable()
    {
        EventHanlder.SendGameMessage -= OnSendGameMessage;
    }
    public void OnSendGameMessage(string message)
    {
        GameObject startPoint = GameObject.FindWithTag("messageStartPoint");
        GameObject obj = Instantiate(messagePrefabs, startPoint.transform.position, Quaternion.identity, transform) as GameObject;

        obj.GetComponent<GameMessage>().PlayMessage(message);
    }
    #endregion
}
