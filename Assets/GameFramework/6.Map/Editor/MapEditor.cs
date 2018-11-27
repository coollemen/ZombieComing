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
        public string[] panelNames = new string[] {"Brushes", "Blocks", "Layers", "Config"};
        public int selectPanelIndex =2;
        public string[] blockCreateModes = new string[] {"One Tex", "Two Tex", "Tree Tex", "Six Tex"};
        public int blockCreateModeIndex = 0;
        public BlockDefinition activeBlockDef;
        public Sprite top, bottom, left, right, front, back;
        public Vector2 blockScrollViewPos = Vector2.zero;
        public int selectBlockIndex = 0;
        public int selectLayerIndex = 0;
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
            Map map = (Map) target;
            var data = map.data;
            EditorUtility.SetDirty(data);
        }

        private void OnDestroy()
        {
            Map map = (Map) target;
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
            if (panelNames[selectPanelIndex] == "Brushes")
            {
                this.DrawBurshesPanel(data);
            }
            else if (panelNames[selectPanelIndex] == "Blocks")
            {
                this.DrawBlocksPanel(data);
            }
            else if (panelNames[selectPanelIndex] == "Layers")
            {
                this.DrawLayersPanel(data);
            }
            else if (panelNames[selectPanelIndex] == "Config")
            {
            }
        }
        /// <summary>
        /// 绘制笔刷页面
        /// </summary>
        /// <param name="data"></param>
        public void DrawBurshesPanel(MapData data)
        {
            
        }
        /// <summary>
        /// 绘制Layers页面
        /// </summary>
        /// <param name="data"></param>
        public void DrawLayersPanel(MapData data)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            MyGUITools.SetBackgroundColor(Color.green);
            if (GUILayout.Button("Add New Layer"))
            {
                MapLayer layer = new MapLayer(0,255);
                data.layers.Add(layer);

            }
            MyGUITools.RestoreBackgroundColor();
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < data.layers.Count; i++)
            {
                var layer = data.layers[i];
                if (i == selectLayerIndex)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical((GUIStyle)"SelectionRect");
                    GUILayout.Toggle(false, string.Format("{0}:Start:{1}-End:{2}", i, layer.start, layer.end), (GUIStyle)"TL SelectionBarPreview", GUILayout.ExpandWidth(true));
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Start");
                    layer.start = EditorGUILayout.IntField( layer.start);
                    GUILayout.Label("End  ");
                    layer.end = EditorGUILayout.IntField( layer.end);
                    GUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    MyGUITools.SetBackgroundColor(Color.green);
                    if (GUILayout.Button("Add New Item"))
                    {
                        MapLayerItem newItem = new MapLayerItem();
                        layer.items.Add(newItem);
                    }
                    MyGUITools.RestoreBackgroundColor();
                    EditorGUILayout.EndHorizontal();
                    for (int j = 0; j < layer.items.Count; j++)
                    {
                        var item = layer.items[j];
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Block");
                        item.blockId= EditorGUILayout.IntPopup(item.blockId,GetBlockNames(data),GetBlockIDs(data));
                        GUILayout.Label("Weight ");
                        item.weight= EditorGUILayout.FloatField(item.weight);
                        MyGUITools.SetBackgroundColor(Color.red);
                        if (GUILayout.Button("Del"))
                        {
                            layer.items.RemoveAt(j);
                        }
                        MyGUITools.RestoreBackgroundColor();
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
                else
                {
                    var flag = GUILayout.Toggle(false, string.Format("{0}:Start:{1}-End:{2}", i,layer.start,layer.end), (GUIStyle)"OL Title");
                    if (flag == true)
                    {
                        selectLayerIndex = i;
                    }
                }
            }
        }

        public int[] GetBlockIDs(MapData data)
        {
            List<int> ids = new List<int>();
            foreach (var d in data.blockDefinitions)
            {
                ids.Add(d.id);
            }
            return ids.ToArray();
        }
        public string[] GetBlockNames(MapData data)
        {
            List<string> names = new List<string>();
            foreach (var d in data.blockDefinitions)
            {
                names.Add(d.name);
            }
            return names.ToArray();
        }
        /// <summary>
        /// 绘制Blocks页面
        /// </summary>
        /// <param name="data"></param>
        public void DrawBlocksPanel(MapData data)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            MyGUITools.SetBackgroundColor(Color.green);
            if (GUILayout.Button("Add New Block Definition"))
            {
                BlockDefinition newdef = new BlockDefinition();
                newdef.id = data.blockDefinitions.Count;
                data.blockDefinitions.Add(newdef);
            }
            MyGUITools.RestoreBackgroundColor();
            GUILayout.EndHorizontal();
            //            EditorGUI.indentLevel++;
            GUILayout.Label("Block Count(" + data.blockDefinitions.Count + ")");

            var contents = new List<GUIContent>();
            for (int i = 0; i < data.blockDefinitions.Count; i++)
            {
                var def = data.blockDefinitions[i];
                if (i == selectBlockIndex)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginVertical((GUIStyle) "SelectionRect");
                    GUILayout.Toggle(true, string.Format("ID:{0},Name:{1}", def.id, def.name),
                        (GUIStyle) "TL SelectionBarPreview", GUILayout.ExpandWidth(true));
                    def = data.blockDefinitions[i];
                    def.id = EditorGUILayout.IntField("ID", def.id);
                    def.name = EditorGUILayout.TextField("Name", def.name);
                    //贴图设置
                    EditorGUILayout.PrefixLabel("CreateMode");
                    blockCreateModeIndex = GUILayout.Toolbar(blockCreateModeIndex, blockCreateModes);
                    if (blockCreateModeIndex == 0)
                    {
                        var temp = EditorGUILayout.ObjectField("All Face", def.top, typeof(Sprite), false) as Sprite;
                        if (temp != null && temp != def.top)
                        {
                            def.top = temp;
                            def.bottom = def.front = def.back = def.left = def.right = def.top;
                        }
                    }
                    else if (blockCreateModeIndex == 1)
                    {
                        def.top = EditorGUILayout.ObjectField("Top Face", def.top, typeof(Sprite), false) as Sprite;
                        var temp =
                            EditorGUILayout.ObjectField("Other Face", def.bottom, typeof(Sprite), false) as Sprite;
                        if (temp != null && temp != def.bottom)
                        {
                            def.bottom = temp;
                            def.front = def.back = def.left = def.right = def.bottom;
                        }
                    }
                    else if (blockCreateModeIndex == 2)
                    {
                        def.top = EditorGUILayout.ObjectField("Top Face", def.top, typeof(Sprite), false) as Sprite;
                        def.bottom =
                            EditorGUILayout.ObjectField("Bottom Face", def.bottom, typeof(Sprite), false) as Sprite;
                        var temp = EditorGUILayout.ObjectField("Other Face", def.front, typeof(Sprite), false) as Sprite;
                        if (temp != null && temp != def.front)
                        {
                            def.front = temp;
                            def.back = def.left = def.right = def.front;
                        }
                    }
                    else if (blockCreateModeIndex == 3)
                    {
                        def.top = EditorGUILayout.ObjectField("Top Face", def.top, typeof(Sprite), false) as Sprite;
                        def.bottom =
                            EditorGUILayout.ObjectField("Bottom Face", def.bottom, typeof(Sprite), false) as Sprite;
                        def.front =
                            EditorGUILayout.ObjectField("Front Face", def.front, typeof(Sprite), false) as Sprite;
                        def.back = EditorGUILayout.ObjectField("Back Face", def.back, typeof(Sprite), false) as Sprite;
                        def.left = EditorGUILayout.ObjectField("Left Face", def.left, typeof(Sprite), false) as Sprite;
                        def.right =
                            EditorGUILayout.ObjectField("Right Face", def.right, typeof(Sprite), false) as Sprite;
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    MyGUITools.SetBackgroundColor(Color.red);
                    if (GUILayout.Button("Del", GUILayout.MinWidth(80)))
                    {
                        data.blockDefinitions.RemoveAt(selectBlockIndex);
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
                        selectBlockIndex = i;
                    }
                }
            }
        }
    }//end class
}