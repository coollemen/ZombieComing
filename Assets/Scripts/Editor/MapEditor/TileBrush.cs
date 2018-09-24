using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 瓷砖笔刷
/// </summary>
[System.Serializable]
public class TileBrush
{
    public string name;
    public string prefabPath;
    public Texture2D texture;

    public TileBrush(string setName)
    {
        name = setName;
    }
}
