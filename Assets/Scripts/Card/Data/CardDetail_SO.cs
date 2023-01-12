using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDetail", menuName = "Game-Assets-Project/CardDetail", order = 0)]
public class CardDetail_SO : ScriptableObject {
    public int payCardNum = 0;
    public Vector2 cardOffset = new Vector2();
}