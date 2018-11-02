using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 四边形坐标轴
/// </summary>
[System.Serializable]
public class QuadCoordinates
{
    [SerializeField]
    private int x;
    [SerializeField]
    private int z;

    public int X
    {
        get { return this.x; }
    }

    public int Z
    {
        get { return this.z; }
    }
    public QuadCoordinates(int setX, int setZ)
    {
        this.x = setX;
        this.z = setZ;
    }
    public override string ToString()
    {
        return "(" + this.x.ToString() + "," + this.z.ToString() + ")";
    }
}
