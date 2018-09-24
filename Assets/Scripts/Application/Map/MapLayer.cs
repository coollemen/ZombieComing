using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 地图图层
/// </summary>
[System.Serializable]
public class MapLayer
{
    public string name;
    public int orderIndex;
    public Dictionary<string, List<MapObject>> objects = new Dictionary<string, List<MapObject>>();

    public string[] PrefabNames
    {
        get
        {
            List<string> names = new List<string>();
            foreach (KeyValuePair<string, List<MapObject>> kv in objects)
            {
                names.Add(kv.Key);
            }
            return names.ToArray();
        }
    }

    public MapLayer(string setName)
    {
        this.name = setName;
    }
    public void Add(MapObject obj)
    {
        if (objects.ContainsKey(obj.prefab))
        {
            objects[obj.prefab].Add(obj);
        }
        else
        {
            var list = new List<MapObject>();
            list.Add(obj);
            objects.Add(obj.prefab, list);
        }
    }

    public bool Contains(MapObject obj)
    {
        if (objects.ContainsKey(obj.prefab))
        {
           return objects[obj.prefab].Contains(obj);
        }
        else
        {
            return false;
        }
    }

    public void Remove(MapObject obj)
    {
        if (objects.ContainsKey(obj.prefab))
        {
             objects[obj.prefab].Remove(obj);
        }
    }
}
