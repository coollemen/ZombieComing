using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 图块物体
    /// </summary>
    public class BlockObject : MonoBehaviour
    {
        /// <summary>
        /// 数据
        /// </summary>
        public BlockObjectData data;
        /// <summary>
        /// 图块池
        /// </summary>
        public Dictionary<byte, Block> blockPool;
        /// <summary>
        /// 地图块的三维数组，每个块数据类型为byte，
        /// 存储block的id,如果为0，代表空
        /// </summary>
        public byte[,,] blocks;

        /// <summary>
        /// 多边形
        /// </summary>
        public Mesh mesh;

        /// <summary>
        ///顶点列表
        /// </summary>
        private List<Vector3> vertices = new List<Vector3>();

        /// <summary>
        ///三角形面顶点索引列表
        /// </summary>
        private List<int> triangles = new List<int>();

        /// <summary>
        ///所有的uv信息
        /// </summary>
        private List<Vector2> uv = new List<Vector2>();

        /// <summary>
        ///让UV稍微缩小一点，避免出现它旁边的贴图
        /// </summary>
        public static float shrinkSize = 0.001f;

        /// <summary>
        ///是否需要更新Mesh
        /// </summary>
        public bool isDirty = false;

        /// <summary>
        ///当前是否正在生成中
        /// </summary>
        private bool isWorking = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// 将数据读取到组件
        /// </summary>
        public void LoadFromData()
        {
            this.blocks = new byte[data.Width, data.Height, data.Depth];
            for (int z = 0; z < data.Depth; z++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    for (int x = 0; x < data.Width; x++)
                    {
                        blocks[x, y, z] = data.blocks[x, y, z];
                    }
                }
            }
        }
        /// <summary>
        /// 将组件数据保存
        /// </summary>
        public void SaveToData()
        {
            for (int z = 0; z < data.Depth; z++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    for (int x = 0; x < data.Width; x++)
                    {
                        data.blocks[x, y, z] = blocks[x, y, z];
                    }
                }
            }
        }
        /// <summary>
        /// 获取图块
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>图块</returns>
        public Block GetBlock(byte id)
        {
            return this.blockPool.ContainsKey(id) ? blockPool[id] : null;
        }
        /// <summary>
        /// 初始化地图块,每个block的值为0
        /// </summary>
        public void InitBlocks()
        {
            blocks = data.blocks;
        }
        /// <summary>
        /// 创建地图块
        /// </summary>
        /// <param name="data">断面数据</param>
        public void SetBlocks(BlockObjectData data)
        {
            this.blocks = data.blocks;
            isDirty = true;
        }
        /// <summary>
        /// 异步创建Mesh
        /// </summary>
        /// <returns></returns>
        public IEnumerator CreateMeshAsyn()
        {
            while (isWorking)
            {
                yield return null;
            }

            isWorking = true;
            //创建顶点、索引、uv
            //初始化列表
            mesh = new Mesh();
            mesh.name = data.name;
            vertices.Clear();
            triangles.Clear();
            uv.Clear();
            //把所有面的点和面的索引添加进去
            for (int x = 0; x <data.Width; x++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    for (int z = 0; z < data.Depth; z++)
                    {
                        //获取当前坐标的Block对象
                        Block block = this.GetBlock((byte)(this.blocks[x, y, z] - 1));
                        if (block == null) continue;
                        if (IsBlockTransparent(x + 1, y, z))
                        {
                            AddRightFace(x, y, z, block);
                        }

                        if (IsBlockTransparent(x - 1, y, z))
                        {
                            AddLeftFace(x, y, z, block);
                        }

                        if (IsBlockTransparent(x, y, z + 1))
                        {
                            AddBackFace(x, y, z, block);
                        }

                        if (IsBlockTransparent(x, y, z - 1))
                        {
                            AddFrontFace(x, y, z, block);
                        }

                        if (IsBlockTransparent(x, y + 1, z))
                        {
                            AddTopFace(x, y, z, block);
                        }

                        if (IsBlockTransparent(x, y - 1, z))
                        {
                            AddBottomFace(x, y, z, block);
                        }
                    }
                }
            }

            //为点和index赋值
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            isWorking = false;
        }

        //此坐标方块是否透明，Chunk中的局部坐标
        public bool IsBlockTransparent(int x, int y, int z)
        {
            if (x >= data.Width || y >= data.Height || z >= data.Depth || x < 0 || y < 0 || z < 0)
            {
                return true;
            }
            else
            {
                //如果当前方块的id是0，那的确是透明的
                return this.blocks[x, y, z] == 0;
            }
        }

        /// <summary>
        /// 在指定位置创建指定类型的地图块
        /// </summary>
        /// <param name="x">x位置</param>
        /// <param name="y">y位置</param>
        /// <param name="z">z位置</param>
        /// <param name="blockID">地图类型ID</param>
        public void SetBlock(int x, int y, int z, byte blockID)
        {
            this.blocks[x, y, z] = blockID;
            isDirty = true;
        }

        /// <summary>
        /// 删除指定位置的地图块
        /// </summary>
        /// <param name="x">x位置</param>
        /// <param name="y">y位置</param>
        /// <param name="z">z位置</param>
        public void DeleteBlock(int x, int y, int z)
        {
            this.blocks[x, y, z] = 0;
            isDirty = true;
        }
        #region 创建各个面

        //左面
        void AddLeftFace(int x, int y, int z, Block block)
        {
            var p = GetCubePivot(x, y, z);
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            //            vertices.Add(new Vector3(-1 + x, 0 + y, 1 + z));
            //            vertices.Add(new Vector3(-1 + x, 0 + y, 0 + z));
            //            vertices.Add(new Vector3(-1 + x, 1 + y, 0 + z));
            //            vertices.Add(new Vector3(-1 + x, 1 + y, 1 + z));

            vertices.Add(new Vector3(-0.5f + p.x, -0.5f + p.y, 0.5f + p.z));
            vertices.Add(new Vector3(-0.5f + p.x, -0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(-0.5f + p.x, 0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(-0.5f + p.x, 0.5f + p.y, 0.5f + p.z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.uvLeft[3].x + shrinkSize, block.uvLeft[3].y + shrinkSize));
            uv.Add(new Vector2(block.uvLeft[1].x - shrinkSize, block.uvLeft[1].y + shrinkSize));
            uv.Add(new Vector2(block.uvLeft[2].x - shrinkSize, block.uvLeft[2].y - shrinkSize));
            uv.Add(new Vector2(block.uvLeft[0].x + shrinkSize, block.uvLeft[0].y - shrinkSize));
        }

        //右面
        void AddRightFace(int x, int y, int z, Block block)
        {
            var p = GetCubePivot(x, y, z);
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            //            vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));
            //            vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
            //            vertices.Add(new Vector3(0 + x, 1 + y, 1 + z));
            //            vertices.Add(new Vector3(0 + x, 1 + y, 0 + z));

            vertices.Add(new Vector3(0.5f + p.x, -0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, -0.5f + p.y, 0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, 0.5f + p.y, 0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, 0.5f + p.y, -0.5f + p.z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.uvRight[3].x + shrinkSize, block.uvRight[3].y + shrinkSize));
            uv.Add(new Vector2(block.uvRight[1].x - shrinkSize, block.uvRight[1].y + shrinkSize));
            uv.Add(new Vector2(block.uvRight[2].x - shrinkSize, block.uvRight[2].y - shrinkSize));
            uv.Add(new Vector2(block.uvRight[0].x + shrinkSize, block.uvRight[0].y - shrinkSize));
        }


        //后面
        void AddBackFace(int x, int y, int z, Block block)
        {
            var p = GetCubePivot(x, y, z);
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            //            vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
            //            vertices.Add(new Vector3(-1 + x, 0 + y, 1 + z));
            //            vertices.Add(new Vector3(-1 + x, 1 + y, 1 + z));
            //            vertices.Add(new Vector3(0 + x, 1 + y, 1 + z));

            vertices.Add(new Vector3(0.5f + p.x, -0.5f + p.y, 0.5f + p.z));
            vertices.Add(new Vector3(-0.5f + p.x, -0.5f + p.y, 0.5f + p.z));
            vertices.Add(new Vector3(-0.5f + p.x, 0.5f + p.y, 0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, 0.5f + p.y, 0.5f + p.z));
            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.uvBack[3].x + shrinkSize, block.uvBack[3].y + shrinkSize));
            uv.Add(new Vector2(block.uvBack[1].x - shrinkSize, block.uvBack[1].y + shrinkSize));
            uv.Add(new Vector2(block.uvBack[2].x - shrinkSize, block.uvBack[2].y - shrinkSize));
            uv.Add(new Vector2(block.uvBack[0].x + shrinkSize, block.uvBack[0].y - shrinkSize));
        }

        private Vector3 GetCubePivot(int x, int y, int z)
        {
            return new Vector3(x - data.Width / 2 + 0.5f, y - data.Height / 2 + 0.5f, z - data.Depth / 2 + 0.5f);
        }

        //前面
        void AddFrontFace(int x, int y, int z, Block block)
        {
            var p = GetCubePivot(x, y, z);
            //第一个三角面
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);

            //第二个三角面
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);


            //添加4个点
            vertices.Add(new Vector3(-0.5f + p.x, -0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, -0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, 0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(-0.5f + p.x, 0.5f + p.y, -0.5f + p.z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.uvFront[3].x + shrinkSize, block.uvFront[3].y + shrinkSize));
            uv.Add(new Vector2(block.uvFront[1].x - shrinkSize, block.uvFront[1].y + shrinkSize));
            uv.Add(new Vector2(block.uvFront[2].x - shrinkSize, block.uvFront[2].y - shrinkSize));
            uv.Add(new Vector2(block.uvFront[0].x + shrinkSize, block.uvFront[0].y - shrinkSize));
        }

        //上面
        void AddTopFace(int x, int y, int z, Block block)
        {
            var p = GetCubePivot(x, y, z);
            //第一个三角面
            triangles.Add(1 + vertices.Count);
            triangles.Add(0 + vertices.Count);
            triangles.Add(3 + vertices.Count);

            //第二个三角面
            triangles.Add(3 + vertices.Count);
            triangles.Add(2 + vertices.Count);
            triangles.Add(1 + vertices.Count);


            //添加4个点
            //            vertices.Add(new Vector3(-0.5f + x, 1 + y, -0.5f + z));
            //            vertices.Add(new Vector3(-0.5f + x, 1 + y, 1 + z));
            //            vertices.Add(new Vector3(-1 + x, 1 + y, 1 + z));
            //            vertices.Add(new Vector3(-1 + x, 1 + y, -0.5f + z));

            vertices.Add(new Vector3(-0.5f + p.x, 0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, 0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, 0.5f + p.y, 0.5f + p.z));
            vertices.Add(new Vector3(-0.5f + p.x, 0.5f + p.y, 0.5f + p.z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.uvTop[3].x + shrinkSize, block.uvTop[3].y + shrinkSize));
            uv.Add(new Vector2(block.uvTop[1].x - shrinkSize, block.uvTop[1].y + shrinkSize));
            uv.Add(new Vector2(block.uvTop[2].x - shrinkSize, block.uvTop[2].y - shrinkSize));
            uv.Add(new Vector2(block.uvTop[0].x + shrinkSize, block.uvTop[0].y - shrinkSize));
        }

        //下面
        void AddBottomFace(int x, int y, int z, Block block)
        {
            var p = GetCubePivot(x, y, z);
            //            //第一个三角面
            //            triangles.Add(1 + vertices.Count);
            //            triangles.Add(0 + vertices.Count);
            //            triangles.Add(3 + vertices.Count);
            //
            //            //第二个三角面
            //            triangles.Add(3 + vertices.Count);
            //            triangles.Add(2 + vertices.Count);
            //            triangles.Add(1 + vertices.Count);
            //第一个三角面
            triangles.Add(3 + vertices.Count);
            triangles.Add(0 + vertices.Count);
            triangles.Add(1 + vertices.Count);

            //第二个三角面
            triangles.Add(1 + vertices.Count);
            triangles.Add(2 + vertices.Count);
            triangles.Add(3 + vertices.Count);

            //添加4个点
            //            vertices.Add(new Vector3(-1 + x, 0 + y, 0 + z));
            //            vertices.Add(new Vector3(-1 + x, 0 + y, 1 + z));
            //            vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
            //            vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));

            vertices.Add(new Vector3(-0.5f + p.x, -0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, -0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, -0.5f + p.y, 0.5f + p.z));
            vertices.Add(new Vector3(-0.5f + p.x, -0.5f + p.y, 0.5f + p.z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.uvBottom[3].x + shrinkSize, block.uvBottom[3].y + shrinkSize));
            uv.Add(new Vector2(block.uvBottom[1].x - shrinkSize, block.uvBottom[1].y + shrinkSize));
            uv.Add(new Vector2(block.uvBottom[2].x - shrinkSize, block.uvBottom[2].y - shrinkSize));
            uv.Add(new Vector2(block.uvBottom[0].x + shrinkSize, block.uvBottom[0].y - shrinkSize));
        }

        #endregion
    }
}