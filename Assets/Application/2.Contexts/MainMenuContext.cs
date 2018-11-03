using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class MainMenuContext : UIContext {

    public override string TypeID
    {
        get
        {
            return "MainMenuContext";
        }
    }
    public void OnNewGame()
    {
        Debug.Log("New Game");
    }

    public void OnLoadGame()
    {
        Debug.Log("Load Game");
    }

    public void OnGameConfig()
    {
        Debug.Log("Game Config");
    }

    public void OnExitGame()
    {
        Debug.Log("Exit Game");
    }
}
