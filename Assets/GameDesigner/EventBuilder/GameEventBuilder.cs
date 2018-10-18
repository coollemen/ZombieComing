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
    public string[] groupNames = new string[] { };

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
    public Color oldColor;

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

        oldColor = GUI.backgroundColor;
        //绘制基础信息
        this.DrawBasePanel();
        //绘制组合编辑界面
        this.DrawGroupPanel();
        //绘制事件编辑界面
        this.DrawEventPanel();
        EditorGUILayout.Separator();
        //绘制当前组事件信息
        this.DrawEventList();
        //绘制导出按键
        EditorGUILayout.Separator();
        GUI.backgroundColor =new Color(1f,0,1f);
        if (GUILayout.Button("Export To Code"))
        {
            this.ExportToCode();
        }
        GUI.backgroundColor = oldColor;

    }

    private void DrawBasePanel()
    {
        //添加标题
        EditorGUILayout.LabelField(" Base Config", (GUIStyle) "flow navbar back");
        EditorGUI.indentLevel++;
        GUILayout.BeginHorizontal();
        //绘制路径，并添加选择按钮
        config.path = EditorGUILayout.TextField("Save Path", config.path);
        this.currGroup = this.config.groups[selectedGroupIndex];
        if (GUILayout.Button("", "IN ObjectField", GUILayout.Width(18)))
        {
            config.path = EditorUtility.OpenFolderPanel("Save Event Codes", config.path, "GameEvents");
        }

        GUILayout.EndHorizontal();
        //绘制命名空间字段
        config.hasNameSpace = EditorGUILayout.Toggle("Add NameSpace", config.hasNameSpace);
        if (config.hasNameSpace)
        {
            EditorGUI.indentLevel++;
            config.nameSpace = EditorGUILayout.TextField("NameSpace", config.nameSpace);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Separator();
        EditorGUI.indentLevel--;
    }

    private void DrawGroupPanel()
    {
        //绘制标题
        EditorGUILayout.LabelField(" Event Groups", (GUIStyle) "flow navbar back");
        //如果不是编辑模式
        if (isEditMode == false)
        {
            //开始缩进
            EditorGUI.indentLevel++;
            //组合基本字段
            selectedGroupIndex = EditorGUILayout.Popup("Group", selectedGroupIndex, groupNames);
            currGroup.startNum = EditorGUILayout.IntField("Start Num", currGroup.startNum);
            currGroup.span = EditorGUILayout.IntField("Span", currGroup.span);
            //开始水平组
            GUILayout.BeginHorizontal();
            //将button居右
            GUILayout.FlexibleSpace();
            //设置按键颜色
            GUI.backgroundColor = new Color(0, 0.9f, 1); ;
            if (GUILayout.Button("Edit Group", (GUIStyle)"minibutton", GUILayout.Width(100)))
            {
                isEditMode = true;
            }

            GUI.backgroundColor = oldColor;
            //结束水平组
            GUILayout.EndHorizontal();
            //结束缩进
            EditorGUI.indentLevel--;
        }
        else
        {
            //开始缩进
            EditorGUI.indentLevel++;
            //开始垂直组
            GUILayout.BeginVertical();
            //开始子水平组
            GUILayout.BeginHorizontal();
            //设置标题文字颜色
            GUI.contentColor = Color.gray;
            //绘制标题
            EditorGUILayout.LabelField("Group Name", GUILayout.Width(120));
            EditorGUILayout.LabelField("Count", GUILayout.Width(60));
            //结束子水平组
            GUILayout.EndHorizontal();
            //恢复默认颜色
            GUI.contentColor = oldColor;
            //绘制组合列表
            for (int i = 0; i < config.groups.Count; i++)
            {
                var g = config.groups[i];
                GUILayout.BeginHorizontal();
                EditorGUILayout.TextField(g.name, GUILayout.Width(120));
                EditorGUILayout.LabelField(g.events.Count.ToString(), GUILayout.Width(60));
                GUI.backgroundColor = Color.red;
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Del", (GUIStyle)"minibutton",GUILayout.Width(50)))
                {
                    //如果组合里存在事件，那么删除时显示确定对话框
                    if (config.groups[i].events.Count > 0)
                    {
                        if (EditorUtility.DisplayDialog("删除", "确定删除当前事件组？", "确定", "取消"))
                        {
                            config.groups.RemoveAt(i);
                            isDirty = true;
                        }
                    }
                }

                GUI.backgroundColor = oldColor;
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            //开始水平组
            GUILayout.BeginHorizontal();
            newGroupName = EditorGUILayout.TextField("New Group Name", newGroupName);
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Add Group", (GUIStyle)"minibuttonleft", GUILayout.MinWidth(60)))
            {
                var newGroup = new GameEventGroup(newGroupName);
                this.config.groups.Add(newGroup);
                isDirty = true;
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Exit Edit", (GUIStyle)"minibuttonright", GUILayout.MinWidth(60)))
            {
                isEditMode = false;
            }

            GUI.backgroundColor = oldColor;
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }
    }

    private void DrawEventPanel()
    {
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
        if (GUILayout.Button("New Event", (GUIStyle)"minibuttonleft", GUILayout.MinWidth(80)))
        {
            this.NewEventDef();
        }
        GUI.backgroundColor =new Color(0,0.9f,1);
        if (GUILayout.Button("Add Event", (GUIStyle)"minibuttonmid", GUILayout.MinWidth(80)))
        {
            currGroup.events.Add(tempEventDef);
            this.NewEventDef();
        }
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Clear Events", (GUIStyle)"minibuttonright", GUILayout.MinWidth(80)))
        {
            if (EditorUtility.DisplayDialog("清空", "确定清空当前事件组？", "确定", "取消"))
            {
                currGroup.events.Clear();
                this.NewEventDef();
            }
        }
        GUI.backgroundColor = oldColor;

        GUILayout.EndHorizontal();
    }

    public void DrawEventList()
    {
        EditorGUILayout.LabelField(" [ " + groupNames[selectedGroupIndex] + " ] Events",
            (GUIStyle)"flow navbar back");
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
            if (GUILayout.Button("Del", (GUIStyle)"minibutton", GUILayout.Width(30)))
            {
                currGroup.events.RemoveAt(i);
            }
            GUI.backgroundColor = oldColor;
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        EditorGUI.indentLevel--;
    }
    /// <summary>
    /// 导出为C#代码
    /// </summary>
    private void ExportToCode()
    {
        //判断是否有响应的文件夹在
        string folderPath = config.path + "/Events/";
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderPath);
        if (dir.Exists == false)
        {
            dir.Create();
        }

        foreach (var g in config.groups)
        {
            var code = this.GroupToEnumCode(g);
            var fileName = g.name + "Events.cs";
            var filePath = folderPath + fileName;
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Truncate);
                System.IO.StreamWriter writer = new System.IO.StreamWriter(fs);
                writer.Write(code);
                writer.Close();
                fs.Close();
            }
            catch(System.Exception ex)
            {
                System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
                System.IO.StreamWriter writer = new System.IO.StreamWriter(fs);
                writer.Write(code);
                writer.Close();
                fs.Close();
            }

        }
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", "导出代码成功！", "确定");
    }

    private string GroupToEnumCode(GameEventGroup g)
    {
        string code = "";
        code += "using System.Collections;\r\n";
        code += "using System.Collections.Generic;\r\n";
        if (config.hasNameSpace)
        {
            code += string.Format("namespace {0}\r\n", config.nameSpace);
            code += "{\r\n";
        }
        code += string.Format("public enum {0}\r\n", g.name + "Events");
        code += "{\r\n";
        if (g.events.Count > 0)
        {
            for (int i = 0; i < g.events.Count - 1; i++)
            {
                code += string.Format("{0}={1},\r\n", g.events[i].name, g.events[i].id);
            }
            code += string.Format("{0}={1}\r\n", g.events[g.events.Count - 1].name, g.events[g.events.Count - 1].id);
        }
        code += "}\r\n";
        if (config.hasNameSpace)
        {
            code += "}\r\n";
        }
        return code;
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

public enum GameEvents
{
    Game_Start=0,
}