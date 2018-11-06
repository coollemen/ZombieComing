using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 视图位置类型
    /// </summary>
    public enum UIViewType 
    {
        /// <summary>
        /// 游戏中的血条等
        /// </summary>
        Hud,
        /// <summary>
        /// 固定窗口，技能、人物信息等
        /// </summary>
        Fixed,
        /// <summary>
        /// 普通窗口，商城等
        /// </summary>
        Normal,
        /// <summary>
        /// 置顶显示UI，体力，金钱等
        /// </summary>
        Top,
        /// <summary>
        /// 弹出窗口，显示在最顶层
        /// </summary>
        Popup
    }
}