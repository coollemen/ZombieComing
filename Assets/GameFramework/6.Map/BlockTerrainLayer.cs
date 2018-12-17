using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    [System.Serializable]
    public class BlockTerrainLayerItem
    {
        /// <summary>
        /// 地图块索引
        /// </summary>
        public int blockId;

        /// <summary>
        /// 生成权重
        /// </summary>
        public float weight;
    }

    /// <summary>
    /// 地图地层,表示地图256层中指定层中block的类型分布
    /// </summary>
    [System.Serializable]
    public class BlockTerrainLayer
    {
        public string Name
        {
            get { return string.Format("{0}-{1}", start, end); }
        }
        /// <summary>
        /// 地图Y轴起始索引
        /// </summary>
        public int start;
        /// <summary>
        /// 地图Y轴结束索引
        /// </summary>
        public int end;
        /// <summary>
        /// 不同类型Block的比例定义
        /// </summary>
        public List<BlockTerrainLayerItem> items = new List<BlockTerrainLayerItem>();

        public BlockTerrainLayer()
        {
            
        }

        public BlockTerrainLayer(int setStart, int setEnd)
        {
            this.start = setStart;
            this.end = setEnd;
        }
    }
}