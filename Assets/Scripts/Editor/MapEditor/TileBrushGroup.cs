using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 瓷砖笔刷组
/// </summary>
[System.Serializable]
public class TileBrushGroup
{
    /// <summary>
    /// 组名
    /// </summary>
    public string name;
    /// <summary>
    /// 笔刷集合
    /// </summary>
    public List<TileBrush> brushes = new List<TileBrush>();
    /// <summary>
    /// 描述
    /// </summary>
    public string description;

    public TileBrushGroup(string setName)
    {
        this.name = setName;
    }
}
