﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditor : EditorWindow
{
    public Map map;
    public Color defaultColor;
    public Color labelColor = Color.yellow;
    public GUISkin editorSkin;
    public GUIStyle layerStyle;
    public GUIStyle titleStyle;
    public GUIStyle titleActiveStyle;
    public GUIStyle gridStyle;
    public string groupName = "";
    public int leftViewWidth = 200;
    public Texture2D myTex;
    public TileBrushGroup activeGroup;
    public List<TileBrushGroup> brushGroups = new List<TileBrushGroup>();
    public int selectBrushIndex = 0;
    public int rowCount = 4;
    public int buttonWidth = 80;
    public Texture2D prefabIcon;
    public Vector2 scrollPos = new Vector2();
    public string msg = "";
    public string workMode = "正常模式";
    private void Awake()
    {
        editorSkin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/Scripts/Application/Map/MapEditorSkin.guiskin");
        layerStyle = editorSkin.toggle;
        gridStyle = editorSkin.FindStyle("grid");
        titleStyle= editorSkin.FindStyle("title");
        titleActiveStyle = editorSkin.FindStyle("titleActive");
        prefabIcon = EditorGUIUtility.FindTexture("PrefabNormal Icon");
        this.LoadEditorConfig();
    }

    private void Update()
    {
    }

    void OnGUI()
    {
        if (map == null)
        {
            GUILayout.Label("Not Fount Map Component!");
            return;
        }
        EditorGUILayout.BeginVertical();
        this.DrawToolbar();
        EditorGUILayout.BeginHorizontal();
        this.DrawLeftView();
        this.DrawRightView();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        this.DrawFooter();
        GUILayout.EndVertical();
    }

    private void DrawToolbar()
    {
        Rect toolbarRect = EditorGUILayout.BeginHorizontal();
        this.defaultColor = GUI.contentColor;
        GUI.contentColor = labelColor;
        GUILayout.Label("Current Map:[ " + map.name + " ]", EditorStyles.miniLabel, GUILayout.MinWidth(50));
        GUI.contentColor = defaultColor;
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Laod", EditorStyles.toolbarButton, GUILayout.Width(buttonWidth)))
        {
        }
        if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(buttonWidth)))
        {
        }
        if (GUILayout.Button("Save as", EditorStyles.toolbarButton, GUILayout.Width(buttonWidth)))
        {
        }
        if (GUILayout.Button("Layer Config", EditorStyles.toolbarButton, GUILayout.Width(buttonWidth)))
        {
//         var layerConfigBtnRect = GUILayoutUtility.GetLastRect();
            var layerConfigWin = EditorWindow.GetWindow<MapLayerConfigWindow>();
            layerConfigWin.map = this.map;
            layerConfigWin.Show();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawFooter()
    {
        Rect footerRect= EditorGUILayout.BeginHorizontal();
        if (activeGroup != null&&activeGroup.brushes.Count>0)
        {
            GUILayout.Space(20);
            var iconRect = GUILayoutUtility.GetLastRect();
            iconRect.x = 4;
            iconRect.height = 16;
            iconRect.width = 16;
            if (prefabIcon == null)
            {
                prefabIcon = EditorGUIUtility.FindTexture("PrefabNormal Icon");
            }
            GUI.DrawTexture(iconRect, prefabIcon);
            GUILayout.Label(activeGroup.brushes[selectBrushIndex].prefabPath);
        }

        GUILayout.FlexibleSpace();
        rowCount = EditorGUILayout.IntSlider(rowCount, 4, 20);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawLeftView()
    {
        GUILayout.BeginVertical(GUILayout.Width(leftViewWidth));
        GUILayout.BeginHorizontal();
        groupName = GUILayout.TextField(groupName, GUILayout.Width(140));
        if (GUILayout.Button("Add", EditorStyles.toolbarButton))
        {
            if (!string.IsNullOrEmpty(groupName))
            {
                var group = new TileBrushGroup(groupName);
                this.brushGroups.Add(group);
                this.activeGroup = group;
                this.groupName = "";
                this.selectBrushIndex = 0;
            }
        }
        if (GUILayout.Button("Del", EditorStyles.toolbarButton))
        {
            this.brushGroups.Remove(activeGroup);
        }
        GUILayout.EndHorizontal();
        foreach (var g in brushGroups)
        {
            var flag = GUILayout.Toggle(g == activeGroup, g.name, layerStyle);
            if (flag)
            {
                if (this.activeGroup != g)
                {
                    this.activeGroup = g;
                    this.selectBrushIndex = 0;
                }
            }
        }
        GUILayout.EndVertical();
    }

    private void DrawRightView()
    {
        Rect rect = EditorGUILayout.BeginVertical();
        rect.width = this.position.width - leftViewWidth;
        rect.height = this.position.height - 20*2;
        //绘制背景
        EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f));
        //绘制笔刷组
        scrollPos= GUILayout.BeginScrollView(scrollPos);
//        GUILayout.Box("",GUILayout.Width(100),GUILayout.Height(100));
//        if (myTex)
//        {
//            Rect textRect = GUILayoutUtility.GetLastRect();
//            Debug.Log(textRect);
//            EditorGUI.DrawPreviewTexture(textRect,myTex);
//        }
        this.DrawActiveBrushGroup();
        GUILayout.EndScrollView();
        var e = Event.current;
        if (e.type == EventType.ContextClick)
        {
            if (rect.Contains(e.mousePosition))
            {
                var menu= new GenericMenu();

                menu.AddItem(new GUIContent("Delete Selected Brush"), false,this.DeleteSelectedBrush);
                menu.AddItem(new GUIContent("Delete All Brushes"), false, this.DeleteAllBrushes);
                menu.ShowAsContext();

                e.Use();
            }
        }
        if (e.type == EventType.DragUpdated || e.type == EventType.DragPerform)
        {
            if (rect.Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                //如果拖入了拖拽区
                if (e.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                    {
                        var obj = DragAndDrop.objectReferences[i];
                        TileBrush brush = new TileBrush(obj.name);
                        brush.prefabPath = DragAndDrop.paths[i];
                        brush.texture = AssetPreview.GetAssetPreview(obj);
                        if (activeGroup == null)
                        {
                            var defaultGroup = new TileBrushGroup("default");
                            this.brushGroups.Add(defaultGroup);
                            this.activeGroup = defaultGroup;
                        }
                        this.activeGroup.brushes.Add(brush);
                    }
                }
            }
            Event.current.Use();
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawActiveBrushGroup()
    {
        List<GUIContent> contents = new List<GUIContent>();
        if (activeGroup == null) return;
        foreach (var b in activeGroup.brushes)
        {
            if (b.texture == null)
            {
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(b.prefabPath);
                b.texture = AssetPreview.GetAssetPreview(go);
            }
            contents.Add(new GUIContent(b.name.Substring(0, 6) + "...", b.texture));
        }
        selectBrushIndex = GUILayout.SelectionGrid(selectBrushIndex, contents.ToArray(), rowCount, gridStyle);
    }

    private void DeleteSelectedBrush()
    {
        this.activeGroup.brushes.Remove(this.activeGroup.brushes[selectBrushIndex]);
    }

    private void DeleteAllBrushes()
    {
        this.activeGroup.brushes.Clear();
    }
    private void FindMap()
    {
        GameObject mapObj = GameObject.FindGameObjectWithTag("GameMap");
        if (mapObj != null && mapObj.GetComponent<Map>() != null)
        {
            this.map = mapObj.GetComponent<Map>();
        }
        Debug.Log(map.name);
    }

    private void CreateMapObjectAtMousePosition(Vector3 mousePos)
    {
        var brush = this.activeGroup.brushes[selectBrushIndex];
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(brush.prefabPath);
        var go = Instantiate(prefab,mousePos,Quaternion.identity);
        var mapRoot = GameObject.FindGameObjectWithTag("GameMap");
        go.transform.parent = mapRoot.transform;
    }

    public void SaveEditorConfig()
    {
        Debug.Log("save config");
        string path = "Assets/MapEditorSettings.asset";
        MapEditorSettings settings = AssetDatabase.LoadAssetAtPath<MapEditorSettings>(path);
        if (settings == null)
        {
            settings = ScriptableObject.CreateInstance<MapEditorSettings>();
            settings.brushGroups = this.brushGroups;
            settings.resPath = path;
            AssetDatabase.CreateAsset(settings, path);
        }
        else
        {
            settings.brushGroups = this.brushGroups;
            settings.resPath = path;
            AssetDatabase.SaveAssets();
        }

    }

    public void LoadEditorConfig()
    {
        Debug.Log("load config");
        string path = "Assets/MapEditorSettings.asset";
        MapEditorSettings settings = AssetDatabase.LoadAssetAtPath<MapEditorSettings>(path);
        if(settings!=null)
        this.brushGroups = settings.brushGroups;
    }
    private void OnDestroy()
    {
        this.SaveEditorConfig();
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }
    private void OnLostFocus()
    {
        //this.SaveEditorConfig();
    }
    private void OnFocus()
    {
        this.LoadEditorConfig();
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
        Repaint();

    }
    public void OnSceneGUI(SceneView sceneView)
    {
        var e = Event.current;
        GUILayout.BeginArea(new Rect(6, 6, 300, 300));
        Color oldColor = GUI.contentColor;
        Color oldBgColor = GUI.backgroundColor;
        if (e.control)
        {
            workMode = "绘制模式";
            GUI.backgroundColor = Color.red;
            GUI.contentColor = Color.white;
            GUILayout.Box(workMode, "GroupBox", GUILayout.Width(80));
            GUI.backgroundColor = oldBgColor;
            GUI.contentColor =oldColor;
        }
        else
        {
            workMode = "正常模式";
            GUILayout.Box(workMode, "GroupBox", GUILayout.Width(80));
        }
        GUILayout.EndArea();
        if (e.type == EventType.MouseDown && e.control)
        {
            var mousePos = GetWorldPosition(sceneView,map.transform);
            CreateMapObjectAtMousePosition(mousePos);
            e.Use();
        }
    }
    private Vector3 GetWorldPosition(SceneView sceneView, Transform parent)
    {
        Camera cam = sceneView.camera;

        Vector3 mousepos = Event.current.mousePosition;
        float mult = EditorGUIUtility.pixelsPerPoint;
        mousepos.y = sceneView.camera.pixelHeight - mousepos.y * mult;
        mousepos.x *= mult;
        RaycastHit hit;
        Ray ray = sceneView.camera.ScreenPointToRay(mousepos);
        if (Physics.Raycast(ray, out hit))
        {
            mousepos = hit.point;
        }
        //        mousepos.z = cam.worldToCameraMatrix.MultiplyPoint(parent.position).z;
//        mousepos.y = cam.pixelHeight - mousepos.y;
//        mousepos = sceneView.camera.ScreenToWorldPoint(mousepos);
//        mousepos.y =0;
        return mousepos;
    }
    /// <summary>
    /// 创建地形底层，用于鼠标点击定位
    /// </summary>
    /// <returns></returns>
    private GameObject CreateTerrainPanel()
    {
        GameObject terrain = GameObject.CreatePrimitive(PrimitiveType.Plane);
        terrain.transform.localScale = new Vector3(map.mapLength / 10, 1, map.mapWidth / 10);
        return terrain;
    }
    #region 初始化
    [MenuItem("GameDesign/Map Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MapEditor window = (MapEditor) EditorWindow.GetWindow(typeof(MapEditor));
        window.Show();
        GameObject mapObj = GameObject.FindGameObjectWithTag("GameMap");
        if (mapObj != null && mapObj.GetComponent<Map>() != null)
        {
            window.map = mapObj.GetComponent<Map>();
        }
        SceneView.onSceneGUIDelegate += window.OnSceneGUI;
    }


    #endregion
}