using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
namespace GameFramework
{
    public class BlockTerrain : MonoSingleton<BlockTerrain>
    {
        public int width = 64;
        public int depth = 64;
        public int viewWidth = 9;
        public int viewDepth = 9;
        public Chunk[,] chunks;
        public Chunk[,] viewChunks;
        public GameObject chunkPrefab;
        public BlockTerrainData data;
        public Dictionary<byte, Block> blocks = new Dictionary<byte, Block>();
        // Use this for initialization
        void Start()
        {
            chunks = new Chunk[width, depth];
            viewChunks = new Chunk[viewWidth, viewDepth];
            CreateRandomMap();
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void CreateMap()
        {

        }
        public void LoadMap()
        {

        }

        public void SaveMap()
        {

        }

        public IEnumerator CreateMapByEditor()
        {
            this.CreateBlocks();
            viewChunks = new Chunk[width, depth];
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
                    yield return StartCoroutine(viewChunks[i, j].CreateChunkMesh());

                }
            }
        }
        /// <summary>
        /// 根据地图块定义创建block
        /// </summary>
        public void CreateBlocks()
        {
            blocks.Clear();
            foreach (var def in data.blockDefinitions)
            {
                Block b = new Block((byte)def.id, def.name, def.top.uv, def.bottom.uv, def.left.uv, def.right.uv, def.front.uv,
                    def.back.uv);
                this.blocks.Add(b.id, b);
            }
        }

        public Vector2[] GetUvFromSprite(Sprite spr)
        {
            return null;
        }
        /// <summary>
        /// 获取指定id的block
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>地图块</returns>
        public  Block GetBlock(byte id)
        {
            return blocks.ContainsKey(id) ? blocks[id] : null;
        }
        /// <summary>
        /// 创建随机地图
        /// </summary>
        public void CreateRandomMap()
        {
            viewChunks = new Chunk[viewWidth, viewDepth];
            for (int i = 0; i < viewWidth; i++)
            {
                for (int j = 0; j < viewDepth; j++)
                {
                    //地图原点在左下角
                    var go = Instantiate(chunkPrefab);
                    go.transform.SetParent(this.transform);
                    go.transform.position =
                        new Vector3(transform.position.x+i*16+8, transform.position.y, transform.position.z+j*16+8);
                    go.name = string.Format("Chunk_{0}_{1}", i, j);
                    viewChunks[i, j] = go.GetComponent<Chunk>();                  
                }
            }
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
            var chunk = chunks[mapX, mapZ];
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
            var chunk = chunks[mapX, mapZ];
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