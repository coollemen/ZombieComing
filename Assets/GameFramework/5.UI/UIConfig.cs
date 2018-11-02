using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public enum UILoadMode
    {
        /// <summary>
        /// 在Awake函数里直接实例化
        /// </summary>
        LoadOnAwake,

        /// <summary>
        /// 在用的时候去实例化
        /// </summary>
        LoadOnUse
    }

    public enum UIUnLoadMode
    {
        /// <summary>
        /// 关闭后销毁
        /// </summary>
        UnloadAfterClose,

        /// <summary>
        /// 不要销毁
        /// </summary>
        DontUnload
    }
    [System.Serializable]
    public class UIPath
    {
        public string prefabPath;
        public UILoadMode loadMode;
        public UIUnLoadMode unloadMode;
    }

    [CreateAssetMenu(fileName ="UIConfig.asset",menuName ="GameFramework/UI Config Asset")]
    [System.Serializable]
    public class UIConfig : ScriptableObject
    {
        public List<UIPath> paths = new List<UIPath>();
    }
}