using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
     /// <summary>
     /// 地图管理类 
     /// </summary>
    public class MapManager : MonoSingleton<MapManager>
     {
         public List<Block> blockPool = new List<Block>();
         public BlockConfig blockConfig;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

         public void CreateBlockPool()
         {
             
         }
    }
}