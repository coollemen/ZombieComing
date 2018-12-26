using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
namespace GameFramework
{
    public class BlockTerrain : MonoSingleton<BlockTerrain>
    {
        public int width = 64;
        public int height = 64;
        public int depth = 64;
        public BlockTerrainData data;
        public int viewWidth = 9;
        public int viewDepth = 9;

//        public Chunk[,] chunks;
        public Chunk[,] viewChunks;
        public GameObject chunkPrefab;

        public Dictionary<byte, Block> blockPool = new Dictionary<byte, Block>();
        #region 属性
        public int Width
        {
            get
            {
                if (this.data == null)
                {
                    return -1;
                }
                else
                {
                   return this.data.width;
                }
            }
            set
            {
                if (this.data != null)
                {
                    this.data.width = value;
                }
            }
        }
        public int Height
        {
            get
            {
                if (this.data == null)
                {
                    return -1;
                }
                else
                {
                    return this.data.height;
                }
            }
            set
            {
                if (this.data != null)
                {
                    this.data.height = value;
                }
            }
        }
        public int Depth
        {
            get
            {
                if (this.data == null)
                {
                    return -1;
                }
                else
                {
                    return this.data.depth;
                }
            }
            set
            {
                if (this.data != null)
                {
                    this.data.depth = value;
                }
            }
        }
        #endregion
        // Use this for initialization
        void Start()
        {
//            chunks = new Chunk[width, depth];
            viewChunks = new Chunk[viewWidth, viewDepth];
        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// 创建地形
        /// </summary>
        public void CreateTerrain()
        {

        }
        /// <summary>
        /// 装载地形数据
        /// </summary>
        public IEnumerator LoadTerrainAsyn()
        {
            //读取数据
            this.CreateBlocks();
            //初始化chunk数组
            viewChunks = new Chunk[this.Width, this.Depth];
            //创建chunk子物体
            for (int j = 0; j < depth; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    //地图原点在左下角
                    var go = Instantiate(chunkPrefab);
                    go.transform.SetParent(this.transform);
                    go.transform.position =
                        new Vector3(transform.position.x + i * 16 + 8, transform.position.y,
                            transform.position.z + j * 16 + 8);
                    go.name = string.Format("Chunk_{0}_{1}", i, j);
                    viewChunks[i, j] = go.GetComponent<Chunk>();
                    //获取chunk的数据
                    viewChunks[i, j].data = this.data.chunkDatas[j][i];
                    yield return StartCoroutine(viewChunks[i, j].CreateChunkMeshAsyn());
                }
            }
        }
        /// <summary>
        ///更新地形数据
        /// </summary>
        public IEnumerator UpdateTerrainAsyn()
        {
            //创建chunk子物体
            for (int j = 0; j < depth; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    yield return StartCoroutine(viewChunks[i, j].UpdateChunkMeshAsyn());
                }
            }
        }
        /// <summary>
        /// 保存地形数据
        /// </summary>
        public void SaveTerrain()
        {

        }

        /// <summary>
        /// 根据地图块定义创建block
        /// </summary>
        public void CreateBlocks()
        {
            blockPool.Clear();
            foreach (var def in data.blockDefinitions)
            {
                Block b = new Block((byte)def.id, def.name, def.top.uv, def.bottom.uv, def.left.uv, def.right.uv, def.front.uv,
                    def.back.uv);
                this.blockPool.Add(b.id, b);
            }
        }

        /// <summary>
        /// 获取指定id的block
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>地图块</returns>
        public  Block GetBlock(byte id)
        {
            return blockPool.ContainsKey(id) ? blockPool[id] : null;
        }

        /// <summary>
        /// 在地图指定位置设置地图块（Block）
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="z">Z坐标</param>
        /// <param name="block">地图块</param>
        public void SetBlockByMapPoint(int x, int y, int z, byte block)
        {
            int mapX = Mathf.FloorToInt(x / 16);
            int mapZ = Mathf.FloorToInt(z / 16);
            var chunk = viewChunks[mapX, mapZ];
            //判断Chunk是否正确
            if (chunk == null)
            {
                Debug.LogError("未找到指定Chunk，查找的坐标有误！");
            }
            int chunkX = x % 16;
            int chunkY = y % 16;
            int chunkZ = z % 16;
             chunk.SetBlockByChunkPoint(chunkX, chunkY, chunkZ,block);
        }
        /// <summary>
        /// 获取地图指定位置地图块（Block）
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="z">Z坐标</param>
        /// <returns>地图块</returns>
        public int GetBlockByMapPoint(int x, int y, int z)
        {
            int mapX = Mathf.FloorToInt(x / 16);
            int mapZ = Mathf.FloorToInt(z / 16);
            var chunk = viewChunks[mapX, mapZ];
            //判断Chunk是否正确
            if (chunk == null)
            {
                Debug.LogError("未找到指定Chunk，查找的坐标有误！");
                return -1;
            }
            int chunkX = x % 16;
            int chunkY = y % 16;
            int chunkZ = z % 16;
            return chunk.GetBlockByChunkPoint(chunkX, chunkY, chunkZ);
        }
        public void InitViewChunks()
        {

        }

        public void ViewMoveUp()
        {

        }

        public void ViewMoveDown()
        {

        }

        public void ViewMoveLeft()
        {

        }

        public void ViewMoveRight()
        {

        }
        private void OnGUI()
        {
            if (GUILayout.Button("Left Chunk"))
            {

            }
            if (GUILayout.Button("Right Chunk"))
            {

            }
            if (GUILayout.Button("Top Chunk"))
            {

            }
            if (GUILayout.Button("Bottom Chunk"))
            {

            }
        }
    }
}