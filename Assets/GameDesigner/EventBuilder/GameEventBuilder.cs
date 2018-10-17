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
    public string[] groupNames = new string[] {};

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
    public bool isEditMode = false;
    public string newGroupName = "";
    public bool isDirty = false;

    void Start()
    {

    }

    private void InitNewConfig()
    {
        this.config = ScriptableObject.CreateInstance<GameEventsConfig>();
        //添加几个默认事件组
        var gameGruop = new GameEventGroup("Game");
        config.groups.Add(gameGruop);
        var uiGroup = new GameEventGroup("UI");
        config.groups.Add(uiGroup);
        this.groupNames = GetGroupNames();
    }

    private void LoadConfig()
    {
        string folderPath = "Assets/Resources/GameEventBuilder";
        if (AssetDatabase.IsValidFolder(folderPath) == false)
        {
            AssetDatabase.CreateFolder("Assets", "GameEventBuilder");
            AssetDatabase.MoveAsset("Assets/GameEventBuilder", "Assets/Resources/GameEventBuilder");
            this.InitNewConfig();
            this.SaveConfig();
        }
        else
        {
            string loadPath = folderPath + "/EventBuilderConfig.asset";
            this.config = AssetDatabase.LoadAssetAtPath<GameEventsConfig>(loadPath);
            if (config == null)
            {
                this.InitNewConfig();
                this.SaveConfig();
            }
            this.groupNames = GetGroupNames();
        }
    }

    private void SaveConfig()
    {
        string folderPath = "Assets/Resources/GameEventBuilder";
        if (AssetDatabase.IsValidFolder(folderPath) == false)
        {
            AssetDatabase.CreateFolder("Assets", "GameEventBuilder");
            AssetDatabase.MoveAsset("Assets/GameEventBuilder", "Assets/Resources/GameEventBuilder");
        }
        string savePath = folderPath + "/EventBuilderConfig.asset";
        if (AssetDatabase.Contains(config) == false)
        {
            AssetDatabase.CreateAsset(config, savePath);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取事件组合的名称
    /// </summary>
    /// <returns>名称数组</returns>
    private string[] GetGroupNames()
    {
        string[] names = new string[config.groups.Count];
        for (int i = 0; i < config.groups.Count; i++)
        {
            names[i] = config.groups[i].name;
        }
        return names;
    }

    private void OnGUI()
    {
        if (isDirty)
        {
            this.groupNames = GetGroupNames();
            isDirty = false;
        }
        var oldColor = GUI.backgroundColor;
        EditorGUILayout.LabelField(" Base Config", (GUIStyle) "flow navbar back");
        EditorGUI.indentLevel++;
        GUILayout.BeginHorizontal();
        config.path = EditorGUILayout.TextField("Save Path", config.path);
        this.currGroup = this.config.groups[selectedGroupIndex];
        if (GUILayout.Button("", "IN ObjectField", GUILayout.Width(18)))
        {
            config.path = EditorUtility.OpenFolderPanel("Save Event Codes", config.path, "GameEvents");
        }
        GUILayout.EndHorizontal();
        config.hasNameSpace = EditorGUILayout.Toggle("Add NameSpace", config.hasNameSpace);
        if (config.hasNameSpace)
        {
            EditorGUI.indentLevel++;
            config.nameSpace = EditorGUILayout.TextField("NameSpace", config.nameSpace);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.Separator();
        EditorGUI.indentLevel--;
        //绘制组合数据
        EditorGUILayout.LabelField(" Event Groups", (GUIStyle) "flow navbar back");
        if (isEditMode == false)
        {
            EditorGUI.indentLevel++;
            selectedGroupIndex = EditorGUILayout.Popup("Group", selectedGroupIndex, groupNames);
            currGroup.startNum = EditorGUILayout.IntField("Start Num", currGroup.startNum);
            currGroup.span = EditorGUILayout.IntField("Span", currGroup.span);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Edit Group", GUILayout.Width(100)))
            {
                isEditMode = true;
            }
            GUI.backgroundColor = oldColor;
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;
            GUILayout.BeginVertical();
            GUI.contentColor = Color.gray;
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Group Name", GUILayout.Width(160));
            EditorGUILayout.LabelField("Group Count");
            GUILayout.EndHorizontal();
            GUI.contentColor = oldColor;
            for (int i = 0; i < config.groups.Count; i++)
            {
                var g = config.groups[i];
                GUILayout.BeginHorizontal();
                EditorGUILayout.TextField(g.name, GUILayout.Width(160));
                EditorGUILayout.LabelField(g.events.Count.ToString());
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Del", GUILayout.Width(30)))
                {
                    if (config.groups[i].events.Count > 0)
                    {
                        if (EditorUtility.DisplayDialog("删除", "确定删除当前事件组？", "确定", "取消"))
                        {
                            config.groups.RemoveAt(i);
                            isDirty = true;
                        }
                    }
                    GUI.backgroundColor = oldColor;
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.BeginHorizontal();
            newGroupName = EditorGUILayout.TextField("New Group Name", newGroupName);
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Add Group", GUILayout.Width(100)))
            {
                var newGroup = new GameEventGroup(newGroupName);
                this.config.groups.Add(newGroup);
                isDirty = true;
            }
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Exit Edit", GUILayout.Width(100)))
            {
                isEditMode = false;
            }
            GUI.backgroundColor = oldColor;
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }

        //绘制事件添加界面
        EditorGUI.indentLevel++;
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Add New Event To [ " + groupNames[selectedGroupIndex] + " ] Group:");
        if (tempEventDef == null)
        {
            tempEventDef = new GameEventDef();
        }
        tempEventDef.id = EditorGUILayout.IntField("ID:", tempEventDef.id);
        tempEventDef.name = EditorGUILayout.TextField("Name:", tempEventDef.name);
        EditorGUILayout.LabelField("Description:");
        tempEventDef.description = EditorGUILayout.TextArea(tempEventDef.description);
        EditorGUI.indentLevel--;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("New Event", GUILayout.Width(100)))
        {
            this.NewEventDef();
        }
        GUI.backgroundColor = Color.blue;
        if (GUILayout.Button("Add Event", GUILayout.Width(100)))
        {
            currGroup.events.Add(tempEventDef);
            this.NewEventDef();
        }
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Clear Events", GUILayout.Width(100)))
        {
            if (EditorUtility.DisplayDialog("清空", "确定清空当前事件组？", "确定", "取消"))
            {
                currGroup.events.Clear();
                this.NewEventDef();
            }
        }
        GUI.backgroundColor = oldColor;

        GUILayout.EndHorizontal();
        //绘制当前的事件组合里的事件列表
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField(" [ " + groupNames[selectedGroupIndex] + " ] Events",
            (GUIStyle) "flow navbar back");
        GUILayout.BeginVertical();
        //绘制标题列
        EditorGUI.indentLevel++;
        GUI.contentColor = Color.gray;
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Event ID", GUILayout.Width(100));
        EditorGUILayout.LabelField("Event Name", GUILayout.Width(160));
        EditorGUILayout.LabelField("Event Description");
        GUILayout.EndHorizontal();
        GUI.contentColor = oldColor;
        for (int i = 0; i < currGroup.events.Count; i++)
        {
            var e = currGroup.events[i];
            GUILayout.BeginHorizontal();
            EditorGUILayout.IntField(e.id, GUILayout.Width(100));
            EditorGUILayout.TextField(e.name, GUILayout.Width(160));
            EditorGUILayout.TextField(e.description);
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Del", GUILayout.Width(30)))
            {
                currGroup.events.RemoveAt(i);
            }
            GUI.backgroundColor = oldColor;
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        EditorGUI.indentLevel--;
        //绘制当前数组
    }


    private void NewEventDef()
    {
        tempEventDef = new GameEventDef();
        tempEventDef.id = this.config.groups[selectedGroupIndex].GetNextID();
        tempEventDef.name = this.config.groups[selectedGroupIndex].name + "_";
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        this.LoadConfig();
    }

    private void OnDisable()
    {
        this.SaveConfig();
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
