using System.Collections;
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
    public Vector3 selectedCellPosition = new Vector3();
    public string selectedCellMsg = "";
    public Vector3 rayOriginPos = new Vector3();
    public Vector3 rayTargetPos = new Vector3();
    public Vector3 mouseWorldPos = new Vector3();
    public int repaintCount = 0;
    public bool isDrawMode = false;

    private void Awake()
    {
        editorSkin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/Scripts/Application/Map/MapEditorSkin.guiskin");
        layerStyle = editorSkin.toggle;
        gridStyle = editorSkin.FindStyle("grid");
        titleStyle = editorSkin.FindStyle("title");
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
        Rect footerRect = EditorGUILayout.BeginHorizontal();
        if (activeGroup != null && activeGroup.brushes.Count > 0)
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
        rect.height = this.position.height - 20 * 2;
        //绘制背景
        EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f));
        //绘制笔刷组
        scrollPos = GUILayout.BeginScrollView(scrollPos);
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
                var menu = new GenericMenu();

                menu.AddItem(new GUIContent("Delete Selected Brush"), false, this.DeleteSelectedBrush);
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

    private void CreateMapObjectAtActiveCell()
    {
        var brush = this.activeGroup.brushes[selectBrushIndex];
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(brush.prefabPath);
        var go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        map.AddGameObjectToActiveCell(go);
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
        if (settings != null)
            this.brushGroups = settings.brushGroups;
    }

    private void OnDestroy()
    {
        this.SaveEditorConfig();
    }

    private void OnLostFocus()
    {
        //this.SaveEditorConfig();
    }
    void OnEnable()
    {
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    private void OnFocus()
    {
        this.LoadEditorConfig();
        Repaint();
    }

    public void OnSceneGUI(SceneView sceneView)
    {
        var e = Event.current;
        Handles.BeginGUI();
        if (e.type == EventType.KeyUp && e.keyCode == KeyCode.LeftControl)
        {
            isDrawMode = !isDrawMode;
            if (isDrawMode == false)
            {
                map.activeCell = null;
            }
        }
        Color oldColor = GUI.contentColor;
        Color oldBgColor = GUI.backgroundColor;
        if (isDrawMode)
        {
            workMode = "绘制模式[left ctrl切换]";
            GUI.backgroundColor = Color.red;
            GUI.contentColor = Color.white;
            GUILayout.Box(workMode, "GroupBox", GUILayout.Width(140));
            GUI.backgroundColor = oldBgColor;
            GUI.contentColor = oldColor;
        }
        else
        {
            workMode = "正常模式[left ctrl切换]";
            GUILayout.Box(workMode, "GroupBox", GUILayout.Width(140));
        }
        Handles.EndGUI();
        if (isDrawMode)
        {
            if (e.type == EventType.MouseDown)
            {
                mouseWorldPos = GetWorldPositionFromMousePosition(e.mousePosition, Vector3.up, new Vector3(1, 0, 1));
                int row;
                int col;
                if (map.IsEnterCell(mouseWorldPos, out row, out col))
                {
                    if (e.button == 0)
                    {
                        CreateMapObjectAtActiveCell();
                    }
                    else if (e.button == 1)
                    {
                        map.RemoveGameObjectFromActiveCell();
                    }
                    e.Use();
                }
            }
            else if (e.type == EventType.MouseDrag)
            {
                mouseWorldPos = GetWorldPositionFromMousePosition(e.mousePosition, Vector3.up, new Vector3(1, 0, 1));
                int row;
                int col;
                if (map.IsEnterCell(mouseWorldPos, out row, out col))
                {
                    if (e.button == 0)
                    {
                        CreateMapObjectAtActiveCell();
                    }
                    else if (e.button == 1)
                    {
                        map.RemoveGameObjectFromActiveCell();
                    }
                    e.Use();
                }
            }
            else if (e.type == EventType.MouseMove)
            {
                mouseWorldPos = GetWorldPositionFromMousePosition(e.mousePosition, Vector3.up, new Vector3(1, 0, 1));
                int row;
                int col;
                if (map.IsEnterCell(mouseWorldPos, out row, out col))
                {

                }
            }
        }
    }

    /// <summary>
    /// 获取鼠标点击的Map Grid的点的世界坐标
    /// </summary>
    /// <param name="mousePosition">鼠标位置</param>
    /// <param name="panelNormal">Map Grid Panel法线</param>
    /// <param name="panelPoint">Map Grid Panel 上任意点</param>
    /// <returns></returns>
    private Vector3 GetWorldPositionFromMousePosition(Vector2 mousePosition, Vector3 panelNormal, Vector3 panelPoint)
    {
        //获取鼠标点击射线
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(mousePosition);
        //计算射线与屏幕的交点
        var intersectPoint = GetIntersectWithLineAndPlane(mouseRay.origin, mouseRay.direction, panelNormal, panelPoint);
//        Debug.Log("交点" + intersectPoint.ToString());
        return intersectPoint;
    }

    /// <summary> 计算直线与平面的交点 </summary>  
    /// <param name="point">直线上某一点</param>   
    /// <param name="direct">直线的方向</param>   
    /// <param name="planeNormal">垂直于平面的的向量</param>   
    /// <param name="planePoint">平面上的任意一点</param>   
    /// <returns></returns>  
    private Vector3 GetIntersectWithLineAndPlane(Vector3 point, Vector3 direct, Vector3 planeNormal, Vector3 planePoint)
    {
        float d = Vector3.Dot(planePoint - point, planeNormal) / Vector3.Dot(direct.normalized, planeNormal);
        return d * direct.normalized + point;
    }

    #region 初始化

    [
        MenuItem("GameDesign/Map Editor")]
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
    }

    #endregion
}