using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameDesignMenuItems  {

    [MenuItem("Game Designer/Add BlockTerrain To Scene")]
    static void CreateMapObject()
    {
        GameObject go = GameObject.FindGameObjectWithTag("GameMap");
        if (go != null)
        {
            if (go.GetComponent<Map>() == null)
            {
               var map= go.AddComponent<Map>();
            }
            else
            {
                Debug.Log("地图组件已经存在，无法重复添加！");
            }
        }
        else
        {
            go = new GameObject("BlockTerrain");
            var map= go.AddComponent<Map>();
            go.tag = "GameMap";
        }

    }
    /// <summary>
    /// 创建地形底层，用于鼠标点击定位
    /// </summary>
    /// <returns></returns>
    private static  GameObject CreateTerrainPanel(float length,float width)
    {
        GameObject terrain = GameObject.CreatePrimitive(PrimitiveType.Plane);
        terrain.transform.localScale = new Vector3(length / 10, 1, width / 10);
        return terrain;
    }
}
