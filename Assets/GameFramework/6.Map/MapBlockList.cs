using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace GameFramework
{
    /// <summary>
    /// 存储所有的Block对象的信息
    /// </summary>
    public class BlockList : MonoBehaviour
    {
        public static Dictionary<byte, MapBlock> blocks = new Dictionary<byte, MapBlock>();

        void Awake()
        {
            MapBlock dirt = new MapBlock(1, "Dirt", 2, 31);
            blocks.Add(dirt.id, dirt);

            MapBlock grass = new MapBlock(2, "Grass", 3, 31, 0, 31, 2, 31);
            blocks.Add(grass.id, grass);
        }

        public static MapBlock GetBlock(byte id)
        {
            return blocks.ContainsKey(id) ? blocks[id] : null;
        }
    }
}
