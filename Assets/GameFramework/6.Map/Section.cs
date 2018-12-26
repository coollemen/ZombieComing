using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace GameFramework
{
    public class Section
    {
        /// <summary>
        /// x轴宽度
        /// </summary>
        public static int width = 16;

        /// <summary>
        /// y轴高度
        /// </summary>
        public static int height = 16;

        /// <summary>
        /// z轴深度
        /// </summary>
        public static int depth = 16;

        /// <summary>
        /// 在簇中的序列，范围：0-15
        /// </summary>
        public int id;

        /// <summary>
        /// 地图块的三维数组，每个块数据类型为byte，
        /// 存储block的id,如果为0，代表空
        /// </summary>
        public byte[,,] blocks;

        /// <summary>
        /// 当前选择的block；
        /// </summary>
        public Vector3Int activeBlock;

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
        ///当前Chunk是否正在生成中
        /// </summary>
        private bool isWorking = false;

        /// <summary>
        /// 所属于的图块地形
        /// </summary>
        private BlockTerrain terrain;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="setId">标识</param>
        /// <param name="setTerrain">地形</param>
        public Section(int setId, BlockTerrain setTerrain)
        {
            this.id = setId;
            this.terrain = setTerrain;
        }

        /// <summary>
        /// 初始化地图块,每个block的值为0
        /// </summary>
        public void InitBlocks()
        {
            blocks = new byte[width, height, depth];
            for (int x = 0; x < Section.width; x++)
            {
                for (int y = 0; y < Section.height; y++)
                {
                    for (int z = 0; z < Section.depth; z++)
                    {
                        blocks[x, y, z] = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 创建地图块
        /// </summary>
        /// <param name="data">断面数据</param>
        public void SetBlocks(SectionData data)
        {
            this.blocks = data.blocks;
            isDirty = true;
        }
        /// <summary>
        /// 异步创建Mesh
        /// </summary>
        /// <returns></returns>
        public IEnumerator CreateSectionMeshAsyn()
        {
            while (isWorking)
            {
                yield return null;
            }

            isWorking = true;
            //创建顶点、索引、uv
            //初始化列表
            mesh = new Mesh();
            mesh.name = "Section" + id.ToString();
            vertices.Clear();
            triangles.Clear();
            uv.Clear();
            //把所有面的点和面的索引添加进去
            for (int x = 0; x < Section.width; x++)
            {
                for (int y = 0; y < Section.height; y++)
                {
                    for (int z = 0; z < Section.depth; z++)
                    {
                        //获取当前坐标的Block对象
                        Block block = terrain.GetBlock((byte) (this.blocks[x, y, z] - 1));
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
            if (x >= width || y >= height || z >= width || x < 0 || y < 0 || z < 0)
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
        /// 设置某个地图块为选中地图块，并显示选中的样式，例如出现边框，高亮
        /// </summary>
        /// <param name="blockIdx"></param>
        public void SetBlockActive(Vector3 blockIdx)
        {
            this.activeBlock = new Vector3Int((int) blockIdx.x, (int) blockIdx.y, (int) blockIdx.z);
            this.isDirty = true;
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
            return new Vector3(x - width / 2 + 0.5f, y - height / 2 + 0.5f, z - depth / 2 + 0.5f);
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