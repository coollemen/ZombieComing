using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
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
    [CreateAssetMenu(fileName = "CustomMapData.asset", menuName = "GameFramework/Map Data Asset")]
    /// <summary>
    /// 地图数据
    /// </summary>
    public class MapData : ScriptableObject
    {
        /// <summary>
        /// 地图的名称
        /// </summary>
        public string name;
        /// <summary>
        /// 主版本数字
        /// </summary>
        private int versionMainNum;
        /// <summary>
        /// 功能版本数字
        /// </summary>
        private int versionFunctionNum;
        /// <summary>
        /// 修复bug版本数字
        /// </summary>
        private int versionFixNum;
        /// <summary>
        /// 版本
        /// </summary>
        [ShowInInspector,PropertyOrder(1)]
        public string version
        {
            get { return string.Format("{0}.{1}.{2}", versionMainNum, versionFunctionNum, versionFixNum); }
        }
        /// <summary>
        /// 地图的宽度，对应x轴chunk的个数
        /// </summary>
        public int width;
        /// <summary>
        /// 地图的高度，对应z轴chunk的个数
        /// </summary>
        public int height;
        /// <summary>
        /// chunk数据
        /// </summary>
        public List<ChunkData> chunkDatas = new List<ChunkData>();
        /// <summary>
        /// block数据
        /// </summary>
        public List<BlockDefinition> blockDefinitions = new List<BlockDefinition>();

        public List<MapLayer> layers = new List<MapLayer>();
        private void Awake()
        {

            this.SetVersion();
            this.CreateDefaultData();
        }

        public virtual void SetVersion()
        {
            versionMainNum = 1;
            versionFunctionNum = 0;
            versionFixNum = 0;
        }
        public void CreateDefaultData()
        {
            name = "Map Data";
            width = 64;
            height = 64;
        }

    }
}