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
        private List<Bounds> chunkBounds;
        private bool isDirty = false;
        private int selectedChunkIndex = -1;
        private bool isMouseIn = false;
        private Map map;
        private MapData data;
        private void OnEnable()
        {
            this.widthProp = serializedObject.FindProperty("width");
            this.depthProp = serializedObject.FindProperty("depth");
            this.viewWidthProp = serializedObject.FindProperty("viewWidth");
            this.viewDepthProp = serializedObject.FindProperty("viewDepth");
            this.chunkPrefabProp = serializedObject.FindProperty("chunkPrefab");
            this.dataProp = serializedObject.FindProperty("data");
            map = (Map)target;
            data = map.data;
        }

        private void OnDisable()
        {
            EditorUtility.SetDirty(data);
        }

        private void OnDestroy()
        {
            EditorUtility.SetDirty(data);
        }
        /// <summary>
        /// 场景视图GUI绘制
        /// </summary>
        public void OnSceneGUI()
        {
            //如果map为null 不绘制
            if (map == null) return;
            if (isDirty || this.chunkBounds==null)
            {
                this.CreateChunkBounds(map);
                isDirty = false;
            }
//            this.DrawMapCube(map);
//            this.DrawMapRectangle(map);
            this.DrawChunkRactangle(map);
//            this.DrawChunkBounds(map);
            List<int> hitBlockBounds = new List<int>();
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            for (int i = 0; i < chunkBounds.Count; i++)
            {
                if (chunkBounds[i].IntersectRay(mouseRay))
                {
                    hitBlockBounds.Add(i);
                }
            }
            if (hitBlockBounds.Count > 0)
            {
//                Debug.Log("hit chunk!");
                float distance = 10000000;
                int activeID = -1;
                for (int i = 0; i < hitBlockBounds.Count; i++)
                {
                    var index = hitBlockBounds[i];
                    float tempDistance = (mouseRay.origin -chunkBounds[index].center).magnitude;
                    if (tempDistance < distance)
                    {
                        distance = tempDistance;
                        activeID = index;
                    }

                }
                this.selectedChunkIndex = activeID;
            }
            this.DrawActiveChunk(map);
            //绘制地图碰撞盒
            var mapBounds = this.GetMapBounds(map);
            //判读鼠标是不是在地图内
            if (mapBounds.IntersectRay(mouseRay))
            {
                this.DrawMapBounds(mapBounds);
                isMouseIn = true;
            }
            else
            {
                isMouseIn = false;
            }

            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 0 && isMouseIn)
                {
                    Debug.Log("Mouse Left Button Click!");
                    if (this.selectedChunkIndex != -1 )
                    {
                        var go= map.transform.GetChild(0).gameObject;
                        EditorGUIUtility.PingObject(go);
                        Selection.activeGameObject = go;
                        Event.current.Use();
                    }

                }
            }
        }
        #region SenceView相关函数
        private void CreateBlockBounds()
        {
            
        }

        private void CheckHitBlock()
        {
            
        }

        private void DrawActiveBlocks()
        {
            
        }
        private void DrawActiveChunk(Map map)
        {
            if (this.selectedChunkIndex == -1) return;
//            Debug.Log("Select Chunk Index=" + this.selectedChunkIndex);
            var b = this.chunkBounds[this.selectedChunkIndex];
            var c = Handles.color;
            Handles.color = Color.green;
            Handles.DrawWireCube(b.center, b.size);
            Handles.color = c;
        }

        private void DrawMapBounds(Bounds b)
        {
            var c = Handles.color;
            Handles.color = Color.yellow;
            Handles.DrawWireCube(b.center, b.size);
            Handles.color = c;
        }
        private Bounds GetMapBounds(Map map)
        {
            var pos = map.gameObject.transform.position;

            var center = new Vector3(pos.x + map.width / 2 * 16,0.5f, pos.z + map.depth / 2 * 16);
            var size = new Vector3(map.width * 16, 1, map.depth * 16);
            Bounds b = new Bounds(center,size);
            return b;
        }
        private void CreateChunkBounds(Map map)
        {
            this.chunkBounds = new List<Bounds>();
            var pos = map.gameObject.transform.position;
            for (int j = 0; j < map.depth; j++)
            {
                for (int i = 0; i < map.width; i++)
                {

                    Bounds b = new Bounds();
                    b.center = new Vector3(pos.x + i * 16 + 8,
                        pos.y + 0.5f,
                        pos.z + j * 16 + 8);
                    //一层的高度，一个chunk的宽度和深度
                    b.size = new Vector3(16, 1, 16);
                    chunkBounds.Add(b);
                }
            }
        }

        private void DrawChunkBounds(Map map)
        {
            for (int i = 0; i < chunkBounds.Count; i++)
            {
                var b = chunkBounds[i];
                var c = Handles.color;
                Handles.color = Color.green;
                Handles.DrawWireCube(b.center, b.size);
                Handles.color = c;
            }
        }
        private void CheckChunkBounds()
        {
            
        }

        private void DrawChunkRactangle(Map map)
        {
            var pos = map.gameObject.transform.position;
            for (int j = 0; j < map.depth; j++)
            {
                for (int i = 0; i < map.width; i++)
                {
                    Vector3[] verts = new Vector3[]
                    {
                        new Vector3(pos.x + i * 16, pos.y, pos.z + j * 16),
                        new Vector3(pos.x + (i + 1) * 16, pos.y, pos.z + j * 16),
                        new Vector3(pos.x + (i + 1) * 16, pos.y, pos.z + (j + 1) * 16),
                        new Vector3(pos.x + i * 16, pos.y, pos.z + (j + 1) * 16),
                    };
                    if ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0))
                    {
                        Handles.DrawSolidRectangleWithOutline(verts, new Color(1f, 1f, 1f, 0.1f),
                            new Color(1f, 1f, 1f, 0.2f));
                    }
                    else
                    {
                        Handles.DrawSolidRectangleWithOutline(verts, new Color(1f, 1f, 1f, 0.01f),
                            new Color(1f, 1f, 1f, 0.2f));
                    }
//                        Handles.DrawSolidRectangleWithOutline(verts, new Color(1f, 1f, 1f, 0.1f), new Color(0f, 1f, 0f, 1f));
                }
            }
        }

        private void DrawMapRectangle(Map map)
        {
            var pos = map.gameObject.transform.position;
            Vector3[] verts = new Vector3[]
            {
                new Vector3(pos.x, pos.y, pos.z),
                new Vector3(pos.x+map.width*16, pos.y, pos.z),
                new Vector3(pos.x+map.width*16, pos.y, pos.z+map.depth*16),
                new Vector3(pos.x, pos.y, pos.z+map.depth*16),

            };
            Handles.DrawSolidRectangleWithOutline(verts,new Color(1f,1f,1f,0.1f), new Color(0f, 1f, 0f, 1f));
        }

        private void DrawMapCube(Map map)
        {

            var pos = map.gameObject.transform.position;
            var center = new Vector3();
            center.x = pos.x + map.width / 2 * 16;
            center.y = pos.y;
            center.z = pos.z + map.depth / 2 * 16;
            Handles.DrawWireCube(center, new Vector3(map.width * 16, 1, map.depth * 16));
        }

        /// <summary>
        /// 为每个chunk绘制一个白色的线框
        /// </summary>
        /// <param name="map"></param>
        private void DrawChunkCube(Map map)
        {
            var pos = map.gameObject.transform.position;
            for (int i = 0; i < map.width; i++)
            {
                for (int j = 0; j < map.depth; j++)
                {
                    var center = new Vector3();
                    center.x = pos.x + i * 16 + 8;
                    center.y = pos.y;
                    center.z = pos.z + j * 16 + 8;
                    Handles.DrawWireCube(center, new Vector3(16, 16, 16));
                }
            }
        }
        #endregion
