using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace GameFramework
{
    /// <summary>
    /// 地图簇，由16个Section组成，一个簇就是一个Mesh
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        /// <summary>
        /// 网格
        /// </summary>
        public Mesh mesh;
        /// <summary>
        /// 断面数组
        /// </summary>
        public Section[] sections;
        /// <summary>
        /// 断面个数
        /// </summary>
        public int sectionCount = 16;
        /// <summary>
        /// 地形父组件
        /// </summary>
        public BlockTerrain terrain;
        /// <summary>
        /// 行数
        /// </summary>
        public int row;
        /// <summary>
        /// 列数
        /// </summary>
        public int col;
        /// <summary>
        /// 簇数据
        /// </summary>
        public ChunkData data;
        /// <summary>
        ///当前Chunk是否正在生成中
        /// </summary>   
        private bool isWorking = false;

        /// <summary>
        ///是否需要更新
        /// </summary>
        public bool isDirty = false;

        void Start()
        {
            this.Init();
        }

        public void Init()
        {
            StartCoroutine(CreateChunkMesh());
        }

        /// <summary>
        /// 异步创建chunk的网格
        /// </summary>
        /// <returns></returns>
        public IEnumerator CreateChunkMesh()
        {
            while (isWorking)
            {
                yield return null;
            }

            isWorking = true;
            //获取图块地形组件
            if (terrain == null)
            {
                this.terrain = GetComponentInParent<BlockTerrain>();
            }
            //初始化截面数组
            sections = new Section[sectionCount];
            //初始化网格
            mesh = new Mesh();
            //设置索引格式，支持32位三角形索引
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            //设置网格名字
            mesh.name = "chunk";
            for (int i = 0; i < sections.Length; i++)
            {
                sections[i] = new Section(i, terrain);
                sections[i].CreateBlocks(this.data.sectionData[i]);
                yield return StartCoroutine(sections[i].CreateMesh());
            }

            CombineInstance[] instances = new CombineInstance[sections.Length];
            for (int i = 0; i < sections.Length; i++)
            {
                instances[i] = new CombineInstance();
                instances[i].mesh = sections[i].mesh;
                Vector3 pos = Vector3.zero;
                pos.y += i * 16 - 8 * sectionCount + 8;
                Quaternion rot = Quaternion.identity;
                Vector3 scale = new Vector3(1, 1, 1);
                instances[i].transform = Matrix4x4.TRS(pos, rot, scale);
            }

            //组合mesh
            mesh.CombineMeshes(instances);
            mesh.RecalculateNormals();
            mesh.bounds = new Bounds(Vector3.zero, new Vector3(16, 256, 16));
            this.GetComponent<MeshFilter>().mesh = mesh;
            this.GetComponent<MeshCollider>().sharedMesh = mesh;

            isWorking = false;
        }

        void OnGUI()
        {
//            GUILayout.Label("Center=" + mesh.bounds.center);
//            GUILayout.Label("Extends=" + mesh.bounds.extents);
        }

        private void Update()
        {
            if (isDirty)
            {
                StartCoroutine(UpdateMesh());
            }
        }

        public IEnumerator UpdateMesh()
        {
            Debug.Log("Update Mesh Start!");
            while (isWorking)
            {
                yield return null;
            }

            isWorking = true;

            mesh = new Mesh();
            for (int i = 0; i < sections.Length; i++)
            {
                if (sections[i].isDirty)
                {
                    yield return StartCoroutine(sections[i].CreateMesh());
                }
            }

            CombineInstance[] instances = new CombineInstance[sections.Length];
            for (int i = 0; i < sections.Length; i++)
            {
                instances[i] = new CombineInstance();
                instances[i].mesh = sections[i].mesh;
                Vector3 pos = Vector3.zero;
                pos.y += i * 16 - 8 * sectionCount + 8;
                Quaternion rot = Quaternion.identity;
                Vector3 scale = new Vector3(1, 1, 1);
                instances[i].transform = Matrix4x4.TRS(pos, rot, scale);
            }

            //组合mesh
            mesh.CombineMeshes(instances);
            mesh.RecalculateNormals();
            mesh.bounds = new Bounds(Vector3.zero, new Vector3(16, 256, 16));
            this.GetComponent<MeshFilter>().mesh = mesh;
            this.GetComponent<MeshCollider>().sharedMesh = mesh;

            isWorking = false;
            isDirty = false;
            Debug.Log("Update Mesh End!");
        }

        public void FocusBlock(Vector3 worldPoint)
        {
            //转换为chunk中的block索引
            var blockPoint = GetBlockFromWorldPoint(worldPoint);
            //确定在哪个Section中
            var idx = Mathf.FloorToInt(blockPoint.y / 16);
            var y = blockPoint.y % 16;
            var localBlockPoint = new Vector3(blockPoint.x, y, blockPoint.z);
        }

        public void DeleteBlock(Vector3 blockPoint)
        {
//            //转换为chunk中的block索引
//            var blockPoint = GetBlockFromWorldPoint(worldPoint);
            //确定在哪个Section中
            var idx = Mathf.FloorToInt(blockPoint.y / 16);
            var y = blockPoint.y % 16;
            var localBlockPoint = new Vector3(blockPoint.x, y, blockPoint.z);
            sections[idx].DeleteBlock((int) localBlockPoint.x, (int) localBlockPoint.y, (int) localBlockPoint.z);
            isDirty = true;
        }

        /// <summary>
        /// 获取Chuck指定位置地图块（Block）
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="z">Z坐标</param>
        /// <returns>地图块</returns>
        public int GetBlockByChunkPoint(int x, int y, int z)
        {
            int sectionIndex = Mathf.FloorToInt(y / 16);
            int sectionY = y % 16;
            return sections[sectionIndex].blocks[x, sectionY, z];
        }

        /// <summary>
        /// 在Chuck指定位置设置地图块（Block）
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="z">Z坐标</param>
        /// <param name="block">地图块</param>
        public void SetBlockByChunkPoint(int x, int y, int z, byte block)
        {
            int sectionIndex = Mathf.FloorToInt(y / 16);
            int sectionY = y % 16;
            sections[sectionIndex].blocks[x, sectionY, z] = block;
        }

        public Vector3 GetBlockFromWorldPoint(Vector3 worldPoint)
        {
            Vector3 localPoint = this.transform.InverseTransformPoint(worldPoint);
            float x = Mathf.Ceil(localPoint.x) - 1 + 8;
            float y = Mathf.Ceil(localPoint.y) - 1 + sectionCount / 2 * 16;
            float z = Mathf.Ceil(localPoint.z) - 1 + 8;

            //判断取值区间
            int xmax = Mathf.CeilToInt(localPoint.x);
            int xmin = Mathf.FloorToInt(localPoint.x);

            int ymax = Mathf.CeilToInt(localPoint.y);
            int ymin = Mathf.FloorToInt(localPoint.y);

            int zmax = Mathf.CeilToInt(localPoint.z);
            int zmin = Mathf.FloorToInt(localPoint.z);

            Vector3 blockPoint = new Vector3(x, y, z);
            //确定在哪个Section中
            var idx = Mathf.FloorToInt(blockPoint.y / 16);
            var yy = blockPoint.y % 16;
            var localBlockPoint = new Vector3(blockPoint.x, yy, blockPoint.z);
            //如果x正好处于边界，如果当前Block为空，那么将x右移
            if (xmax == xmin && ymax != ymin && zmax != zmin)
            {
                if (sections[idx].IsBlockTransparent((int) localBlockPoint.x, (int) localBlockPoint.y,
                    (int) localBlockPoint.z))
                {
                    blockPoint.x += 1;
                }
            }
            else if (ymax == ymin && xmax != xmin && zmax != zmin)
            {
                if (sections[idx].IsBlockTransparent((int) localBlockPoint.x, (int) localBlockPoint.y,
                    (int) localBlockPoint.z))
                {
                    blockPoint.y += 1;
                }
            }
            else if (zmax == zmin && xmax != xmin && ymax != ymin)
            {
                if (sections[idx].IsBlockTransparent((int) localBlockPoint.x, (int) localBlockPoint.y,
                    (int) localBlockPoint.z))
                {
                    blockPoint.z += 1;
                }
            }
            else if (xmax == xmin && ymax == ymin && zmax != zmin)
            {
                blockPoint = new Vector3(-999, -999, -999);
            }
            else if (xmax == xmin && ymax != ymin && zmax == zmin)
            {
                blockPoint = new Vector3(-999, -999, -999);
            }
            else if (xmax != xmin && ymax == ymin && zmax == zmin)
            {
                blockPoint = new Vector3(-999, -999, -999);
            }
            else
            {
                //x,y,z都为整数，那么代表这个点太特殊，不判断位置了
                blockPoint = new Vector3(-999, -999, -999);
            }

            Debug.Log(worldPoint + "=>" + blockPoint);
            return blockPoint;
        }

        #region 地形变化函数

        /// <summary>
        /// 在地图指定位置向外扩张
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="power"></param>
        public void Expand(int x, int y, int z, int power)
        {
        }

        /// <summary>
        /// 在地图指定位置向内塌陷收缩
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="z">Z坐标</param>
        /// <param name="power">强度</param>
        public void Collapse(int x, int y, int z, int power)
        {
        }

        /// <summary>
        /// 切开指定地图块
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="z">Z坐标</param>
        /// <param name="power">强度</param>
        public void Cut(int x, int y, int z, int power)
        {
        }

        /// <summary>
        /// 升高指定地图块
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="z">Z坐标</param>
        /// <param name="power">强度</param>
        public void Raise(int x, int y, int z, int power)
        {
        }

        /// <summary>
        /// 下降指定地图块
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="z">Z坐标</param>
        /// <param name="power">强度</param>
        public void Lower(int x, int y, int z, int power)
        {
        }

        #endregion
    }
}