using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
[ExecuteInEditMode]
public class Chunk : MonoBehaviour
{
    public int chunkSize = 16;
    public Color color1 = new Color(0.5f, 0.5f, 0.5f, 1f);
    public Color color2 = new Color(0.6f, 0.6f, 0.6f, 1f);
    public List<Block> blocks = new List<Block>();
    public Block activeBlock;
    private int xslide = 16;
    public bool showGrid = true;
    [ShowInInspector, PropertyRange(1, 16)]
    public int XSlide
    {
        get { return xslide; }
        set
        {
            xslide = value;
            this.CaculateCheckBounds();
        }
    }
    private int yslide = 16;
    [ShowInInspector, PropertyRange(1,16)]
    public int YSlide
    {
        get { return yslide; }
        set
        {
            yslide = value;
            this.CaculateCheckBounds();
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
            this.CaculateCheckBounds();
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
        if (Selection.activeGameObject != this.gameObject|| blocks.Count == 0||showGrid==false)
        {
            return;
        }
        foreach (var bb in checkBounds)
        {
            Color oldColor = Gizmos.color;
            if (activeBlock != null && activeBlock.index == bb.id)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(blocks[bb.id].transform.position, new Vector3(0.99f, 0.99f, 0.99f));
                Gizmos.color = oldColor;
            }
            Gizmos.DrawWireCube(blocks[bb.id].transform.position, new Vector3(1f, 1f, 1f));
        }
//        for (int y = 0; y < yslide; y++)
//        {
//            for (int z = 0; z < zslide; z++)
//            {
//                for (int x = 0; x < xslide; x++)
//                {
//
//                    int blockIndex = GetBlockIndexByXYZ(x, y, z);
//                    Color oldColor = Gizmos.color;
//                    if (activeBlock != null && activeBlock.index == blockIndex)
//                    {
//                        Gizmos.color = Color.green;
//                        Gizmos.DrawCube(blocks[blockIndex].transform.position, new Vector3(0.99f, 0.99f, 0.99f));
//                        Gizmos.color = oldColor;
//                    }
//                        Gizmos.DrawWireCube(blocks[blockIndex].transform.position, new Vector3(1f, 1f, 1f));
//                }
//            }
//        }
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
                    GameObject go = new GameObject("Block_"+x.ToString()+"_"+y.ToString()+"_"+z.ToString());
                    go.transform.SetParent(transform);
                    go.transform.localPosition = new Vector3(x+0.5f, y + 0.5f, z + 0.5f);
                    var block = go.AddComponent<Block>();
                    block.index = this.blocks.Count;
                    this.blocks.Add(block);
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
    }

    public void AddBlock(int x, int y, int z, GameObject go)
    {
    }

    public void RemoveBlock(int x, int y, int z)
    {
    }
    /// <summary>
    /// 计算需要射线检测的block的bound
    /// </summary>
    public void CaculateCheckBounds()
    {
        checkBounds.Clear();
        //一共六个面
        //正面 和 后面
        for (int y = 0; y < yslide; y++)
        {
            for (int x = 0; x < xslide; x++)
            {
                //正面， z=0
                int bid1 = GetBlockIndexByXYZ(x, y, 0);
                Bounds bounds1 = new Bounds(blocks[bid1].transform.position, new Vector3(1, 1, 1));
                checkBounds.Add(new BlockBounds(bid1,bounds1));
                //后面， z=zslide
                int bid2 = GetBlockIndexByXYZ(x, y, zslide-1);
                Bounds bounds2 = new Bounds(blocks[bid2].transform.position, new Vector3(1, 1, 1));
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
                Bounds bounds1 = new Bounds(blocks[bid1].transform.position, new Vector3(1, 1, 1));
                checkBounds.Add(new BlockBounds(bid1, bounds1));
                //后面， x=xslide
                int bid2 = GetBlockIndexByXYZ(xslide-1, y, z);
                Bounds bounds2 = new Bounds(blocks[bid2].transform.position, new Vector3(1, 1, 1));
                checkBounds.Add(new BlockBounds(bid2, bounds2));
            }
        }
        //上面 和 下面
        for (int z = 0; z< zslide; z++)
        {
            for (int x = 0; x < xslide; x++)
            {
                //正面，y=0
                int bid1 = GetBlockIndexByXYZ(x, 0, z);
                Bounds bounds1 = new Bounds(blocks[bid1].transform.position, new Vector3(1, 1, 1));
                checkBounds.Add(new BlockBounds(bid1, bounds1));
                //后面，y=yslide
                int bid2 = GetBlockIndexByXYZ(x, yslide-1, z);
                Bounds bounds2 = new Bounds(blocks[bid2].transform.position, new Vector3(1, 1, 1));
                checkBounds.Add(new BlockBounds(bid2, bounds2));
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
        return x + 16 * z + y * 256;
    }
    public void OnSceneGUI(SceneView sceneView)
    {
        if (Selection.activeGameObject != this.gameObject)
        {
            return;
        }
        if (BlockBrush.activeBrush != null )
        {
            Handles.BeginGUI();
            GUILayout.Label("当前笔刷:" + BlockBrush.activeBrush.name);
            GUILayout.Label("旋转角度:" + BlockBrush.activeBrush.rotation);
            Handles.EndGUI();
        }
        var e = Event.current;
        if (e.type==EventType.KeyUp )
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
        if (activeBlock != null && e.type == EventType.MouseUp)
        {
            if (e.button == 0)
            {
                //左键添加block
                if (BlockBrush.activeBrush != null)
                {
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(BlockBrush.activeBrush.prefab);
                    var go = Instantiate(prefab, Vector3.zero,
                        Quaternion.AngleAxis(BlockBrush.activeBrush.rotation, Vector3.up));
                    activeBlock.AddBlock(go);
                    DestroyImmediate(go);
                }
            }
            else if (e.button == 1)
            {
                //右键删除block
                activeBlock.RemoveBlock();

            }
        }
        List<BlockBounds> hitBlockBounds = new List<BlockBounds>();
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        for (int i=0;i<checkBounds.Count;i++)
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
            activeBlock = this.blocks[hitBlockBounds[activeID].id];
        }
        else
        {
            activeBlock = null;
        }
    }
    void OnEnable()
    {
//        this.CaculateCheckBounds();
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }
}