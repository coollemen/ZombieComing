using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics  {
    /// <summary>
    /// 外径
    /// </summary>
    public const float outerRadius = 10f;
    /// <summary>
    ///内径
    /// </summary>
    public const float innerRadius = outerRadius * 0.866025404f;

    public static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)
    };

}
