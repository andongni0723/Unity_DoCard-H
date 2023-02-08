using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectDetail", menuName = "Game-Assets-Project/EffectDetail", order = 1)]
public class EffectDetail_SO : ScriptableObject
{
    public string effectName;
    public EffectType effectType;
    public Sprite effectIcon;
    public bool isAccumulate;
    public string description;

    [Header("Status Editor")]
    public bool isBehaviorProhibit;
    public BehaviorProhibit behaviorProhibit;

    [Space(15)]
    public bool isAttributeChange;
    public AttributeChange attributeChange;

    [Space(15)]

    public bool isLogicTrigger;
    public LogicTrigger logicTrigger;

}

[System.Serializable]
public class BehaviorProhibit
{
    public Behavior behavior;
}
public enum Behavior
{
    Move, PlayCard
}

[System.Serializable]
public class AttributeChange
{
    public Attribute attributeType;
    public float changeMagnification;
}
public enum Attribute
{
    AttackPower, Hurt
}

[System.Serializable]
public class LogicTrigger
{
    public bool isHurtLogic;
    public HurtLogic hurtLogic;

    [Space(10)]
    public bool isCureLogic;
    public CureLogic cureLogic;
}
[System.Serializable]
public class HurtLogic
{
    public TriggerTimeType triggerTimeType;
    public int hurt;
}
[System.Serializable]
public class CureLogic
{
    public TriggerTimeType triggerTimeType;
    public int cureNum;
}
public enum TriggerTimeType
{
    CharacterStepStart, SelfHurt, EnemyHurt
}