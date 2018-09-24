using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameDesignMenuItems  {

    [MenuItem("GameDesign/Add Map To Scene")]
    static void CreateMapObject()
    {
        GameObject go = GameObject.FindGameObjectWithTag("GameMap");
        if (go != null)
        {
            if (go.GetComponent<Map>() == null)
            {
                go.AddComponent<Map>();
            }
            else
            {
                Debug.Log("地图组件已经存在，无法重复添加！");
            }
        }
        else
        {
            go = new GameObject("Map");
            go.AddComponent<Map>();
            go.tag = "GameMap";
        }

    }
}
