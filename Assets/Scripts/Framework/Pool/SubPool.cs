using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPool
{

    /// <summary>
    /// 预设
    /// </summary>
    GameObject m_prefab;
    /// <summary>
    /// 游戏物体集合
    /// </summary>
    List<GameObject> m_objects = new List<GameObject>();
    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
        get { return m_prefab.name; }
    }
    public SubPool(GameObject prefab)
    {
        this.m_prefab = prefab;
    }
    /// <summary>
    /// 生产对象
    /// </summary>
    /// <returns>对象</returns>
    public GameObject Spawn()
    {
        GameObject go = null;
        foreach (GameObject obj in this.m_objects)
        {
            if (!obj.activeSelf)
            {
                go = obj;
                break;
            }
        }
        if (go == null)
        {
            go = GameObject.Instantiate<GameObject>(m_prefab);
            m_objects.Add(go);
        }
        go.SetActive(true);
        go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        return go;
    }
    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="go"></param>
    public void Unspawn(GameObject go)
    {
        if (!Contains(go)) return;
        go.SendMessage("OnUnspawn", SendMessageOptions.DontRequireReceiver);
        go.SetActive(false);
    }
    /// <summary>
    /// 回收全部对象
    /// </summary>
    public void UnspawnAll()
    {
        foreach (GameObject obj in m_objects)
        {
            if (obj.activeSelf)
            {
                Unspawn(obj);
            }
        }
    }
    /// <summary>
    /// 是否存在对象
    /// </summary>
    /// <param name="go">回收对象</param>
    /// <returns>是否在对象池中</returns>
    public bool Contains(GameObject go)
    {
        return m_objects.Contains(go);
    }
}
