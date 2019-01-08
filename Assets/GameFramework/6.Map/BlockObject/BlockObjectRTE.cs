using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 图块物体实时编辑组件
    /// </summary>
    public class BlockObjectRTE : BlockObject
    {
        /// <summary>
        /// 是否图块定义需要更新
        /// </summary>
        public bool isDefDirty = false;

        public bool isInit = false;
        public void Init()
        {
            this.LoadFromData();
            this.CreateBlockPool();

            isInit = true;
        }
        //        public void SetMeshToMeshFilter()
        //        {
        //            this.GetComponent<MeshFilter>().mesh = this.mesh;
        //        }
    }
}