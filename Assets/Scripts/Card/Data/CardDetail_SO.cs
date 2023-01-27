using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDetail", menuName = "Game-Assets-Project/CardDetail", order = 0)]
public class CardDetail_SO : ScriptableObject
{
    public CardType cardType;

    [Header("Game View")]
    public string cardName;
    public Sprite cardSkillSprite;
    public int payCardNum = 0;
    public string cardDestription;
    public GameStep cardUseGameStep;

    [Header("Card Data")]
    public AttackTypeDetails attackTypeDetails;
    public TankTypeDetails tankTypeDetails;
    public MoveTypeDetails moveTypeDetails;
    
}


[System.Serializable]
public class AttackTypeDetails
{
    public int cardHurtHP;
    public Vector2 cardAttackOffset = new Vector2();
    public List<Effect> CardEffectList = new List<Effect>();
    public List<CardDetail_SO> CardInstantiateCardList = new List<CardDetail_SO>();
}

[System.Serializable]
public class TankTypeDetails
{
    public int addArmor;
    public int addHealth;
    public List<CardDetail_SO> CardInstantiateCardList = new List<CardDetail_SO>(); 
}

[System.Serializable]
public class MoveTypeDetails
{
    public MoveType moveType;
    public Vector2 cardMoveOffset = new Vector2();
}