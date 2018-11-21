﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
namespace GameFramework
{
    
    /// <summary>
    /// 地图块的定义，用于创建地图块
    /// </summary>
    [System.Serializable]
    public class BlockDefinition
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id;
        /// <summary>
        /// 名字
        /// </summary>
        public string name;
        /// <summary>
        /// 图标名字
        /// </summary>
        public string iconName;
        //方向（指的是前面所面朝的方向）
        public BlockDirection direction = BlockDirection.Front;
        
        //前面贴图的坐标
        public byte textureFrontX;
        public byte textureFrontY;

        //后面贴图的坐标
        public byte textureBackX;
        public byte textureBackY;

        //右面贴图的坐标
        public byte textureRightX;
        public byte textureRightY;

        //左面贴图的坐标
        public byte textureLeftX;
        public byte textureLeftY;

        //上面贴图的坐标
        public byte textureTopX;
        public byte textureTopY;

        //下面贴图的坐标
        public byte textureBottomX;
        public byte textureBottomY;
        //都是A面的方块
        public BlockDefinition(byte id, string name, byte textureX, byte textureY)
            : this(id, name, textureX, textureY, textureX, textureY, textureX, textureY, textureX, textureY)
        {
        }

        //上面是A，其他面是B的方块
        public BlockDefinition(byte id, string name, byte textureX, byte textureY, byte textureTopX, byte textureTopY)
            : this(id, name, textureX, textureY, textureX, textureY, textureX, textureY, textureX, textureY,
                textureTopX, textureTopY, textureX, textureY)
        {
        }

        //上面是A，下面是B，其他面是C的方块
        public BlockDefinition(byte id, string name, byte textureX, byte textureY, byte textureTopX, byte textureTopY,
            byte textureBottomX, byte textureBottomY)
            : this(id, name, textureX, textureY, textureX, textureY, textureX, textureY, textureX, textureY,
                textureTopX, textureTopY, textureBottomX, textureBottomY)
        {
        }

        //上面是A，下面是B，前面是C，其他面是D的方块
        public BlockDefinition(byte id, string name, byte textureFrontX, byte textureFrontY, byte textureX, byte textureY,
            byte textureTopX, byte textureTopY, byte textureBottomX, byte textureBottomY)
            : this(id, name, textureFrontX, textureFrontY, textureX, textureY, textureX, textureY, textureX, textureY,
                textureTopX, textureTopY, textureBottomX, textureBottomY)
        {
        }

        //上下左右前后面都不一样的方块
        public BlockDefinition(byte id, string name, byte textureFrontX, byte textureFrontY, byte textureBackX, byte textureBackY,
            byte textureRightX, byte textureRightY,
            byte textureLeftX, byte textureLeftY, byte textureTopX, byte textureTopY, byte textureBottomX,
            byte textureBottomY)
        {
            this.id = id;
            this.name = name;

            this.textureFrontX = textureFrontX;
            this.textureFrontY = textureFrontY;

            this.textureBackX = textureBackX;
            this.textureBackY = textureBackY;

            this.textureRightX = textureRightX;
            this.textureRightY = textureRightY;

            this.textureLeftX = textureLeftX;
            this.textureLeftY = textureLeftY;

            this.textureTopX = textureTopX;
            this.textureTopY = textureTopY;

            this.textureBottomX = textureBottomX;
            this.textureBottomY = textureBottomY;
        }
    }
}
