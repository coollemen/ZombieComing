using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏属性类
/// </summary>
public class GameProperty<T>  {
    /// <summary>
    /// 当前值
    /// </summary>
    public T value;
    /// <summary>
    /// 最小值
    /// </summary>
    public T min;
    /// <summary>
    /// 最大值
    /// </summary>
    public T max;
    public GameProperty()
    {

    }
    public GameProperty(T setValue,T setMin,T setMax)
    {
        this.value = setValue;
        this.min = setMin;
        this.max = setMax;
    }

}
