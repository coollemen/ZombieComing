using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class Map : MonoSingleton<Map>
    {
        public int width = 64;
        public int depth = 64;
        public int viewWidth = 9;
        public int viewDepth = 9;
        public Chunk[,] chunks;
        public Chunk[,] viewChunks;
        public GameObject chunkPrefab;
        public MapData data;

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

        public void LoadMap()
        {

        }

        public void SaveMap()
        {

        }
        /// <summary>
        /// 创建随机地图
        /// </summary>
        public void CreateRandomMap()
        {
            for (int i = 0; i < viewWidth; i++)
            {
                for (int j = 0; j < viewDepth; j++)
                {
                    var go = Instantiate(chunkPrefab);
                    go.transform.SetParent(this.transform);
                    go.transform.position =
                        new Vector3(transform.position.x+(i-1)*16, transform.position.y, transform.position.z+(j-1)*16);
                    go.name = string.Format("Chunk_{0}_{1}", i, j);
                    viewChunks[i, j] = go.GetComponent<Chunk>();                  
                }
            }
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