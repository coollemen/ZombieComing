using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool:Singleton<ObjectPool>
{
    public string ResourceDir = "";
    Dictionary<string, SubPool> m_pools = new Dictionary<string, SubPool>();
    /// <summary>
    /// 生产对象
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject Spawn(string name)
    {
        if (!m_pools.ContainsKey(name))
        {
            RegisterNew(name);
        }

        SubPool pool = m_pools[name];
        return pool.Spawn();
    }
    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="go"></param>
    public void Unspawn(GameObject go)
    {
        SubPool pool = null;
        foreach (SubPool p in m_pools.Values)
        {
            if (p.Contains(go))
            {
                pool = p;
                break;
            }
        }

        pool.Unspawn(go);
    }
    /// <summary>
    /// 回收所有对象
    /// </summary>
    public void UnspawnAll()
    {
        foreach (SubPool p in m_pools.Values)
        {
            p.UnspawnAll();
        }
    }
    /// <summary>
    /// 注册新的子对象池
    /// </summary>
    /// <param name="name"></param>
    public void RegisterNew(string name)
    {
        //创建路径
        string path = "";
        if (string.IsNullOrEmpty(ResourceDir))
        {
            path = name;
        }
        else
        {
            path = ResourceDir + "/" + name;
        }
        //加载预设
        GameObject prefab = Resources.Load<GameObject>(path);
        //创建子对象池
        SubPool pool = new SubPool(prefab);
        m_pools.Add(pool.Name, pool);
    }
}