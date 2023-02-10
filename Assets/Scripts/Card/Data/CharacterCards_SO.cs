using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cards", menuName = "Game-Assets-Project/Cards", order = 2)]
public class CharacterCards_SO : ScriptableObject
{
    public List<CardDetail_SO> Cards;
}
