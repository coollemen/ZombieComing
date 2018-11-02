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
                context = UIManager.Instance.GetContext<PlayerInfoContext>() as PlayerInfoContext;
            }
            context.playerLv ++;
        }
        if (GUILayout.Button("Show Window"))
        {
            UIManager.Instance.ShowView("PlayerInfoView");
        }
        if (GUILayout.Button("Hide Window"))
        {
            UIManager.Instance.HideView("PlayerInfoView");
        }
    }
}
