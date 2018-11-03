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
    public override string ContextID
    {
        get
        {
            return "MainMenuContext";
        }
    }
    public override void DataBinding()
    {
        base.DataBinding();
        var context = UIManager.Instance.GetContext<MainMenuContext>() as MainMenuContext;
        newGameButton.OnClickAsObservable().Subscribe(_=>context.OnNewGame());
        loadGameButton.OnClickAsObservable().Subscribe(_ => context.OnLoadGame());
        gameConfigButton.OnClickAsObservable().Subscribe(_ => context.OnGameConfig());
        exitGameButton.OnClickAsObservable().Subscribe(_ => context.OnExitGame());
    }
}
