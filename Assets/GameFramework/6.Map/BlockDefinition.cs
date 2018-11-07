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
        public string name;
        public string textureName;
        public List<Vector2> uv;
    }
}
