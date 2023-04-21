using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : Singleton<ModeManager>
{
    public GameMode gameMode;

    public void ChangeGameMode(GameMode mode)
    {
        gameMode = mode;
    }
}
