using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace GameFramework
{
    /// <summary>
    /// 地图块的定义，用于创建地图块
    /// </summary>
    [System.Serializable]
    public class BlockDefinition
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id;

        /// <summary>
        /// 名字
        /// </summary>
        public string name;

        /// <summary>
        /// 图标名字
        /// </summary>
        public string iconName;

        public BlockDefinition()
        {
            
        }
        public BlockDefinition(byte setID, string setName)
        {
            this.id = setID;
            this.name = setName;
        }
    }
}
