using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
namespace GameFramework
{
    public class Section 
    {
        public static int width = 16;
        public static int height = 16;
        public static int depth = 16;
        public int id;
        public byte[,,] blocks;
        public Vector3Int activeBlock;
        public Mesh mesh;
        //顶点列表
        private List<Vector3> vertices = new List<Vector3>();
        //三角形面顶点索引列表
        private List<int> triangles = new List<int>();
        //所有的uv信息
        private List<Vector2> uv = new List<Vector2>();
        //uv贴图每行每列的宽度(0~1)，这里我的贴图是32×32的，所以是1/32
        public static float textureOffset = 1 / 32f;
        //让UV稍微缩小一点，避免出现它旁边的贴图
        public static float shrinkSize = 0.001f;
        //是否需要更新Mesh
        public bool isDirty = false;
        //当前Chunk是否正在生成中
        private bool isWorking = false;

        public Section()
        {
            
        }
        public Section(int setId)
        {
            this.id = setId;
        }
        /// <summary>
        /// 创建地图块
        /// </summary>
        public void CreateBlocks()
        {
            blocks = new byte[width, height, depth];
            for (int x = 0; x < Section.width; x++)
            {
                for (int y = 0; y < Section.height; y++)
                {
                    for (int z = 0; z < Section.depth; z++)
                    {
                        if (y == Section.height - 1 && id == 1)
                        {
                            if (Random.Range(1, 5) == 1)
                            {
                                blocks[x, y, z] = 2;
                            }
                        }
                        else
                        {
                            blocks[x, y, z] = 1;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 创建Mesh
        /// </summary>
        /// <returns></returns>
        public  IEnumerator CreateMesh()
        {
            while (isWorking)
            {
                yield return null;
            }
            isWorking = true;       
            //创建顶点、索引、uv
           //初始化列表
            mesh = new Mesh();
            mesh.name = "Section"+id.ToString();
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
                        Block block = BlockList.GetBlock(this.blocks[x, y, z]);
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
        public void CreateBlock(int x, int y, int z,byte blockID)
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
            uv.Add(new Vector2(block.textureBackX * textureOffset, block.textureBackY * textureOffset) +
                   new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureBackX * textureOffset + textureOffset, block.textureBackY * textureOffset) +
                   new Vector2(-shrinkSize, shrinkSize));
            uv.Add(
                new Vector2(block.textureBackX * textureOffset + textureOffset,
                    block.textureBackY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureBackX * textureOffset, block.textureBackY * textureOffset + textureOffset) +
                   new Vector2(shrinkSize, -shrinkSize));
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
            uv.Add(new Vector2(block.textureFrontX * textureOffset, block.textureFrontY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureFrontX * textureOffset + textureOffset, block.textureFrontY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureFrontX * textureOffset + textureOffset, block.textureFrontY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureFrontX * textureOffset, block.textureFrontY * textureOffset + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
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
            uv.Add(new Vector2(block.textureRightX * textureOffset, block.textureRightY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureRightX * textureOffset + textureOffset, block.textureRightY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureRightX * textureOffset + textureOffset, block.textureRightY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureRightX * textureOffset, block.textureRightY * textureOffset + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
        }

        private Vector3 GetCubePivot(int x, int y, int z)
        {
            return new Vector3(x - width/2 + 0.5f, y -height/2 + 0.5f, z - depth/2 + 0.5f);
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
            vertices.Add(new Vector3(0.5f + p.x, 0.5f  + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(-0.5f + p.x, 0.5f + p.y, -0.5f + p.z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureLeftX * textureOffset, block.textureLeftY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureLeftX * textureOffset + textureOffset, block.textureLeftY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureLeftX * textureOffset + textureOffset, block.textureLeftY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureLeftX * textureOffset, block.textureLeftY * textureOffset + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
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
            uv.Add(new Vector2(block.textureTopX * textureOffset, block.textureTopY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureTopX * textureOffset + textureOffset, block.textureTopY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureTopX * textureOffset + textureOffset, block.textureTopY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureTopX * textureOffset, block.textureTopY * textureOffset + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
        }

        //下面
        void AddBottomFace(int x, int y, int z, Block block)
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
//            vertices.Add(new Vector3(-1 + x, 0 + y, 0 + z));
//            vertices.Add(new Vector3(-1 + x, 0 + y, 1 + z));
//            vertices.Add(new Vector3(0 + x, 0 + y, 1 + z));
//            vertices.Add(new Vector3(0 + x, 0 + y, 0 + z));

            vertices.Add(new Vector3(-0.5f + p.x, -0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, -0.5f + p.y, -0.5f + p.z));
            vertices.Add(new Vector3(0.5f + p.x, -0.5f + p.y, 0.5f + p.z));
            vertices.Add(new Vector3(-0.5f + p.x, -0.5f + p.y, 0.5f + p.z));

            //添加UV坐标点，跟上面4个点循环的顺序一致
            uv.Add(new Vector2(block.textureBottomX * textureOffset, block.textureBottomY * textureOffset) + new Vector2(shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureBottomX * textureOffset + textureOffset, block.textureBottomY * textureOffset) + new Vector2(-shrinkSize, shrinkSize));
            uv.Add(new Vector2(block.textureBottomX * textureOffset + textureOffset, block.textureBottomY * textureOffset + textureOffset) + new Vector2(-shrinkSize, -shrinkSize));
            uv.Add(new Vector2(block.textureBottomX * textureOffset, block.textureBottomY * textureOffset + textureOffset) + new Vector2(shrinkSize, -shrinkSize));
        }
        #endregion
    }

}