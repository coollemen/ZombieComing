using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework
{
    [CustomEditor(typeof(BlockObjectRTE))]
    public class BlockObjectRTEEditor : Editor
    {
        public Vector3Int canvasSize;
        public string[] panelNames = new string[] {"工具栏", "图块", "设置"};
        public int selectedPanelIndex = 0;

        public string[] blockDefTypes = new string[] {"单色", "贴图"};
        public int selectedBlockDefTypeIndex = 0;
        public string blockDefName = "block";
        public Color blockDefColor = Color.white;
        public Sprite blockDefSprite;
        public string[] sprBlockCreateModes = new string[] {"One Tex", "Two Tex", "Tree Tex", "Six Tex"};
        public int selectedSprBlockCreateModeIndex = 0;

        public int selectedBlockDefIndex = 0;

        public List<string> blockNames = new List<string>();
        public int selectedBlockIndex = 0;

        public string[] toolNames = new string[] {"画笔", "油漆桶", "选择工具", "移动工具","3D物体"};
        public int selectedToolIndex = 0;

        public string[] geometryNames = new string[] {"正方体", "长方体", "球", "圆柱体"};
        public int selectedGeometeryIndex = 0;
        public BlockObjectRTE boEditor;

        public override void OnInspectorGUI()
        {
            if (boEditor == null)
            {
                boEditor = target as BlockObjectRTE;
            }
            base.OnInspectorGUI();
            //            this.boEditor.data = EditorGUILayout.ObjectField(boEditor.data, typeof(BlockObjectData)) as BlockObjectData;
            EditorGUILayout.LabelField("Canvas");
            EditorGUI.indentLevel++;
            this.canvasSize = EditorGUILayout.Vector3IntField("Size", canvasSize);
            EditorGUI.indentLevel--;
            selectedPanelIndex = GUILayout.Toolbar(selectedPanelIndex, panelNames);
            if (selectedPanelIndex == 0)
            {
                this.DrawBrushPanel();
            }
            else if (selectedPanelIndex == 1)
            {
                this.DrawBlockPanel();
            }
        }

        private void DrawBlockPanel()
        {
            EditorGUILayout.LabelField("创建新图块");
            EditorGUI.indentLevel++;

            selectedBlockDefTypeIndex = EditorGUILayout.Popup("图块类型", selectedBlockDefTypeIndex, blockDefTypes);
            if (selectedBlockDefTypeIndex == 0)
            {
                blockDefName = EditorGUILayout.TextField("名称", blockDefName);
                blockDefColor = EditorGUILayout.ColorField("颜色", blockDefColor);
            }
            else if (selectedBlockDefTypeIndex == 1)
            {
                blockDefName = EditorGUILayout.TextField("名称", blockDefName);
                blockDefSprite = EditorGUILayout.ObjectField("精灵", blockDefSprite, typeof(Sprite)) as Sprite;
            }
            EditorGUI.indentLevel--;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("添加", GUILayout.Width(80)))
            {
                if (selectedBlockDefTypeIndex == 0)
                {
                    var def = new ColorBlockDefinition((byte) boEditor.data.blockDefs.Count, blockDefName,
                        blockDefColor);
                    boEditor.data.blockDefs.Add(def);
                }
                else if (selectedBlockDefTypeIndex == 1)
                {
                    var def = new SpriteBlockDefinition((byte) boEditor.data.blockDefs.Count, blockDefName,
                        blockDefSprite);
                    boEditor.data.blockDefs.Add(def);
                }
            }
            if (GUILayout.Button("清空", GUILayout.Width(40)))
            {
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            //显示当前存在的图块定义
            EditorGUILayout.LabelField("图块列表");
            EditorGUI.indentLevel++;
            for (int i = 0; i < boEditor.data.blockDefs.Count; i++)
            {
                var def = boEditor.data.blockDefs[i];
                if (i == selectedBlockDefIndex)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical((GUIStyle) "MeTransitionSelect", GUILayout.Height(200));
                    GUILayout.Toggle(true, string.Format("ID:{0},Name:{1}", def.id, def.name),
                        (GUIStyle) "MeTransitionSelectHead", GUILayout.Height(30));
                    def = boEditor.data.blockDefs[i];
                    if (def is SpriteBlockDefinition)
                    {
                        this.DrawSpriteBlockDefItem(def as SpriteBlockDefinition);
                    }
                    else if (def is ColorBlockDefinition)
                    {
                        this.DrawColorBlockDefItem(def as ColorBlockDefinition);
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    MyGUITools.SetBackgroundColor(Color.red);
                    if (GUILayout.Button("Del", GUILayout.MinWidth(80)))
                    {
                        boEditor.data.blockDefs.RemoveAt(selectedBlockDefIndex);
                    }
                    MyGUITools.RestoreBackgroundColor();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
                else
                {
                    var flag = GUILayout.Toggle(false, string.Format("ID:{0},Name:{1}", def.id, def.name),
                        (GUIStyle) "OL Title");
                    if (flag == true)
                    {
                        selectedBlockDefIndex = i;
                    }
                }
            }
            EditorGUI.indentLevel--;
        }

        private void DrawSpriteBlockDefItem(SpriteBlockDefinition sprDef)
        {
            sprDef.id = EditorGUILayout.IntField("ID", sprDef.id);
            sprDef.name = EditorGUILayout.TextField("Name", sprDef.name);
            //贴图设置
            EditorGUILayout.PrefixLabel("CreateMode");
            selectedSprBlockCreateModeIndex = GUILayout.Toolbar(selectedSprBlockCreateModeIndex, sprBlockCreateModes);
            if (selectedSprBlockCreateModeIndex == 0)
            {
                var temp = EditorGUILayout.ObjectField("All Face", sprDef.top, typeof(Sprite), false) as Sprite;
                if (temp != null && temp != sprDef.top)
                {
                    sprDef.top = temp;
                    sprDef.bottom = sprDef.front = sprDef.back = sprDef.left = sprDef.right = sprDef.top;
                }
            }
            else if (selectedSprBlockCreateModeIndex == 1)
            {
                sprDef.top = EditorGUILayout.ObjectField("Top Face", sprDef.top, typeof(Sprite), false) as Sprite;
                var temp =
                    EditorGUILayout.ObjectField("Other Face", sprDef.bottom, typeof(Sprite), false) as Sprite;
                if (temp != null && temp != sprDef.bottom)
                {
                    sprDef.bottom = temp;
                    sprDef.front = sprDef.back = sprDef.left = sprDef.right = sprDef.bottom;
                }
            }
            else if (selectedSprBlockCreateModeIndex == 2)
            {
                sprDef.top = EditorGUILayout.ObjectField("Top Face", sprDef.top, typeof(Sprite), false) as Sprite;
                sprDef.bottom =
                    EditorGUILayout.ObjectField("Bottom Face", sprDef.bottom, typeof(Sprite), false) as Sprite;
                var temp =
                    EditorGUILayout.ObjectField("Other Face", sprDef.front, typeof(Sprite), false) as Sprite;
                if (temp != null && temp != sprDef.front)
                {
                    sprDef.front = temp;
                    sprDef.back = sprDef.left = sprDef.right = sprDef.front;
                }
            }
            else if (selectedSprBlockCreateModeIndex == 3)
            {
                sprDef.top = EditorGUILayout.ObjectField("Top Face", sprDef.top, typeof(Sprite), false) as Sprite;
                sprDef.bottom =
                    EditorGUILayout.ObjectField("Bottom Face", sprDef.bottom, typeof(Sprite), false) as Sprite;
                sprDef.front =
                    EditorGUILayout.ObjectField("Front Face", sprDef.front, typeof(Sprite), false) as Sprite;
                sprDef.back =
                    EditorGUILayout.ObjectField("Back Face", sprDef.back, typeof(Sprite), false) as Sprite;
                sprDef.left =
                    EditorGUILayout.ObjectField("Left Face", sprDef.left, typeof(Sprite), false) as Sprite;
                sprDef.right =
                    EditorGUILayout.ObjectField("Right Face", sprDef.right, typeof(Sprite), false) as Sprite;
            }
        }

        private void DrawBlockCreateMode()
        {
        }

        private void DrawColorBlockDefItem(ColorBlockDefinition colorDef)
        {
            colorDef.id = EditorGUILayout.IntField("ID", colorDef.id);
            colorDef.name = EditorGUILayout.TextField("Name", colorDef.name);
            colorDef.color = EditorGUILayout.ColorField("Color", colorDef.color);
        }

        private void DrawBrushPanel()
        {
            this.blockNames.Clear();
            for (int i = 0; i < boEditor.data.blockDefs.Count; i++)
            {
                this.blockNames.Add(boEditor.data.blockDefs[i].name);
            }
            EditorGUILayout.LabelField("图块列表");
            selectedBlockIndex = GUILayout.SelectionGrid(selectedBlockIndex, blockNames.ToArray(), 5);
            EditorGUILayout.LabelField("笔刷工具");
            selectedToolIndex = GUILayout.Toolbar(selectedToolIndex, toolNames);
            if (toolNames[selectedToolIndex] == "画笔")
            {

            }
            else if (toolNames[selectedToolIndex] == "油漆桶")
            {
            }
            else if (toolNames[selectedToolIndex] == "选择工具")
            {
            }
            else if (toolNames[selectedToolIndex] == "移动工具")
            {
            }
            else if (toolNames[selectedToolIndex] == "3D物体")
            {
                selectedGeometeryIndex = EditorGUILayout.Popup("几何体",selectedGeometeryIndex, geometryNames);
            }
        }
        
        private void DrawConfigPanel()
        {
        }

        public void OnSceneGUI()
        {
        }

        #region 场景视图绘制函数

        #endregion
    }
}