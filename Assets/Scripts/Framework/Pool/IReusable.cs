using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReusable  {
    /// <summary>
    /// 取出时调用
    /// </summary>
    void OnSpawn();
    /// <summary>
    /// 当回收时调用
    /// </summary>
    void OnUnspawn();
}
