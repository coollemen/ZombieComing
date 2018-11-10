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

        public Mesh mesh;
        public Section[] sections;
        public int sectionCount = 16;

        //当前Chunk是否正在生成中
        private bool isWorking = false;
        public bool isDirty = false;
        void Start()
        {
            sections = new Section[sectionCount];
            StartCoroutine(CreateChunkMesh());
        }

        IEnumerator CreateChunkMesh()
        {
            while (isWorking)
            {
                yield return null;
            }
            isWorking = true;
            mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.name = "chunk";
            for (int i = 0; i < sections.Length; i++)
            {
                sections[i] = new Section(i);
                sections[i].CreateBlocks();
               yield return StartCoroutine(sections[i].CreateMesh());
            }
            CombineInstance[] instances = new CombineInstance[sections.Length];
            for (int i = 0; i < sections.Length; i++)
            {
                instances[i] = new CombineInstance();
                instances[i].mesh = sections[i].mesh;
                Vector3 pos = Vector3.zero;
                pos.y += i * 16-8*sectionCount+8;
                Quaternion rot = Quaternion.identity;
                Vector3 scale =new Vector3(1,1,1);
                instances[i].transform = Matrix4x4.TRS(pos, rot, scale);
            }
            //组合mesh
            mesh.CombineMeshes(instances);
            mesh.RecalculateNormals();
            mesh.bounds = new Bounds(Vector3.zero, new Vector3(16, 256, 16));
            this.GetComponent<MeshFilter>().mesh =mesh;
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

            mesh.Clear();
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

            isWorking = false;
            isDirty = false;
            Debug.Log("Update Mesh End!");
        }
        public void FocusBlock(Vector3 worldPoint)
        {
            //转换为chunk中的block索引
            var blockPoint = GetBlockFromWorldPoint(worldPoint);
            //确定在哪个Section中
            var idx =Mathf.FloorToInt(blockPoint.y / 16) ;
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
            sections[idx].DeleteBlock((int)localBlockPoint.x, (int)localBlockPoint.y,(int) localBlockPoint.z);
            isDirty = true;
        }
        public Vector3 GetBlockFromWorldPoint(Vector3 worldPoint)
        {
            Vector3 localPoint= this.transform.InverseTransformPoint(worldPoint);
            float x = Mathf.Ceil(localPoint.x)-1+8;
            float y = Mathf.Ceil(localPoint.y)-1+sectionCount/2*16;
            float z = Mathf.Ceil(localPoint.z)-1+8;

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
            if (xmax == xmin && ymax!=ymin && zmax!=zmin)
            {
                if (sections[idx].IsBlockTransparent((int)localBlockPoint.x, (int)localBlockPoint.y,
    (int)localBlockPoint.z))
                {
                    blockPoint.x += 1;
                }
            }
           else  if (ymax == ymin && xmax != xmin && zmax != zmin)
            {
                if (sections[idx].IsBlockTransparent((int)localBlockPoint.x, (int)localBlockPoint.y,
(int)localBlockPoint.z))
                {
                    blockPoint.y += 1;
                }
            }
           else  if (zmax == zmin && xmax != xmin && ymax != ymin)
            {
                if (sections[idx].IsBlockTransparent((int)localBlockPoint.x, (int)localBlockPoint.y,
(int)localBlockPoint.z))
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
            Debug.Log(worldPoint+"=>"+blockPoint);
            return blockPoint;
        }
        
    }
}