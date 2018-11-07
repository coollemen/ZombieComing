using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    /// <summary>
    /// 地图块的工厂，预制地图块都由工厂创建
    /// </summary>
    public class BlockFactory:Singleton<BlockFactory>
    {
        public static Block CreateGrassBlock()
        {
            return null;
        }
    }
}
