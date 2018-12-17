using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MyTools 
{
    public static void DeleteAllChildren(Transform transform)
    {
        for (int i = transform.childCount; i > 0; --i)
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
    }
}
