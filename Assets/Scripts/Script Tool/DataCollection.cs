using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class ConfirmGrid
{
    public int GridX;
    public int GridY;
    private ConfirmGrid gridID;

    public ConfirmGrid(ConfirmGrid gridID)
    {
        this.gridID = gridID;
    }
}


[System.Serializable]
public class ConfirmAreaGridData
{
    public List<ConfirmGrid> ConfirmGridsList = new List<ConfirmGrid>();
    public CardDetail_SO cardDetail;
}

public enum GameStep
{
    StepStart, PlayerStep, CommonStep, PayCardStep, EnemySettlement, EnemyStep, AIStep, PlayerSettlement, StepEnd
}