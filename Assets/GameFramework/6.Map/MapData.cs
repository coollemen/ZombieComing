using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [System.Serializable]
    public class ChunkData
    {
        public int id;
        public List<SectionData> sectionData;
        public ChunkData(int setID)
        {
            this.id = setID;
            sectionData = new List<SectionData>();
        }

    }
    [System.Serializable]
    public class SectionData
    {
        public byte[,,] blocks = new byte[16, 16, 16];
    }

    /// <summary>
    /// 地图数据
    /// </summary>
    public class MapData : ScriptableObject
    {
        public string name;
        public int width;
        public int depth;
        public List<ChunkData> chunkDatas = new List<ChunkData>();

        public MapData(string setName)
        {
            this.name = setName;
        }
    }
}