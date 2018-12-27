using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 图块物体的数据
    /// </summary>
    public class BlockObjectData : ScriptableObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 地图块数据
        /// </summary>
        public byte[,,] blocks;
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
        /// <summary>
        /// 默认数组大小
        /// </summary>
        private int defaultSize = 256;
        /// <summary>
        /// 初始化
        /// </summary>
        public void Awake()
        {
            this.name = "block object";
            this.blocks = new byte[defaultSize, defaultSize, defaultSize];
            this.description = "";
        }
        /// <summary>
        /// 重新设置物体图块数组大小
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        public void ResizeBlocksArray(int width, int height, int depth)
        {

        }

    }
}