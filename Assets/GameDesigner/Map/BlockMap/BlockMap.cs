using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameDesigner
{
    public class BlockMap : MonoBehaviour
    {
        public int width = 2;
        public int height = 2;
        public List<Chunk> chunks = new List<Chunk>();
        public Chunk activeChunk;
        public Color color1 = new Color(0.5f, 0.5f, 0.5f, 1f);
        public Color color2 = new Color(0.6f, 0.6f, 0.6f, 1f);
        public Color activeColor = Color.green;
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
            if (chunks.Count == 0) return;
            for (int i = 0; i < chunks.Count; i++)
            {
                if (Mathf.FloorToInt(i / width) % 2 == 0)
                {
                    Color oldColor = Gizmos.color;
                    if (i % 2 == 0)
                    {
                        Gizmos.color = color1;
                    }
                    else
                    {
                        Gizmos.color = color2;
                    }
                    var chunkPosition = chunks[i].transform.position;
                    var pos = new Vector3(chunkPosition.x + 8, chunkPosition.y, chunkPosition.z + 8);
                    Gizmos.DrawCube(pos, new Vector3(16, 0, 16));
                    Gizmos.color = oldColor;
                }
                else
                {
                    Color oldColor = Gizmos.color;
                    if (i % 2 == 0)
                    {
                        Gizmos.color = color2;
                    }
                    else
                    {
                        Gizmos.color = color1;
                    }
                    var chunkPosition = chunks[i].transform.position;
                    var pos = new Vector3(chunkPosition.x + 8, chunkPosition.y, chunkPosition.z + 8);
                    Gizmos.DrawCube(pos, new Vector3(16, 0, 16));
                    Gizmos.color = oldColor;
                }
            }
        }

        public void CreateChunks()
        {
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    GameObject go = new GameObject("Chunk_" + x.ToString() + "_" + z.ToString());
                    go.transform.SetParent(transform);
                    Vector3 pos = new Vector3(x * 16, 0, z * 16);
                    go.transform.localPosition = pos;
                    var chunk = go.AddComponent<Chunk>();
                    chunk.CreateBlocks();
                    this.chunks.Add(chunk);
                }
            }
        }

        public void ClearChunks()
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                DestroyImmediate(chunks[i].gameObject);
            }
            chunks.Clear();
        }


    }
}