using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameFramework
{
    [CustomEditor(typeof(Map))]
    public class MapEditor : Editor
    {

        public string[] panelNames = new string[] {"Brushes", "Blocks", "Layers", "Config"};
        public int selectPanelIndex =0;
        public string[] blockCreateModes = new string[] {"One Tex", "Two Tex", "Tree Tex", "Six Tex"};
        public int blockCreateModeIndex = 0;
        public BlockDefinition activeBlockDef;
        public Sprite top, bottom, left, right, front, back;
        public Vector2 blockScrollViewPos = Vector2.zero;
        public int selectBlockIndex = 0;
        public int selectLayerIndex = 0;
        //画笔
        public string[] brushNames = new string[] {"笔","油漆桶"};
        public int activeBrushIndex = 0;
        //*****************************
//        private List<Bounds> chunkBounds;
//        private List<Bounds> activeBlocksBounds;
        private bool isDirty = false;
//        private int selectedChunkIndex = -1;
//        private int selectedBlockIndex = -1;
        private bool isMouseIn = false;
        private Map map;
        private MapData data;

        private void OnEnable()
        {
            map = (Map) target;
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
//            this.DrawMapCube(map);
//            this.DrawMapRectangle(map);
            this.DrawChunkRactangle(map);
//            this.DrawChunkBounds(map);
            List<int> hitBlockBounds = new List<int>();
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            var blockPoint = GetHitBlockMapPoint(mouseRay);
            Handles.BeginGUI();
            GUI.Label(new Rect(Event.current.mousePosition, new Vector2(100, 30)), blockPoint.ToString());
            Handles.EndGUI();
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

                }
            }
        }

        #region SenceView相关函数

        /// <summary>
        /// 获取鼠标所在block的地图点坐标
        /// </summary>
        /// <param name="mouseRay">鼠标射线</param>
        /// <returns></returns>
        public Vector3Int GetHitBlockMapPoint(Ray mouseRay)
        {
            var point = new Vector3Int();
            //1、创建chunk的bounds，判断鼠标在哪个chunk
            List<ChunkBounds> chunkBounds = this.CreateChunkBounds(map);
            List<int> hitChunkBounds = new List<int>();
            for (int i = 0; i < chunkBounds.Count; i++)
            {

                if (chunkBounds[i].bounds.IntersectRay(mouseRay))
                {
                    hitChunkBounds.Add(i);
                }
            }
            if (hitChunkBounds.Count > 0)
            {
                //                Debug.Log("hit chunk!");
                float distance = 10000000;
                int activeID = -1;
                for (int i = 0; i < hitChunkBounds.Count; i++)
                {
                    var index = hitChunkBounds[i];
                    float tempDistance = (mouseRay.origin - chunkBounds[index].bounds.center).magnitude;
                    if (tempDistance < distance)
                    {
                        distance = tempDistance;
                        activeID = index;
                    }

                }
                var hitChunk = chunkBounds[activeID];
                this.DrawHitChunkBounds(hitChunk);
                var blocksBounds = this.CreateActiveChunkBlocksBounds(hitChunk);
                var hitBlock = this.CheckHitBlockBounds(mouseRay, blocksBounds);
                if (hitBlock != null)
                {
                    this.DrawHitBlockBounds(hitBlock);
                    return new Vector3Int(hitChunk.x * 16 + hitBlock.x, hitBlock.y, hitChunk.z * 16 + hitBlock.z);
                }

            }
            //2、创建所在chunk的block的bounds，判断鼠标在哪个block
            //3、转换为地图点坐标
            return new Vector3Int(-1, -1, -1);

        }

        private List<BlockBounds> CreateActiveChunkBlocksBounds(ChunkBounds activeChunkBounds)
        {
            var start = activeChunkBounds.bounds.center;
            start.x = start.x - 8;
            start.z = start.z - 8;

            var blocksBounds = new List<BlockBounds>();
            for (int j = 0; j < 16; j++)
            {
                for (int i = 0; i < 16; i++)
                {
                    var b = new Bounds();
                    b.center = new Vector3(start.x + i + 0.5f, start.y, start.z + j + 0.5f);
                    b.size = new Vector3(1, 1, 1);
                    blocksBounds.Add(new BlockBounds(i, 0, j, b));
                }
            }
            return blocksBounds;
        }

        public void DrawHitBlockBounds(BlockBounds b)
        {
            var c = Handles.color;
            Handles.color = Color.white;
            Handles.DrawWireCube(b.bounds.center, b.bounds.size);
            Handles.color = c;

        }

        public BlockBounds CheckHitBlockBounds(Ray mouseRay, List<BlockBounds> blocksBounds)
        {
            for (int i = 0; i < blocksBounds.Count; i++)
            {
                var b = blocksBounds[i].bounds;
                if (b.IntersectRay(mouseRay))
                {
                    return blocksBounds[i];
                }
            }
            return null;
        }



        private void DrawHitChunkBounds(ChunkBounds cb)
        {
            var c = Handles.color;
            Handles.color = Color.green;
            Handles.DrawWireCube(cb.bounds.center, cb.bounds.size);
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

            var center = new Vector3(pos.x + map.width / 2 * 16, 0.5f, pos.z + map.depth / 2 * 16);
            var size = new Vector3(map.width * 16, 1, map.depth * 16);
            Bounds b = new Bounds(center, size);
            return b;
        }

        private List<ChunkBounds> CreateChunkBounds(Map map)
        {
            var chunksBounds = new List<ChunkBounds>();
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
                    chunksBounds.Add(new ChunkBounds(i, j, b));
                }

            }
            return chunksBounds;
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
                new Vector3(pos.x + map.width * 16, pos.y, pos.z),
                new Vector3(pos.x + map.width * 16, pos.y, pos.z + map.depth * 16),
                new Vector3(pos.x, pos.y, pos.z + map.depth * 16),

            };
            Handles.DrawSolidRectangleWithOutline(verts, new Color(1f, 1f, 1f, 0.1f), new Color(0f, 1f, 0f, 1f));
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

        #region MapData操作函数

        /// <summary>
        /// 获取地图某个Block的数据
        /// </summary>
        /// <param name="x">地图X</param>
        /// <param name="y">地图Y</param>
        /// <param name="z">地图Z</param>
        /// <returns>byte类型数据</returns>
        public byte GetBlockFromData(int x, int y, int z)
        {
            Vector2Int chunkIndex = GetChunkIndexFromMapPoint(x, z);
            Vector3Int chunkPoint = GetChunkPointFromMapPoint(x, y, z);
            return data.chunkDatas[chunkIndex.x][chunkIndex.y][chunkPoint.x, chunkPoint.y, chunkPoint.z];
        }

        /// <summary>
        /// 设置地图某个Block的数据
        /// </summary>
        /// <param name="x">地图X</param>
        /// <param name="y">地图Y</param>
        /// <param name="z">地图Z</param>
        /// <param name="blockData">数据</param>
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
            int z = Mathf.FloorToInt(mapZ / data.depth);
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
//            base.OnInspectorGUI();
            Map map = (Map) target;
            var data = map.data;
            data = EditorGUILayout.ObjectField("Data", data, data.GetType()) as MapData;
            data.name = EditorGUILayout.TextField("Name", data.name);
            EditorGUILayout.TextField("Version", data.version);
            data.width = EditorGUILayout.IntField("Wdith(X)", data.width);
            data.height = EditorGUILayout.IntField("Height(Y)", data.height);
            data.depth = EditorGUILayout.IntField("Depth(Z)", data.depth);

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
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            activeBrushIndex= GUILayout.Toolbar(activeBrushIndex, brushNames);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
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
                MapLayer layer = new MapLayer(0, 255);
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
                    EditorGUILayout.BeginVertical((GUIStyle) "SelectionRect");
                    GUILayout.Toggle(false, string.Format("{0}:Start:{1}-End:{2}", i, layer.start, layer.end),
                        (GUIStyle) "TL SelectionBarPreview", GUILayout.ExpandWidth(true));
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Start");
                    layer.start = EditorGUILayout.IntField(layer.start);
                    GUILayout.Label("End  ");
                    layer.end = EditorGUILayout.IntField(layer.end);
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
                        item.blockId = EditorGUILayout.IntPopup(item.blockId, GetBlockNames(data), GetBlockIDs(data));
                        GUILayout.Label("Weight ");
                        item.weight = EditorGUILayout.FloatField(item.weight);
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
                    var flag = GUILayout.Toggle(false, string.Format("{0}:Start:{1}-End:{2}", i, layer.start, layer.end),
                        (GUIStyle) "OL Title");
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
    } //end class

    public class ChunkBounds
    {
        public int x;
        public int z;
        public Bounds bounds;

        public ChunkBounds()
        {
            bounds = new Bounds();
        }

        public ChunkBounds(int setX, int setZ, Bounds setBounds)
        {
            x = setX;
            z = setZ;
            bounds = setBounds;
        }
    }

    public class BlockBounds
    {
        public int x;
        public int y;
        public int z;
        public Bounds bounds;

        public BlockBounds()
        {
            bounds = new Bounds();
        }

        public BlockBounds(int setX, int setY, int setZ, Bounds setBounds)
        {
            x = setX;
            y = setY;
            z = setZ;
            bounds = setBounds;
        }
    }
}