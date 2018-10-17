using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
public class MapDesignerWindow :EditorWindow {
    BlockMap map;
    public List<BlockBrush> brushes = new List<BlockBrush>();
    public int selectedIndex = 0;
    public Vector2 scrollPosition = Vector2.zero;
    public GUISkin editorSkin;
    public GUIStyle gridStyle;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void Awake()
    {
        LoadConfig();
        editorSkin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/Scripts/Application/Map/MapEditorSkin.guiskin");
        gridStyle = editorSkin.FindStyle("grid");

    }

    private void OnGUI()
    {

        Rect rect = new Rect(0,0,position.width,position.height);
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
                        BlockBrush brush = new BlockBrush(obj.name);
                        brush.prefab = DragAndDrop.paths[i];
                        brush.preview = AssetPreview.GetAssetPreview(obj);
                        this.brushes.Add(brush);
                    }
                    this.selectedIndex = 0;
                    BlockBrush.activeBrush = brushes[selectedIndex];
                }
            }
            Event.current.Use();
        }
        List<GUIContent> contents = new List<GUIContent>();
        if (this.brushes.Count == 0)
        {
            EditorGUILayout.HelpBox("拖住Prefab到这里", MessageType.Info);
        }
        for (int i = 0; i < brushes.Count; i++)
        {
            if (brushes[i].preview == null)
            {
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(brushes[i].prefab);
                brushes[i].preview = AssetPreview.GetAssetPreview(go);
            }
            contents.Add(new GUIContent(brushes[i].preview));
        }
        scrollPosition= GUILayout.BeginScrollView(scrollPosition);
        int xcount = Mathf.FloorToInt(rect.width / 64);
        int ycount = Mathf.CeilToInt(brushes.Count / xcount);
        int tempIndex = GUILayout.SelectionGrid(selectedIndex, contents.ToArray(),xcount , gridStyle,GUILayout.Width(64*xcount),GUILayout.Height(64*ycount));
        if (tempIndex != selectedIndex)
        {
            selectedIndex = tempIndex;
            BlockBrush.activeBrush = brushes[selectedIndex];
        }
        GUILayout.EndScrollView();


    }

    public void DeleteSelectedBrush()
    {
        this.brushes.RemoveAt(selectedIndex);
    }

    public void DeleteAllBrushes()
    {
        this.brushes.Clear();
    }

    public void SaveConfig()
    {
        string configtext =JsonConvert.SerializeObject(brushes);
        Debug.Log(configtext);
        EditorUserSettings.SetConfigValue("MapDesignerConfig",configtext);
        Debug.Log("Save Config");
    }

    public void LoadConfig()
    {
        string configtext = EditorUserSettings.GetConfigValue("MapDesignerConfig");

        this.brushes = JsonConvert.DeserializeObject<List<BlockBrush>>(configtext);
        Debug.Log("Load Config");
    }
    private void OnDisable()
    {
        SaveConfig();
    }
    private void OnDestroy()
    {
        SaveConfig();
    }
    #region 初始化

    [
        MenuItem("Game Designer/Visual Map Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MapDesignerWindow window = (MapDesignerWindow)EditorWindow.GetWindow(typeof(MapDesignerWindow),false,"Map Designer");
        window.Show();
        GameObject mapObj = GameObject.FindGameObjectWithTag("GameMap");
        if (mapObj != null && mapObj.GetComponent<BlockMap>() != null)
        {
            window.map = mapObj.GetComponent<BlockMap>();
        }
    }

    #endregion
}
