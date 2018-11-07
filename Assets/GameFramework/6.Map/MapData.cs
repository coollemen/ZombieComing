using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{

    [System.Serializable]
    public class ChunkData
    {
        public Vector3Int position;
        public byte[,,] blocks = new byte[16, 16, 16];
    }
    /// <summary>
    /// 地图数据
    /// </summary>
    public class MapData : ScriptableObject
    {
        public string name;
        public List<MapBlock> blockCaches = new List<MapBlock>();
    }
}