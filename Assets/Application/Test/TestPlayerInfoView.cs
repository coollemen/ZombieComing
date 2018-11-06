using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
public class TestPlayerInfoView : MonoBehaviour {
    public PlayerInfoContext context;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnGUI()
    {
        if (GUILayout.Button("Level UP"))
        {
            if (context == null)
            {
                context = UIManager.Instance.GetView<PlayerInfoView>().Context as PlayerInfoContext;
            }
            context.playerLv ++;
        }
        if (GUILayout.Button("Show Window"))
        {
            UIManager.Instance.ShowView<PlayerInfoView>("PlayerInfoView");
        }
        if (GUILayout.Button("Hide Window"))
        {
            UIManager.Instance.HideView("PlayerInfoView");
        }
        if (GUILayout.Button("Show Menu"))
        {
            UIManager.Instance.ShowView<MainMenuView>();
        }
        if (GUILayout.Button("Hide Menu"))
        {
            UIManager.Instance.HideView<MainMenuView>();
        }
        if (GUILayout.Button("Show MessageBox"))
        {
            UIManager.Instance.ShowView<MessageView>();
        }
        if (GUILayout.Button("Hide MessageBox"))
        {
            UIManager.Instance.HideView<MessageView>();
        }
        if (UIManager.Instance.GetView<MessageView>() != null)
        {
            var c = UIManager.Instance.GetView<MessageView>().Context as MessageContext;
            if(c!=null)
            c.message = GUILayout.TextField(c.message);
        }
        if (GUILayout.Button("Show OtherMessageBox"))
        {
            UIManager.Instance.ShowView<MessageView>("TestMessage");
        }
        if (GUILayout.Button("Hide OntherMessageBox"))
        {
            UIManager.Instance.HideView("TestMessage");
        }
    }
}
