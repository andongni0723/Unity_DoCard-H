using System.Collections;
using System.Collections.Generic;

// Class
[System.Serializable]
public class ConfirmGrid
{
    public int GridX;
    public int GridY;
    public Character GridForCharacter;
    // private ConfirmGrid gridID;

    // public ConfirmGrid(ConfirmGrid gridID)
    // {
    //     this.gridID = gridID;
    // }
}


[System.Serializable]
public class ConfirmAreaGridData
{
    public List<ConfirmGrid> ConfirmGridsList = new List<ConfirmGrid>();
    public Character targetGridForCharacter;
    public CardDetail_SO cardDetail;
}

[System.Serializable]
public class Effect
{
    public Character target;
    public EffectDetail_SO effectData;
    public int effectCount;

    public Effect(Character target, EffectDetail_SO effectData, int effectLayerNum)
    {
        this.target = target;
        this.effectData = effectData;
        this.effectCount = effectLayerNum;
    }
}

// Enum
public enum GameStep
{
    StepStart, PlayerStep, CommonStep, PayCardStep, EnemySettlement, EnemyStep, AIStep, PlayerSettlement, StepEnd
}

public enum GameMode
{
    Menu, PVP, PVE
}

public enum Character
{
    Player, Enemy, AI,  Self
}

public enum CardType
{
    Move, Tank, Attack
}

public enum CardUseStep
{
    CommondStep, SettlementStep
}
public enum EffectType
{
    Poisoned, Bleed, Imprison, Invincibility, Cure
}

public enum ChooseCharacterStep
{
    P1, P2, Ready, Go
}

public enum MoveType
{
    Forward, Back, UpAndDown, Free
}