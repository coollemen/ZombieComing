using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// 方块地图编辑窗口
/// </summary>
public class BlockMapDesignerWindow : EditorWindow {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnGUI()
    {
        GUILayout.Label("Block Map Editor");
    }
    [MenuItem("Game Framework/Block Map Editor")]
    static void ShowWindow()
    {
        var win = EditorWindow.GetWindow<BlockMapDesignerWindow>();
        win.Show();
    }
}
