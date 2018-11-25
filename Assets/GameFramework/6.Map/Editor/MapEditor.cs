using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameFramework
{
    [CustomEditor(typeof(Map))]
    public class MapEditor : Editor
    {
        SerializedProperty widthProp;
        SerializedProperty depthProp;
        SerializedProperty viewWidthProp;
        SerializedProperty viewDepthProp;
        SerializedProperty chunkPrefabProp;
        SerializedProperty dataProp;
        public string[] panelNames = new string[] {"Brushes","Blocks", "Chunks", "Config"};
        public int selectPanelIndex = 0;
        public string[] blockCreateModes = new string[] {"One Tex", "Two Tex", "Tree Tex", "Six Tex"};
        public int blockCreateModeIndex = 0;
        public BlockDefinition activeBlockDef;
        public Sprite top, bottom, left, right, front, back;
        public Vector2 blockScrollViewPos = Vector2.zero;
        public int selectBlockIndex = 0;

        private void OnEnable()
        {
            this.widthProp = serializedObject.FindProperty("width");
            this.depthProp = serializedObject.FindProperty("depth");
            this.viewWidthProp = serializedObject.FindProperty("viewWidth");
            this.viewDepthProp = serializedObject.FindProperty("viewDepth");
            this.chunkPrefabProp = serializedObject.FindProperty("chunkPrefab");
            this.dataProp = serializedObject.FindProperty("data");
        }
        private void OnDisable()
        {
            Map map = (Map)target;
            var data = map.data;
            EditorUtility.SetDirty(data);
        }
        private void OnDestroy()
        {
            Map map = (Map)target;
            var data = map.data;
            EditorUtility.SetDirty(data);
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Map map = (Map) target;
            var data = map.data;
//            serializedObject.Update();
//           var size= EditorGUILayout.Vector2IntField("Size", new Vector2Int(widthProp.intValue, depthProp.intValue));
//            widthProp.intValue = size.x;
//            depthProp.intValue = size.y;
//            GUILayout.BeginHorizontal();
//            widthProp.intValue= EditorGUILayout.IntField("Width",widthProp.intValue);
//            depthProp.intValue = EditorGUILayout.IntField("Depth", depthProp.intValue);
//            GUILayout.EndHorizontal();
            selectPanelIndex = GUILayout.Toolbar(selectPanelIndex, panelNames);
            if (panelNames[selectPanelIndex] == "Blocks")
            {
                this.DrawBlockEditPanel(data);
            }
           else if (panelNames[selectPanelIndex] == "Chunks")
            {
            }
            else if (panelNames[selectPanelIndex] == "Config")
            {
            }
            //            for (int i = 0; i < data.blockDefinitions.Count; i++)
            //            {
            //                
            //            }
        }

        public void DrawBlockEditPanel(MapData data)
        {
            EditorGUILayout.PrefixLabel("Edit Block");
            if (GUILayout.Button("Add New Block Definition"))
            {
                BlockDefinition newdef = new BlockDefinition();
                newdef.id = data.blockDefinitions.Count;
                data.blockDefinitions.Add(newdef);
            }
            //            EditorGUI.indentLevel++;
            EditorGUILayout.PrefixLabel("Block Count("+data.blockDefinitions.Count+")");

            var contents = new List<GUIContent>();
            for (int i = 0; i < data.blockDefinitions.Count; i++)
            {
                var def = data.blockDefinitions[i];
                if (i == selectBlockIndex)
                {
                    EditorGUILayout.BeginVertical((GUIStyle)"SelectionRect");
                    GUILayout.Toggle(true,string.Format("ID:{0},Name:{1}", def.id, def.name), (GUIStyle)"TL SelectionBarPreview", GUILayout.ExpandWidth(true));
                    def = data.blockDefinitions[i];
                    def.id = EditorGUILayout.IntField("ID", def.id);
                    def.name = EditorGUILayout.TextField("Name", def.name);
                    //贴图设置
                    EditorGUILayout.PrefixLabel("CreateMode");
                    blockCreateModeIndex = GUILayout.Toolbar(blockCreateModeIndex, blockCreateModes);
                    if (blockCreateModeIndex == 0)
                    {
                       var temp = EditorGUILayout.ObjectField("All Face", def.top, typeof(Sprite), false) as Sprite;
                        if (temp != null && temp!=def.top)
                        {
                            def.top = temp;
                            def.bottom = def.front = def.back = def.left = def.right = def.top;
                        }
                    }
                    else if (blockCreateModeIndex == 1)
                    {
                        def.top = EditorGUILayout.ObjectField("Top Face", def.top, typeof(Sprite), false) as Sprite;
                        var temp = EditorGUILayout.ObjectField("Other Face", def.bottom, typeof(Sprite), false) as Sprite;
                        if (temp != null && temp!=def.bottom)
                        {
                            def.bottom = temp;
                            def.front = def.back = def.left = def.right = def.bottom;
                        }
                    }
                    else if (blockCreateModeIndex == 2)
                    {
                        def.top = EditorGUILayout.ObjectField("Top Face", def.top, typeof(Sprite), false) as Sprite;
                        def.bottom = EditorGUILayout.ObjectField("Bottom Face", def.bottom, typeof(Sprite), false) as Sprite;
                        var temp= EditorGUILayout.ObjectField("Other Face", def.front, typeof(Sprite), false) as Sprite;
                        if (temp != null && temp!=def.front)
                        {
                            def.front = temp;
                            def.back = def.left = def.right = def.front;
                        }
                    }
                    else if (blockCreateModeIndex == 3)
                    {
                        def.top = EditorGUILayout.ObjectField("Top Face", def.top, typeof(Sprite), false) as Sprite;
                        def.bottom = EditorGUILayout.ObjectField("Bottom Face", def.bottom, typeof(Sprite), false) as Sprite;
                        def.front = EditorGUILayout.ObjectField("Front Face", def.front, typeof(Sprite), false) as Sprite;
                        def.back = EditorGUILayout.ObjectField("Back Face", def.back, typeof(Sprite), false) as Sprite;
                        def.left = EditorGUILayout.ObjectField("Left Face", def.left, typeof(Sprite), false) as Sprite;
                        def.right = EditorGUILayout.ObjectField("Right Face", def.right, typeof(Sprite), false) as Sprite;
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Del",GUILayout.MinWidth(80)))
                    {
                        data.blockDefinitions.RemoveAt(selectBlockIndex);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }
                else
                {
                    var flag = GUILayout.Toggle(false,string.Format("ID:{0},Name:{1}", def.id, def.name),(GUIStyle)"OL Title");
                    if (flag == true)
                    {
                        selectBlockIndex = i;
                    }
                }
                //                contents.Add(new GUIContent(string.Format("ID:{0},Name:{1}", def.id, def.name)));

            }
//            EditorGUI.indentLevel--;
            //            var selected= GUILayout.SelectionGrid(selectBlockIndex, contents.ToArray(),1, (GUIStyle)"OL Title");
            //            if (selected != selectBlockIndex)
            //            {
            //                selectBlockIndex = selected;
            //                this.activeBlockDef = data.blockDefinitions[selectBlockIndex];
            //            }
        }
    }
}