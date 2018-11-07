using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;

namespace GameDesigner
{
    [ExecuteInEditMode]
    public class Chunk : MonoBehaviour
    {
        public int chunkSize = 16;
        public Color color1 = new Color(0.5f, 0.5f, 0.5f, 1f);
        public Color color2 = new Color(0.6f, 0.6f, 0.6f, 1f);
        public List<Block> blocks = new List<Block>();
        public List<BlockInfo> blockInfos = new List<BlockInfo>();
        public Block activeBlock;
        public int selectedBlockID = -1;
        private int xslide = 16;
        public bool showGrid = true;
        public bool isDirty = false;
        public BlockPivotType pivotType = BlockPivotType.Center;
        private GridMode gridMode = GridMode.PanelZX;

        [ShowInInspector]
        public GridMode GridMode
        {
            get { return gridMode; }
            set
            {
                gridMode = value;
                switch (gridMode)
                {
                    case GridMode.Cube:
                        xslide = yslide = zslide = 16;
                        break;
                    case GridMode.PanelXY:
                        xslide = yslide = 16;
                        zslide = 8;
                        break;
                    case GridMode.PanelYZ:
                        yslide = zslide = 16;
                        xslide = 8;
                        break;
                    case GridMode.PanelZX:
                        xslide = zslide = 16;
                        yslide = 8;
                        break;
                }
                isDirty = true;
            }
        }

        [ShowInInspector, PropertyRange(1, 16)]
        public int XSlide
        {
            get { return xslide; }
            set
            {
                xslide = value;
                isDirty = true;
            }
        }

        private int yslide = 16;

        [ShowInInspector, PropertyRange(1, 16)]
        public int YSlide
        {
            get { return yslide; }
            set
            {
                yslide = value;
                isDirty = true;
            }
        }

        private int zslide = 16;

        [ShowInInspector, PropertyRange(1, 16)]
        public int ZSlide
        {
            get { return zslide; }
            set
            {
                zslide = value;
                isDirty = true;
            }
        }

        public List<BlockBounds> checkBounds = new List<BlockBounds>();
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnDrawGizmos()
        {
        }

        private void OnDrawGizmosSelected()
        {
            if (Selection.activeGameObject != this.gameObject || blockInfos.Count == 0 || showGrid == false)
            {
                return;
            }
            foreach (var bb in checkBounds)
            {
                Color oldColor = Gizmos.color;
                if (selectedBlockID != -1 && selectedBlockID == bb.id)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(blockInfos[bb.id].position, new Vector3(0.99f, 0.99f, 0.99f));
                    Gizmos.color = oldColor;
                }
                Gizmos.DrawWireCube(blockInfos[bb.id].position, new Vector3(1f, 1f, 1f));
            }
        }

