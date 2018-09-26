using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Map))]
public class MapInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Map Editor"))
        {
            this.OpenEditorWindow();
        };
    }
    public void OnSceneGUI()
    {

    }
    /// <summary>
    ///  打开编辑器窗口
    /// </summary>
    void OpenEditorWindow()
    {
        // Get existing open window or if none, make a new one:
        MapEditor window = (MapEditor)EditorWindow.GetWindow(typeof(MapEditor));
        window.Show();
        window.map = (Map)target;
    }
    /// <summary>
    /// 读取地图
    /// </summary>
    void LoadMap()
    {
        
    }
    /// <summary>
    /// 保存地图
    /// </summary>
    void SaveMap()
    {
        
    }
}