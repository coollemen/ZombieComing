using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 图块物体的数据
    /// </summary>
    [CreateAssetMenu(fileName = "CustomBlockObject.asset", menuName = "GameFramework/Block Object Data Asset")]
    public class BlockObjectData : ScriptableObject
    {

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
        /// 图块定义数组
        /// </summary>
        public BlockDefinitionCollection blockDefs;

        /// <summary>
        /// 获取图块数据的宽度(x维度)
        /// </summary>
        public int Width
        {
            get
            {
                if (blocks == null)
                {
                    return -1;
                }
                else
                {
                    return this.blocks.GetLength(0);
                }
            }
        }

        /// <summary>
        /// 获取图块数据的高度(y维度)
        /// </summary>
        public int Height
        {
            get
            {
                if (blocks == null)
                {
                    return -1;
                }
                else
                {
                    return this.blocks.GetLength(1);
                }
            }
        }

        /// <summary>
        /// 获取图块数据的深度(z维度)
        /// </summary>
        public int Depth
        {
            get
            {
                if (blocks == null)
                {
                    return -1;
                }
                else
                {
                    return this.blocks.GetLength(2);
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Awake()
        {
            this.name = "block object";
            this.blocks = new byte[defaultSize, defaultSize, defaultSize];
            this.blockDefs = new  BlockDefinitionCollection();
            this.description = "";

        }

        /// <summary>
        /// 获取指定位置的图块
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        /// <returns>数据</returns>
        public byte GetBlock(int x, int y, int z)
        {
            return this.blocks[x, y, z];
        }

        /// <summary>
        /// 设置指定位置的图块数据
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        /// <param name="b">数据</param>
        public void SetBlock(int x, int y, int z, byte b)
        {
            this.blocks[x, y, z] = b;
        }

        /// <summary>
        /// 重新设置物体图块数组大小,将丢失已有数据
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        public void ResizeBlocksArray(int width, int height, int depth)
        {
            this.blocks = new byte[width, height, depth];
        }
    }
}