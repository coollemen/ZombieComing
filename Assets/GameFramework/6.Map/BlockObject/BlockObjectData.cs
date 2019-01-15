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
        public ByteArray3D blocks;

        /// <summary>
        /// 描述
        /// </summary>
        public string description;

        /// <summary>
        /// 默认数组大小
        /// </summary>
        private int defaultSize =16;

        /// <summary>
        /// 图块定义数组
        /// </summary>
        public List<BlockDefinition> blockDefs=new List<BlockDefinition>();

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
                    return this.blocks.Count;
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
                    return this.blocks[0].Count; ;
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
                    return this.blocks[0][0].Count;
                }
            }
        }

        public void InitBlocksArray()
        {
            this.ResizeBlocksArray(defaultSize, defaultSize, defaultSize);
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
            return this.blocks[x][y][ z];
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
            this.blocks[x][y][z] = b;
        }

        /// <summary>
        /// 重新设置物体图块数组大小,将丢失已有数据
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        public void ResizeBlocksArray(int width, int height, int depth)
        {
            this.blocks = new ByteArray3D();
            for (int x = 0; x < width; x++)
            {
                ByteArray2D byteArray2D = new ByteArray2D();
                for (int y = 0; y < height; y++)
                {
                    ByteArray byteArray = new ByteArray();
                    for (int z = 0; z < depth; z++)
                    {
                        byteArray.Add(0);
                    }
                    byteArray2D.Add(byteArray);
                }
                this.blocks.Add(byteArray2D);
            }
        }

        public byte[,,] GetBlocks()
        {
            byte[,,] data = new byte[Width, Height, Depth];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int z = 0; z < Depth; z++)
                    {
                        data[x, y, z] = blocks[x][y][z];
                    }
                }
            }
            return data;
        }

        public void SetBlocks(byte[,,] data)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int z = 0; z < Depth; z++)
                    {
                         blocks[x][y][z]= data[x, y, z] ;
                    }
                }
            }
        }
    }
}