using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockInfo
{
    /// <summary>
    /// 在chunk中的索引
    /// </summary>
    public int id;
    /// <summary>
    /// x
    /// </summary>
    public int x;
    /// <summary>
    /// y
    /// </summary>
    public int y;
    /// <summary>
    /// z
    /// </summary>
    public int z;
    /// <summary>
    /// 是否为空
    /// </summary>
    public bool empty;

    /// <summary>
    /// 块游戏物体的引用
    /// </summary>
    public string prefabPath;
    /// <summary>
    /// 绘制位置
    /// </summary>
    public Vector3 position;
    /// <summary>
    /// 偏移位置
    /// </summary>
    public Vector3 offset;
    /// <summary>
    /// 旋转角度（沿Y轴）
    /// </summary>
    public float rotation;
    /// <summary>
    /// 缩放，默认为1
    /// </summary>
    public float scale;

    public BlockInfo(int setID)
    {
        id = setID;
        empty = true;
        offset = Vector3.zero;
        rotation = 0;
        scale = 1;
    }
}
