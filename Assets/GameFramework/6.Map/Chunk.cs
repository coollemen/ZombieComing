using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
namespace GameFramework
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {

        private Mesh mesh;
        public Section[] sections = new Section[16];


        //当前Chunk是否正在生成中
        private bool isWorking = false;

        void Start()
        {
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
               yield return StartCoroutine(sections[i].CreateMesh());
            }
            CombineInstance[] instances = new CombineInstance[sections.Length];
            for (int i = 0; i < sections.Length; i++)
            {
                instances[i] = new CombineInstance();
                instances[i].mesh = sections[i].mesh;
                Vector3 pos = transform.position;
                pos.y += i * 16;
                Quaternion rot = transform.rotation;
                Vector3 scale = transform.localScale;
                instances[i].transform = Matrix4x4.TRS(pos, rot, scale);
            }
            //组合mesh
            mesh.CombineMeshes(instances);
            this.GetComponent<MeshFilter>().mesh =mesh;
            this.GetComponent<MeshCollider>().sharedMesh = mesh;

            isWorking = false;
        }
        void OnGUI()
        {
            GUILayout.Label("MeshVertexCount=" + mesh.vertexCount);
            GUILayout.Label("MeshTriCount=" + mesh.triangles.LongLength);
        }
     
    }
}