        [ButtonGroup("Create Blocks")]
        public void CreateBlocks()
        {
            this.ClearBlocks();
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    for (int x = 0; x < chunkSize; x++)
                    {
                        //创建block info
                        int id = x + z * chunkSize + y * chunkSize * chunkSize;
                        BlockInfo info = new BlockInfo(id);
                        info.x = x;
                        info.y = y;
                        info.z = z;
                        info.position = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);
//                    switch (pivotType)
//                    {
//                        case BlockPivotType.Top: info.position = new Vector3(x + 0.5f, y+0.5f, z + 0.5f);
//                            break;
//                        case BlockPivotType.Center:
//                            info.position = new Vector3(x + 0.5f, y, z + 0.5f);
//                            break;
//                        case BlockPivotType.Bottom:
//                            info.position = new Vector3(x + 0.5f, y - 0.5f, z + 0.5f);
//                            break;
//                    }

                        this.blockInfos.Add(info);
                    }
                }
            }
            this.CaculateCheckBounds();
        }

        [ButtonGroup("Clear Blocks")]
        public void ClearBlocks()
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                DestroyImmediate(blocks[i].gameObject);
            }
            blocks.Clear();
            //清空块信息表
            blockInfos.Clear();
        }

        [ButtonGroup("Build Blocks")]
        public void BuildBlocks()
        {
        }

        public Block FindOrCreateBlock(int x, int y, int z)
        {
            int bid = GetBlockIndexByXYZ(x, y, z);
            var block = blocks.Find(b => b.index == bid);
            if (block == null)
            {
                GameObject go = new GameObject("Block_" + x.ToString() + "_" + y.ToString() + "_" + z.ToString());
                go.transform.SetParent(transform);
                BlockInfo info = blockInfos[GetBlockIndexByXYZ(x, y, z)];
                go.transform.localPosition = info.position;
                switch (pivotType)
                {
                    case BlockPivotType.Top:
                        go.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);
                        break;
                    case BlockPivotType.Center:
                        go.transform.localPosition = new Vector3(x + 0.5f, y, z + 0.5f);
                        break;
                    case BlockPivotType.Bottom:
                        go.transform.localPosition = new Vector3(x + 0.5f, y - 0.5f, z + 0.5f);
                        break;
                    default:
                        break;
                }
                block = go.AddComponent<Block>();
                block.index = info.id;
                this.blocks.Add(block);
            }
            return block;
        }

        /// <summary>
        /// 计算需要射线检测的block的bound
        /// </summary>
        public void CaculateCheckBounds()
        {
            Debug.Log("Start CaculateCheckBounds");
            checkBounds.Clear();
            if (gridMode == GridMode.Cube)
            {
                //一共六个面
                //正面 和 后面
                for (int y = 0; y < yslide; y++)
                {
                    for (int x = 0; x < xslide; x++)
                    {
                        //正面， z=0
                        int bid1 = GetBlockIndexByXYZ(x, y, 0);
                        Bounds bounds1 = new Bounds(blockInfos[bid1].position, new Vector3(1, 1, 1));
                        checkBounds.Add(new BlockBounds(bid1, bounds1));
                        //后面， z=zslide
                        int bid2 = GetBlockIndexByXYZ(x, y, zslide - 1);
                        Bounds bounds2 = new Bounds(blockInfos[bid2].position, new Vector3(1, 1, 1));
                        checkBounds.Add(new BlockBounds(bid2, bounds2));
                    }
                }
                //左面 和 右面
                for (int y = 0; y < yslide; y++)
                {
                    for (int z = 0; z < zslide; z++)
                    {
                        //正面， x=0
                        int bid1 = GetBlockIndexByXYZ(0, y, z);
                        Bounds bounds1 = new Bounds(blockInfos[bid1].position, new Vector3(1, 1, 1));
                        checkBounds.Add(new BlockBounds(bid1, bounds1));
                        //后面， x=xslide
                        int bid2 = GetBlockIndexByXYZ(xslide - 1, y, z);
                        Bounds bounds2 = new Bounds(blockInfos[bid2].position, new Vector3(1, 1, 1));
                        checkBounds.Add(new BlockBounds(bid2, bounds2));
                    }
                }
                //上面 和 下面
                for (int z = 0; z < zslide; z++)
                {
                    for (int x = 0; x < xslide; x++)
                    {
                        //正面，y=0
                        int bid1 = GetBlockIndexByXYZ(x, 0, z);
                        Bounds bounds1 = new Bounds(blockInfos[bid1].position, new Vector3(1, 1, 1));
                        checkBounds.Add(new BlockBounds(bid1, bounds1));
                        //后面，y=yslide
                        int bid2 = GetBlockIndexByXYZ(x, yslide - 1, z);
                        Bounds bounds2 = new Bounds(blockInfos[bid2].position, new Vector3(1, 1, 1));
                        checkBounds.Add(new BlockBounds(bid2, bounds2));
                    }
                }
            }
            else if (gridMode == GridMode.PanelXY)
            {
                //正面
                for (int y = 0; y < yslide; y++)
                {
                    for (int x = 0; x < xslide; x++)
                    {
                        int bid2 = GetBlockIndexByXYZ(x, y, zslide - 1);
                        Bounds bounds2 = new Bounds(blockInfos[bid2].position, new Vector3(1, 1, 1));
                        checkBounds.Add(new BlockBounds(bid2, bounds2));
                    }
                }
            }
            else if (gridMode == GridMode.PanelYZ)
            {
                //侧面
                for (int y = 0; y < yslide; y++)
                {
                    for (int z = 0; z < zslide; z++)
                    {
                        int bid2 = GetBlockIndexByXYZ(xslide - 1, y, z);
                        Bounds bounds2 = new Bounds(blockInfos[bid2].position, new Vector3(1, 1, 1));
                        checkBounds.Add(new BlockBounds(bid2, bounds2));
                    }
                }
            }
            else if (gridMode == GridMode.PanelZX)
            {
                //俯视面
                for (int z = 0; z < zslide; z++)
                {
                    for (int x = 0; x < xslide; x++)
                    {
                        int bid2 = GetBlockIndexByXYZ(x, yslide - 1, z);
                        Bounds bounds2 = new Bounds(blockInfos[bid2].position, new Vector3(1, 1, 1));
                        checkBounds.Add(new BlockBounds(bid2, bounds2));
                    }
                }
            }
        }

        /// <summary>
        /// 根据X，Y，Z坐标获取block索引
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="z">Z坐标</param>
        /// <returns>列表索引</returns>
        public int GetBlockIndexByXYZ(int x, int y, int z)
        {
            return x + z * chunkSize + y * chunkSize * chunkSize;
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            if (isDirty)
            {
                this.CaculateCheckBounds();
                isDirty = false;
            }
            if (Selection.activeGameObject != this.gameObject)
            {
                return;
            }
            if (BlockBrush.activeBrush != null)
            {
                Handles.BeginGUI();
                GUILayout.Label("当前笔刷:" + BlockBrush.activeBrush.name);
                GUILayout.Label("旋转角度:" + BlockBrush.activeBrush.rotation);
                Handles.EndGUI();
            }
            var e = Event.current;
            if (e.type == EventType.KeyUp)
            {
                if (e.keyCode == KeyCode.Space)
                {
                    BlockBrush.activeBrush.Rotate();
                    e.Use();
                }
                else if (e.keyCode == KeyCode.LeftControl)
                {
                    showGrid = !showGrid;
                    e.Use();
                }
            }
            if (selectedBlockID != -1)
            {
                if (e.type == EventType.MouseUp || e.type == EventType.MouseDrag)
                {
                    if (e.button == 0 && e.control == false && e.alt == false && e.shift == false)
                    {
                        //左键添加block
                        if (BlockBrush.activeBrush != null)
                        {
                            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(BlockBrush.activeBrush.prefab);
                            var go = Instantiate(prefab, Vector3.zero,
                                Quaternion.AngleAxis(BlockBrush.activeBrush.rotation, Vector3.up));
                            var selectBlockInfo = this.blockInfos[selectedBlockID];
                            var block = FindOrCreateBlock(selectBlockInfo.x, selectBlockInfo.y, selectBlockInfo.z);
                            block.AddBlock(go);

                            DestroyImmediate(go);
                        }
                        e.Use();
                    }
                    else if (e.button == 1 && e.control == false && e.alt == false && e.shift == false)
                    {
                        //右键删除block
                        var selectBlockInfo = this.blockInfos[selectedBlockID];
                        for (int i = 0; i < blocks.Count; i++)
                        {
                            if (blocks[i].index == selectBlockInfo.id)
                            {
                                DestroyImmediate(blocks[i].gameObject);
                                blocks.RemoveAt(i);
                                break;
                            }
                        }
                        e.Use();
                    }

                }
            }
            List<BlockBounds> hitBlockBounds = new List<BlockBounds>();
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            for (int i = 0; i < checkBounds.Count; i++)
            {
                if (checkBounds[i].bounds.IntersectRay(mouseRay))
                {
                    hitBlockBounds.Add(checkBounds[i]);
//                activeBlock = this.blocks[checkBounds[i].id];
//                break;
                }
            }
            if (hitBlockBounds.Count > 0)
            {
                float distance = 10000000;
                int activeID = -1;
                for (int i = 0; i < hitBlockBounds.Count; i++)
                {
                    float tempDistance = (mouseRay.origin - hitBlockBounds[i].bounds.center).magnitude;
                    if (tempDistance < distance)
                    {
                        distance = tempDistance;
                        activeID = i;
                    }
                }
//            activeBlock = this.blocks[hitBlockBounds[activeID].id];
                this.selectedBlockID = hitBlockBounds[activeID].id;
            }
            else
            {
//            activeBlock = null;
                this.selectedBlockID = -1;
            }
        }

        void OnEnable()
        {
            SceneView.onSceneGUIDelegate += this.OnSceneGUI;
        }

        void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        }
    }

    public enum BlockPivotType
    {
        Top,
        Center,
        Bottom
    };

    public enum GridMode
    {
        PanelZX,
        PanelXY,
        PanelYZ,
        Cube,
    }
}