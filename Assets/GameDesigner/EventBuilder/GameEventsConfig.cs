using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameDesigner
{
    /// <summary>
    /// 游戏事件配置数据
    /// </summary>
    [System.Serializable]
    public class GameEventsConfig : ScriptableObject
    {
        /// <summary>
        /// 事件代码保存位置
        /// </summary>
        public string path = "Assets/Resources/";

        /// <summary>
        /// 当前选择的组合索引
        /// </summary>
        public int selectedGroupIndex = 0;

        /// <summary>
        ///是否添加NameSpace
        /// </summary>
        public bool hasNameSpace = false;

        /// <summary>
        /// 命名空间
        /// </summary>
        public string nameSpace = "GameFramework";

        /// <summary>
        /// 事件组
        /// </summary>
        public List<GameEventGroup> groups = new List<GameEventGroup>();
    }
}