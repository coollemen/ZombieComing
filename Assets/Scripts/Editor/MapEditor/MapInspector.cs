using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map))]
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
//        var e = Event.current;
//        Handles.BeginGUI();
//        Color oldColor = GUI.contentColor;
//        Color oldBgColor = GUI.backgroundColor;
//        var workMode = "正常模式";
//        GUI.Label(new Rect(6, 6, 300, 300), workMode);
//        if (e.control)
//        {
//            workMode = "绘制模式";
//            GUI.backgroundColor = Color.red;
//            GUI.contentColor = Color.white;
//            GUILayout.Box(workMode, "GroupBox", GUILayout.Width(80));
//            GUI.backgroundColor = oldBgColor;
//            GUI.contentColor = oldColor;
//        }
//        else
//        {
//            workMode = "正常模式";
//            GUILayout.Box(workMode, "GroupBox", GUILayout.Width(80));
//        }
//        Handles.EndGUI();
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