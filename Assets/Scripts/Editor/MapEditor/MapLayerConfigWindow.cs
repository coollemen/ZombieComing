using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MapLayerConfigWindow : EditorWindow {
    public Map map;
    public string layerName = "";
    public GUISkin editorSkin;
    public GUIStyle layerStyle;
    private void Awake()
    {
        editorSkin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/Scripts/Application/BlockTerrain/MapEditorSkin.guiskin");
        layerStyle = editorSkin.toggle;
    }
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        layerName = GUILayout.TextField(layerName, GUILayout.Width(140));
        if (GUILayout.Button("Add", EditorStyles.toolbarButton))
        {
            if (!string.IsNullOrEmpty(layerName))
            {
                this.map.AddLayer(layerName);
                this.layerName = "";
            }

        }
        if (GUILayout.Button("Del", EditorStyles.toolbarButton))
        {
            this.map.RemoveLayer(map.activeLayer.name);
        }
        if (GUILayout.Button("Close", EditorStyles.toolbarButton))
        {
            this.Close();
        }
        GUILayout.EndHorizontal();
        foreach (var kv in map.layers)
        {

            var flag = GUILayout.Toggle(map.IsActiveLayer(kv.Key), kv.Key, layerStyle);
            if (flag)
            {
                map.SetActiveLayer(kv.Key);
            }
        }
        GUILayout.EndVertical();
    }
}
