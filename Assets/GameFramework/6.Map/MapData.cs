using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace GameFramework
{
    [System.Serializable]
    public class ChunkData
    {
        public List<SectionData> sectionData;
        public ChunkData(int sectionCount)
        {         
            this.InitData(sectionCount);
        }

        public void InitData(int sectionCount)
        {
            sectionData = new List<SectionData>();
            for (int i = 0; i < sectionCount; i++)
            {
                var sd = new SectionData();
                this.sectionData.Add(sd);
            }
        }

        public byte this[int x, int y, int z]
        {
            get
            {
                int sectionIndex =Mathf.FloorToInt( y / 16);
                int sectionY = y % 16;

                return sectionData[sectionIndex][x,sectionY,z];
            }
            set
            {
                int sectionIndex = Mathf.FloorToInt(y / 16);
                int sectionY = y % 16;
                sectionData[sectionIndex][x, sectionY, z]=value;
            }
        }
        
    }
    [System.Serializable]
    public class SectionData
    {
        public byte[,,] blocks = new byte[16, 16, 16];

        public SectionData()
        {
            for (int k = 0; k < 16; k++)
            {
                for (int j = 0; j < 16; j++)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        //block数值为0代表该Block为空，没有数据
                        blocks[i, j, k] = 0;
                    }
                }
            }
        }
        public byte this[int x, int y, int z]
        {
            get { return blocks[x,y,z]; }
            set { blocks[x, y, z] = value; }
        }
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
        /// 地图的高度，对应y轴section的个数
        /// </summary>
        public int height;
        /// <summary>
        /// 地图的深度，对应z轴chunk的个数
        /// </summary>
        public int depth;
        /// <summary>
        /// chunk数据
        /// </summary>
        public List<List<ChunkData>> chunkDatas = new List<List<ChunkData>>();
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
            width = 16;
            height = 16;
            depth = 16;
            this.InitChunksData();
        }

        /// <summary>
        /// 对地图chunk的数据进行初始化
        /// </summary>
        public void InitChunksData()
        {
            for (int i = 0; i < width; i++)
            {
                var col = new List<ChunkData>();
                for (int j = 0; j < depth; j++)
                {
                    var cd = new ChunkData(height);
                    col.Add(cd);
                }
                chunkDatas.Add(col);
            }
        }
    }
}