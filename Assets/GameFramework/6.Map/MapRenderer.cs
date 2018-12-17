using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 地图渲染器，单例，一个场景只能存在一个
    /// </summary>
    public class MapRenderer : MonoSingleton<MapRenderer>
    {
        public BlockTerrainData data;
        /// <summary>
        /// 视图中心位置点
        /// </summary>
        public Vector2Int viewCenterPosition;
        /// <summary>
        /// 视图长度
        /// </summary>
        public int viewLength;
        /// <summary>
        /// 视图宽度
        /// </summary>
        public int viewWidth;
        /// <summary>
        ///是否需要从新渲染
        /// </summary>
        public bool isDirty;
        // Use this for initialization
        void Start()
        {
            this.viewLength = 3;
            this.viewWidth = 3;
            this.isDirty = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitViewChunks()
        {
            
        }

        public void ViewMoveUp()
        {
            
        }

        public void ViewMoveDown()
        {
            
        }

        public void ViewMoveLeft()
        {
            
        }

        public void ViewMoveRight()
        {
            
        }
    }
}
