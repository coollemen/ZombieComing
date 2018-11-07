using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
namespace GameFramework {
    /// <summary>
    /// 保存地图块的设置
    /// </summary>
    public class BlockConfig:ScriptableObject
    {
        public string version;

        public List<BlockDefinition> blockDefinitions = new List<BlockDefinition>();
    }
}
