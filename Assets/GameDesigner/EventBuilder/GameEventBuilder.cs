using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameEventBuilder : EditorWindow
{
    /// <summary>
    /// 配置数据
    /// </summary>
    public GameEventsConfig config;

    /// <summary>
    /// 事件组，一个组就是一个事件枚举类
    /// </summary>
    public string[] groupNames;

    /// <summary>
    /// 当前选择的组合索引
    /// </summary>
    public int selectedGroupIndex = 0;
    /// <summary>
    /// 临时事件定义
    /// </summary>
    public GameEventDef tempEventDef;
    /// <summary>
    /// 事件组
    /// </summary>
    public GameEventGroup currGroup;
    // Use this for initialization
    
    void Start()
    {

    }

    private void InitNewConfig()
    {
        this.config = new GameEventsConfig();
        //添加几个默认事件组
        var gameGruop = new GameEventGroup("Game");
        config.groups.Add(gameGruop);
        var uiGroup = new GameEventGroup("UI");
        config.groups.Add(uiGroup);
        this.groupNames = new string[] { "Game", "UI" };
    }
    private void OnGUI()
    {
        if (config == null)
        {
            this.InitNewConfig();
        }
        GUILayout.Label("Game Event Builder");
        EditorGUILayout.Separator();
        GUILayout.BeginHorizontal();
        config.path = EditorGUILayout.TextField("Save Path", config.path);
        if (GUILayout.Button("", "IN ObjectField", GUILayout.Width(18)))
        {
            config.path = EditorUtility.OpenFolderPanel("Save Event Codes", config.path, "GameEvents");
        }

        GUILayout.EndHorizontal();
        selectedGroupIndex = EditorGUILayout.Popup("Event Group", selectedGroupIndex, groupNames);
        EditorGUILayout.Separator();
        config.hasNameSpace = EditorGUILayout.Toggle("Add NameSpace", config.hasNameSpace);
        if (config.hasNameSpace)
        {
            EditorGUI.indentLevel++;
            config.nameSpace = EditorGUILayout.TextField("NameSpace", config.nameSpace);
            EditorGUI.indentLevel--;
        }
        //绘制事件添加界面
        EditorGUILayout.Separator();
        GUILayout.Label("Add New Event To Group");
        if (tempEventDef == null)
        {
            tempEventDef = new GameEventDef();
        }
        EditorGUI.indentLevel++;
        tempEventDef.id = EditorGUILayout.IntField("ID:", tempEventDef.id);
        tempEventDef.name = EditorGUILayout.TextField("Name:", tempEventDef.name);
        EditorGUILayout.LabelField("Description:");
        tempEventDef.description = EditorGUILayout.TextArea( tempEventDef.description);
        EditorGUI.indentLevel--;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        var oldColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Add",GUILayout.Width(60)))
        {

        }
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Clear", GUILayout.Width(60)))
        {

        }
        GUI.backgroundColor = oldColor;
        GUILayout.EndHorizontal();
        //绘制当前数组
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region show window

    [MenuItem("Game Designer/Game Event Builder")]
    public static void ShowWindow()
    {
        var win = EditorWindow.GetWindow<GameEventBuilder>();
        win.Show();
    }

    #endregion
}
