using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 自定义编辑器指令
    /// </summary>
    public interface ICustomCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="go"></param>
        void Execute(GameObject go);

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="go"></param>
        void Undo(GameObject go);
    }
}
