using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// MVVM架构的ViewModel
    /// </summary>
    public   interface IContext
    {
        /// <summary>
        /// 每个Context类型唯一的ID值
        /// </summary>
//        string TypeID { get; }

        /// <summary>
        /// 从Model中获取数据，并提交给View
        /// </summary>
        void GetData();

        /// <summary>
        /// 将View中的数据保存至Model中
        /// </summary>
        void SetData();
    }
}
