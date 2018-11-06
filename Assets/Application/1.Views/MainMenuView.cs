using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
public class MainMenuView : UIView {
    public Button newGameButton;
    public Button loadGameButton;
    public Button gameConfigButton;
    public Button exitGameButton;
    public override string TypeID
    {
        get { return "MainMenuView"; }
    }
    public override void Init()
    {
        base.Init();
        this.Context = new MainMenuContext();
        this.Context.Init();
    }
    public override void DataBinding()
    {
        base.DataBinding();
        var c = this.Context as MainMenuContext;
        newGameButton.OnClickAsObservable().Subscribe(_=> c.OnNewGame());
        loadGameButton.OnClickAsObservable().Subscribe(_ => c.OnLoadGame());
        gameConfigButton.OnClickAsObservable().Subscribe(_ => c.OnGameConfig());
        exitGameButton.OnClickAsObservable().Subscribe(_ => c.OnExitGame());
    }
}