# region MapData操作函数

        public byte GetBlockFromData(int x, int y, int z)
        {
            Vector2Int chunkIndex = GetChunkIndexFromMapPoint(x, z);
            Vector3Int chunkPoint = GetChunkPointFromMapPoint(x, y, z);
            return data.chunkDatas[chunkIndex.x][chunkIndex.y][chunkPoint.x,chunkPoint.y,chunkPoint.z];
        }

        public void SetBlockToData(int x, int y, int z, byte blockData)
        {
            Vector2Int chunkIndex = GetChunkIndexFromMapPoint(x, z);
            Vector3Int chunkPoint = GetChunkPointFromMapPoint(x, y, z);
            data.chunkDatas[chunkIndex.x][chunkIndex.y][chunkPoint.x, chunkPoint.y, chunkPoint.z] = blockData;
        }
        /// <summary>
        /// 将地图坐标转换为Chunk的数组索引
        /// </summary>
        /// <param name="mapX">地图X</param>
        /// <param name="mapZ">地图Z</param>
        /// <returns></returns>
        private Vector2Int GetChunkIndexFromMapPoint(int mapX, int mapZ)
        {
            int x = Mathf.FloorToInt(mapX / data.width);
            int z = Mathf.FloorToInt(mapZ/ data.depth);
            return new Vector2Int(x, z);
        }
        /// <summary>
        /// 将地图坐标转换为在Chunk中的局部坐标
        /// </summary>
        /// <param name="mapX">地图X</param>
        /// <param name="mapY">地图Y</param>
        /// <param name="mapZ">地图Z</param>
        /// <returns></returns>
        private Vector3Int GetChunkPointFromMapPoint(int mapX, int mapY, int mapZ)
        {
            int x = mapX % data.width;
            int z = mapZ % data.depth;
            return new Vector3Int(x, mapY, z);
        }
#endregion
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
            if (GUILayout.Button("Create Map"))
            {
               //创建map的chunks
                for (int i = 0; i < map.transform.childCount; i++)
                {
                    DestroyImmediate(map.transform.GetChild(i).gameObject); 
                }
                map.CreateMapByEditor();
            }
            if (GUILayout.Button("Create Mesh"))
            {
                map.CreateRandomMap();
            }
            selectPanelIndex = GUILayout.Toolbar(selectPanelIndex, panelNames);
            if (panelNames[selectPanelIndex] == "Brushes")
            {
                this.DrawBrushesPanel(data);
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
        #region OnInspectorGUI相关函数
        /// <summary>
        /// 绘制笔刷页面
        /// </summary>
        /// <param name="data"></param>
        public void DrawBrushesPanel(MapData data)
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
        #endregion
    }//end class
}