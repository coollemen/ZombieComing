using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 地图簇创建模式
    /// </summary>
    public enum ChunkCreateMode
    {
        /// <summary>
        /// 按照算法随机生成
        /// </summary>
        Random,
        /// <summary>
        /// 定义需要的Block的类型，数量，然后随即生成
        /// </summary>
        Pool,
        /// <summary>
        /// 手动设置
        /// </summary>
        Manual
    }
}